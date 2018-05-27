using System.Net.Http;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Services
{
    public interface IWorkoutTrackerHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
