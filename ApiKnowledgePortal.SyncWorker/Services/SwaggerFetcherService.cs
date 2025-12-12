using System.Text.Json;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.SyncWorker.Services
{
    public class SwaggerFetcherService
    {
        private readonly ISwaggerSourceRepository _sourceRepository;
        private readonly IApiSpecRepository _apiSpecRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SwaggerParserService _parser;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SwaggerFetcherService> _logger;

        public SwaggerFetcherService(
            ISwaggerSourceRepository sourceRepository,
            IApiSpecRepository apiSpecRepository,
            IUnitOfWork unitOfWork,
            SwaggerParserService parser,
            HttpClient httpClient,
            ILogger<SwaggerFetcherService> logger)
        {
            _sourceRepository = sourceRepository;
            _apiSpecRepository = apiSpecRepository;
            _unitOfWork = unitOfWork;
            _parser = parser;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task FetchAllAsync(CancellationToken cancellationToken = default)
        {
            var sources = await _sourceRepository.GetAllActiveAsync(cancellationToken);
            foreach (var source in sources)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await FetchAndProcessSingleSourceAsync(source, cancellationToken);
            }
        }

        public async Task FetchOneAsync(Guid sourceId, CancellationToken cancellationToken = default)
        {
            var source = await _sourceRepository.GetByIdAsync(sourceId, cancellationToken);
            if (source == null || !source.IsActive)
                return;

            await FetchAndProcessSingleSourceAsync(source, cancellationToken);
        }

        private async Task FetchAndProcessSingleSourceAsync(
            Domain.SwaggerSources.SwaggerSource source,
            CancellationToken ct)
        {
            string? json = null;

            // фетчинг
            try
            {
                using var resp = await _httpClient.GetAsync(source.Url, ct);
                resp.EnsureSuccessStatusCode();
                json = await resp.Content.ReadAsStringAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch from {Url}", source.Url);
                source.MarkFetched($"Error: {ex.Message}");
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }

            // извлечение версии
            string version = "unknown";
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("info", out var infoEl) &&
                    infoEl.TryGetProperty("version", out var verEl))
                {
                    version = verEl.GetString() ?? "unknown";
                }
            }
            catch
            {
                // игнорим
            }

            // всегда создается запись фетчинга
            var apiSpec = new ApiSpecifications(
                ApiSpecId.NewId(),
                source.Id,
                source.Name,
                version,
                json
            );

            await _apiSpecRepository.AddAsync(apiSpec, ct);
            source.MarkFetched("Fetched");
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation(
                "New ApiSpec created for {Name} ({Url}) with id {SpecId}",
                source.Name, source.Url, apiSpec.Id.Value);

            // проверка если контент дублируется
            var previousSpecs = await _apiSpecRepository.Query()
                .Where(s => s.SwaggerSourceId == source.Id && s.Id != apiSpec.Id)
                .ToListAsync(ct);

            bool isDuplicate = previousSpecs.Any(s => AreJsonDocumentsEqual(json, s.Content));

            if (isDuplicate)
            {
                _logger.LogInformation(
                    "ApiSpec {SpecId}: content is duplicate - parsing skipped.",
                    apiSpec.Id.Value);

                source.MarkFetched("Fetched (duplicate, parse skipped)");
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }

            // парсинг новых уникальных спеков
            try
            {
                await _parser.ParseAndSaveAsync(apiSpec);
                _logger.LogInformation("Parse completed for ApiSpec {SpecId}", apiSpec.Id.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Parse failed for ApiSpec {SpecId}", apiSpec.Id.Value);
                source.MarkFetched("Fetched but parse failed");
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }

        // сравнение джсонов

        private static bool AreJsonDocumentsEqual(string json1, string json2)
        {
            try
            {
                using var doc1 = JsonDocument.Parse(json1);
                using var doc2 = JsonDocument.Parse(json2);
                return JsonElementDeepEquals(doc1.RootElement, doc2.RootElement);
            }
            catch
            {
                return false;
            }
        }

        private static bool JsonElementDeepEquals(JsonElement e1, JsonElement e2)
        {
            if (e1.ValueKind != e2.ValueKind)
                return false;

            switch (e1.ValueKind)
            {
                case JsonValueKind.Object:
                    {
                        var p1 = e1.EnumerateObject().OrderBy(p => p.Name).ToList();
                        var p2 = e2.EnumerateObject().OrderBy(p => p.Name).ToList();

                        if (p1.Count != p2.Count)
                            return false;

                        for (int i = 0; i < p1.Count; i++)
                        {
                            if (p1[i].Name != p2[i].Name)
                                return false;

                            if (!JsonElementDeepEquals(p1[i].Value, p2[i].Value))
                                return false;
                        }
                        return true;
                    }

                case JsonValueKind.Array:
                    {
                        var a1 = e1.EnumerateArray().ToList();
                        var a2 = e2.EnumerateArray().ToList();

                        if (a1.Count != a2.Count)
                            return false;

                        for (int i = 0; i < a1.Count; i++)
                        {
                            if (!JsonElementDeepEquals(a1[i], a2[i]))
                                return false;
                        }
                        return true;
                    }

                case JsonValueKind.String:
                    return e1.GetString() == e2.GetString();

                case JsonValueKind.Number:
                    return Math.Abs(e1.GetDouble() - e2.GetDouble()) < double.Epsilon;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return e1.GetBoolean() == e2.GetBoolean();

                case JsonValueKind.Null:
                    return true;

                default:
                    return false;
            }
        }
    }
}