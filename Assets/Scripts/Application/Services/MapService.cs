using SailingBoat.Domain.Grid;
using SailingBoat.Domain.Parsing;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SailingBoat.Application.Services
{
    public class MapService
    {
        private readonly IMapParser _parser;
        public event Action<HexGrid> OnMapReady;

        public MapService(IMapParser parser)
        {
            _parser = parser;
        }

        public async Task LoadMapAsync(TextAsset mapAsset)
        {
            var grid = await _parser.ParseAsync(mapAsset);
            OnMapReady?.Invoke(grid);
        }
    }
}
