using SailingBoat.Domain.Grid;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SailingBoat.Application.Services
{
    public class BoatService
    {
        public event Action<Vector3> OnBoatMoved;
        public event Action<Quaternion> OnBoatRotated;
        public event Action OnBoatStartedMoving;
        public event Action OnBoatStoppedMoving;

        private bool _isMoving = false;

        public async Task MoveBoatAlongPathAsync(IList<HexTile> path, float moveSpeed, float rotationSpeed, CancellationToken token,
            Vector3 startWorldPos, Quaternion startRot)
        {
            try
            {
                await FollowPathAsync(path, moveSpeed, rotationSpeed, token, startWorldPos, startRot);
            }
            catch (OperationCanceledException) { }
        }

        private async Task FollowPathAsync(IList<HexTile> path, float moveSpeed, float rotationSpeedDegPerSec, CancellationToken token,
            Vector3 startWorldPos, Quaternion startRot)
        {
            if (path == null || path.Count < 2)
                return;

            var current = startWorldPos;
            OnBoatMoved?.Invoke(current);
            token.ThrowIfCancellationRequested();

            var prevRotation = startRot;

            for (int i = 1; i < path.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                var next = path[i].GetMiddle();
                var dir = (next - current).normalized;
                var targetRot = Quaternion.LookRotation(dir);

                float deltaAngle = Quaternion.Angle(prevRotation, targetRot);

                if (i == 1 || deltaAngle > 1f)
                    await RotateTowardsAsync(prevRotation, targetRot, rotationSpeedDegPerSec, token);
                else
                {
                    _isMoving = false;
                    OnBoatRotated?.Invoke(targetRot);
                    OnBoatStoppedMoving?.Invoke();
                }

                bool isLastStep = i == (path.Count - 1);
                await MoveToAsync(current, next, moveSpeed, isLastStep, token);

                current = next;
                prevRotation = targetRot;
            }
        }

        private async Task RotateTowardsAsync(Quaternion startRot, Quaternion targetRot, float rotationSpeedDegPerSec, CancellationToken token)
        {
            Quaternion currentRot = startRot;

            while (Quaternion.Angle(currentRot, targetRot) > 0.1f)
            {
                token.ThrowIfCancellationRequested();

                currentRot = Quaternion.RotateTowards(
                    currentRot,
                    targetRot,
                    rotationSpeedDegPerSec * Time.deltaTime
                );

                OnBoatRotated?.Invoke(currentRot);

                await Task.Yield();
            }

            OnBoatRotated?.Invoke(targetRot);
        }

        private async Task MoveToAsync(Vector3 from, Vector3 to, float moveSpeed, bool isLastStep, CancellationToken token)
        {
            if (!_isMoving)
            {
                OnBoatStartedMoving?.Invoke();
                _isMoving = true;
            }

            float distance = Vector3.Distance(from, to);
            float duration = distance / moveSpeed;

            float t = 0f;
            while (t < duration)
            {
                token.ThrowIfCancellationRequested();
                t += Time.deltaTime;
                var pos = Vector3.Lerp(from, to, t / duration);
                OnBoatMoved?.Invoke(pos);
                await Task.Yield();
            }

            if (isLastStep)
            {
                _isMoving = false;
                OnBoatStoppedMoving?.Invoke();
            }

            OnBoatMoved?.Invoke(to);
        }
    }
}