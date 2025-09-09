using System.Threading.Tasks;

namespace SailingBoat.Domain.Parsing
{
    public interface IMapParser
    {
        Task<Grid.HexGrid> ParseAsync(UnityEngine.TextAsset asset);
    }
}