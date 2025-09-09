using System.Threading.Tasks;

namespace SailingBoat.Infrastructure.Addressables
{
    public interface IAsyncLoader
    {
        Task<T> LoadAsync<T>(string key);
    }
}