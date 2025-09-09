using Unity.Cinemachine;
using UnityEngine;

namespace SailingBoat.Application.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private CinemachinePositionComposer positionComposer;

        private CinemachineCamera _cinemachineCamera;

        public void Initialize(CinemachineCamera cinemachineCamera, Transform target)
        {
            _cinemachineCamera = cinemachineCamera;
            cinemachineCamera.Follow = target;
        }

        public void ChangeTarget(Transform target)
        {
            _cinemachineCamera.Follow = target;
        }
    }
}
