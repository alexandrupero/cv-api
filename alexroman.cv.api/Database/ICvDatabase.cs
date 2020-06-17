using System.Threading.Tasks;

namespace alexroman.cv.api
{
    public interface ICvDatabase
    {
        Task<Cv> GetCvAsync();
    }
}
