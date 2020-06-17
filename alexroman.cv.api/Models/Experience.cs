using System;

namespace alexroman.cv.api
{
    public class Experience
    {
        public string Company { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
