using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Configuration;

namespace alexroman.cv.api
{
    public class CvDatabase : ICvDatabase
    {
        private readonly IAppCache _cache;
        private readonly IConfiguration _configuration;

        public CvDatabase(IAppCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<Cv> GetCvAsync()
        {
            async Task<Cv> cvObjectFactory()
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                using FileStream fs = File.OpenRead(_configuration["Cv:File.Path"]);
                return await JsonSerializer.DeserializeAsync<Cv>(fs, options);
            };
            return await _cache.GetOrAddAsync(_configuration["Cv:Cache.Key"], cvObjectFactory, new TimeSpan(0, 5, 0));
        }
    }
}
