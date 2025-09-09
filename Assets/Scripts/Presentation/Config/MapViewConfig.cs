using UnityEngine;

namespace SailingBoat.Presentation.Config
{
    [CreateAssetMenu(menuName = "Config/Map View Config", fileName = "MapViewConfig")]
    public class MapViewConfig : ScriptableObject
    {
        [Range(0f, 1f)]
        public float decorationChance = 0.3f;
        public float backgroundSizeOffset = 15f;
    }
}