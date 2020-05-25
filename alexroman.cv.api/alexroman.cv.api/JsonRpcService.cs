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
        private readonly string[] _technologies = {
            ".NET", "SQL Server", "Javascript", "HTML", "CSS",
            "MS Test", "NUnit", "Jquery", "Angular", "Typescript"
        };

        [JsonRpcMethod("technologies", "search")]
        public Task<string[]> GetTechnologiesAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search) || search.Length < 3)
            {
                throw new JsonRpcServiceException(100L, "Please enter at least 3 characters for \"search\".");
            }

            if (search == "all")
            {
                return Task.FromResult(_technologies);
            }

            return Task.FromResult(_technologies.Where(tech => tech.Contains(search, StringComparison.OrdinalIgnoreCase)).ToArray());
        }
    }
}
