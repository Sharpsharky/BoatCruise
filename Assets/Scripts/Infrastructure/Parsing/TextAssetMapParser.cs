using SailingBoat.Domain.Grid;
using SailingBoat.Domain.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SailingBoat.Infrastructure.Parsing
{
    public class TextAssetMapParser : IMapParser
    {
        public async Task<HexGrid> ParseAsync(TextAsset asset)
        {
            await Task.Yield();
            if (asset == null)
                throw new ArgumentNullException(nameof(asset), "Map TextAsset is null");

            var rawLines = asset.text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int height = rawLines.Length;
            if (height == 0)
                throw new InvalidOperationException("Map data is empty");

            var tokenRows = new List<string[]>();
            foreach (var line in rawLines)
            {
                var trimmed = line.Trim();
                var tokens = trimmed.Split(
                    new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 1)
                {
                    tokens = trimmed.Select(c => c.ToString()).ToArray();
                }
                tokenRows.Add(tokens);
            }

            int width = tokenRows[0].Length;
            for (int r = 1; r < height; r++)
            {
                if (tokenRows[r].Length != width)
                    throw new InvalidOperationException(
                        $"Row {r} length {tokenRows[r].Length} != expected {width}");
            }

            var grid = new HexGrid(width, height);
            for (int r = 0; r < height; r++)
            {
                for (int q = 0; q < width; q++)
                {
                    var token = tokenRows[r][q];
                    var type = token switch
                    {
                        "0" => TileType.Water,
                        "1" => TileType.Terrain,
                        _ => throw new FormatException(
                            $"Invalid tile token '{token}' at ({q},{r})")
                    };
                    grid.SetTile(q, r, new HexTile(q, r, type));
                }
            }

            return grid;
        }
    }
}