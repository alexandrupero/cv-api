using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
                        Name = "search-languages",
                        Params = new string[] { "search_term" }
                    }
                }
            };

            return Task.FromResult(systemDescribe);
        }

        [JsonRpcMethod("search-languages", 0)]
        public async Task<IEnumerable<string>> SearchLanguagesAsync(string search_term)
        {
            if (string.IsNullOrWhiteSpace(search_term) || search_term.Length < 3)
            {
                throw new JsonRpcServiceException(100L, "Please enter at least 3 characters for \"search_term\".");
            }

            var cv = await _db.GetCvAsync();

            return cv.Languages.Where(l => l.Name.Contains(search_term, StringComparison.OrdinalIgnoreCase)).Select(l => l.Name);
        }

        [JsonRpcMethod("languages")]
        public async Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var cv = await _db.GetCvAsync();

            return cv.Languages.Select(l => l.Name);
        }
    }


}
