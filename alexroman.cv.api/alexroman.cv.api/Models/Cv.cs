using System;
using System.Collections.Generic;
using System.Linq;

namespace alexroman.cv.api
{
    public class Cv
    {
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> Frameworks { get; set; }
        public IEnumerable<string> Tools { get; set; }
        public IEnumerable<string> OpenSourceProjects { get; set; }
        public IEnumerable<Experience> Experience { get; set; }
        public string Email { get; set; }

        public IEnumerable<SearchResult> Search(string keyword)
        {
            var searchResult = new List<SearchResult>();
            IEnumerable<SearchResult> search(IEnumerable<string> location, string locationName = "")
                => location
                    .Where(value => value?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
                    ?.Select(result => new SearchResult
                    {
                        Location = locationName,
                        Result = result
                    }) ?? Enumerable.Empty<SearchResult>();

            searchResult.AddRange(search(Languages, nameof(Languages)));
            searchResult.AddRange(search(Frameworks, nameof(Frameworks)));
            searchResult.AddRange(search(Tools, nameof(Tools)));
            searchResult.AddRange(search(OpenSourceProjects, nameof(OpenSourceProjects)));
            searchResult.AddRange(search(new[] { Email }, nameof(Email)));

            foreach (var exp in Experience)
            {
                var experienceSearchResults = search(new[]
                { exp.Company, exp.Position, exp.Location, exp.StartDate.ToString("m"), exp.EndDate?.ToString("m") });

                if (experienceSearchResults.Any())
                {
                    searchResult.Add(new SearchResult
                    {
                        Location = nameof(Experience),
                        Result = exp
                    });
                }
            }

            return searchResult;
        }
    }
}   
