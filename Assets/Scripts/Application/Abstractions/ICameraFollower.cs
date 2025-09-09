using UnityEngine;

namespace SailingBoat.Application.Abstractions
{
    public interface ICameraFollower
    {
        void ChangeTarget(Transform target);
    }
}