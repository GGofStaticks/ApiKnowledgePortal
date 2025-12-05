using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using ApiKnowledgePortal.SyncWorker.Services;
using Hangfire;

namespace ApiKnowledgePortal.SyncWorker.Jobs
{
    public class SwaggerParserJob
    {
        private readonly IApiSpecRepository _apiSpecRepo;
        private readonly SwaggerParserService _parser;

        public SwaggerParserJob(IApiSpecRepository apiSpecRepo, SwaggerParserService parser)
        {
            _apiSpecRepo = apiSpecRepo;
            _parser = parser;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ExecuteAsync()
        {
            var allSpecs = await _apiSpecRepo.GetAllAsync();
            foreach (var spec in allSpecs)
            {
                await _parser.ParseAndSaveAsync(spec);
            }
        }
    }
}