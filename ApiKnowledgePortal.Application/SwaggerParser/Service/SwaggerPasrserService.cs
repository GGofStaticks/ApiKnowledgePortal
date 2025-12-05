using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;

namespace ApiKnowledgePortal.Application.SwaggerParser.Service
{
    public class SwaggerParserService
    {
        private readonly IParsedApiSpecRepository _parsedRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SwaggerParserService(IParsedApiSpecRepository parsedRepo, IUnitOfWork unitOfWork)
        {
            _parsedRepo = parsedRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task ParseAndSaveAsync(ApiSpecifications apiSpec)
        {
                using var doc = JsonDocument.Parse(apiSpec.Content);
                if (!doc.RootElement.TryGetProperty("paths", out var paths))
                    return;

                foreach (var pathProp in paths.EnumerateObject())
                {
                    var path = pathProp.Name;
                    var methods = pathProp.Value.EnumerateObject();

                    foreach (var methodProp in methods)
                    {
                        var method = methodProp.Name.ToUpper();
                        var operationId = methodProp.Value.GetProperty("operationId").GetString() ?? "";
                        var summary = methodProp.Value.GetProperty("summary").GetString() ?? "";

                        var parsed = new ParsedApiSpec(apiSpec.Id.Value, path, method, operationId, summary);
                        await _parsedRepo.AddAsync(parsed);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
        }
    }
}