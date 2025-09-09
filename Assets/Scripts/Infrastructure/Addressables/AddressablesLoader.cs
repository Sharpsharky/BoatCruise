using System.Threading.Tasks;

namespace SailingBoat.Infrastructure.Addressables
{
    public class AddressablesLoader : IAsyncLoader
    {
        public async Task<T> LoadAsync<T>(string key)
        {
            return await UnityEngine.AddressableAssets.Addressables
                .LoadAssetAsync<T>(key)
                .Task;
        }
    }
}
