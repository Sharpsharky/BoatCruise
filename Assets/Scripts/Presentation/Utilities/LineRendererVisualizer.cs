using SailingBoat.Domain.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace SailingBoat.Presentation.Utilities
{
    public class LineRendererVisualizer : MonoBehaviour
    {
        public LineRenderer lineRenderer;

        public void ShowPath(IList<HexTile> path)
        {
            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                var worldPos = path[i].GetMiddle();
                worldPos.y = 0.1f;
                lineRenderer.SetPosition(i, worldPos);
            }
        }
    }
}