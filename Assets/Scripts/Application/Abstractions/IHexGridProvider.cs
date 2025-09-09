using SailingBoat.Domain.Grid;

namespace SailingBoat.Application.Abstractions
{
    public interface IHexGridProvider
    {
        HexGrid CurrentGrid { get; }
    }
}