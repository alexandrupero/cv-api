namespace alexroman.cv.api
{
    public class SystemDescribe
    {
        public Proc[] Procs { get; set; }
    }

    public class Proc
    {
        public string Name { get; set; }

        public string[] Params { get; set; }
    }
}
