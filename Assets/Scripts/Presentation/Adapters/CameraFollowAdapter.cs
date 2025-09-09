using SailingBoat.Application.Abstractions;
using UnityEngine;

namespace SailingBoat.Application.Camera
{
    public class CameraFollowAdapter : MonoBehaviour, ICameraFollower
    {
        [SerializeField] private CameraFollow cameraFollow;
        public void ChangeTarget(Transform target) => cameraFollow.ChangeTarget(target);
    }
}