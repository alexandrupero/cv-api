using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anemonis.AspNetCore.JsonRpc;

namespace alexroman.cv.api
{
    [JsonRpcRoute("/api")]
    public class JsonRpcService : IJsonRpcService
    {
        private readonly ICvDatabase _db;

        public JsonRpcService(ICvDatabase db)
        {
            _db = db;
        }

        [JsonRpcMethod("languages", "search")]
        public async Task<IEnumerable<Language>> GetLanguagesAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search) || search.Length < 3)
            {
                throw new JsonRpcServiceException(100L, "Please enter at least 3 characters for \"search\".");
            }

            var cv = await _db.GetCvAsync();

            if (search == "all")
            {
                return cv.Languages;
            }

            return cv.Languages.Where(l => l.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }
    }
}
