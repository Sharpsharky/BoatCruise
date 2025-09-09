using UnityEngine;

namespace SailingBoat.Presentation.Config
{
    [CreateAssetMenu(menuName = "Config/Boat View Config", fileName = "BoatViewConfig")]
    public class BoatViewConfig : ScriptableObject
    {
        [Header("Movement Settings")]
        public float moveSpeed = 2f;

        [Header("Rotation Settings")]
        public float rotationSpeed = 360f;
    }
}