using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.SyncWorker.Services;
using Hangfire;

namespace ApiKnowledgePortal.SyncWorker.Jobs
{
    public class SwaggerFetcherJob
    {
        private readonly SwaggerFetcherService _fetcherService;

        public SwaggerFetcherJob(SwaggerFetcherService fetcherService)
        {
            _fetcherService = fetcherService;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ExecuteAsync()
        {
            await _fetcherService.FetchAllAsync();
        }
    }
}