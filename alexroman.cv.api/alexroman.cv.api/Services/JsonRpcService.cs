using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anemonis.AspNetCore.JsonRpc;
using Microsoft.Extensions.Configuration;

namespace alexroman.cv.api
{
    [JsonRpcRoute("/api")]
    public class JsonRpcService : IJsonRpcService
    {
        private readonly ICvDatabase _db;
        private readonly IConfiguration _configuration;

        public JsonRpcService(ICvDatabase db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [JsonRpcMethod("system.describe")]
        public Task<SystemDescribe> GetSystemDescribeAsync()
        {
            var systemDescribe = new SystemDescribe
            {
                Procs = new Proc[]
                {
                    new Proc
                    {
                        Name = "languages",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "frameworks",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "tools",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "open-source-projects",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "experience",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "contact",
                        Params = new string[]{ }
                    },
                    new Proc
                    {
                        Name = "search",
                        Params = new string[] { "keyword" }
                    }
                }
            };

            return Task.FromResult(systemDescribe);
        }

        [JsonRpcMethod("search", 0)]
        public async Task<IEnumerable<SearchResult>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
            {
                throw new JsonRpcServiceException(100L, $"Please enter at least 2 characters for {nameof(keyword)}.");
            }

            var cv = await _db.GetCvAsync();

            return cv.Search(keyword);
        }

        [JsonRpcMethod("languages")]
        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Languages;
        }

        [JsonRpcMethod("frameworks")]
        public async Task<IEnumerable<string>> GetFrameworksAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Frameworks;
        }

        [JsonRpcMethod("tools")]
        public async Task<IEnumerable<string>> GetToolsAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Tools;
        }

        [JsonRpcMethod("open-source-projects")]
        public async Task<IEnumerable<string>> GetOpenSourceProjectsAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.OpenSourceProjects;
        }

        [JsonRpcMethod("experience")]
        public async Task<IEnumerable<Experience>> GetExperienceAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Experience;
        }

        [JsonRpcMethod("contact")]
        public async Task<string> GetContactAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Email;
        }
    }
}
