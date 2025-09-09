using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace SailingBoat.Presentation.UI.Label
{
    public class StartPromptUI : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private RectTransform titleTransform;
        [SerializeField] private RectTransform labelTransform;
        [SerializeField] private float moveDistance = 200f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.OutCubic;
        [SerializeField] private float rotationAngle = 15f;

        public void Show()
        {
            Animate(
                offset: Vector3.up * moveDistance,
                startAngle: rotationAngle,
                endAngle: 0f,
                deactivateOnComplete: false
            );
        }

        public void HideAnimated()
        {
            Animate(
                offset: Vector3.up * moveDistance,
                startAngle: 0f,
                endAngle: rotationAngle,
                deactivateOnComplete: true
            );
        }

        private void Animate(Vector3 offset, float startAngle, float endAngle, bool deactivateOnComplete)
        {
            titleTransform.DOKill();
            labelTransform.DOKill();

            var titleOriginal = titleTransform.localPosition;
            var labelOriginal = labelTransform.localPosition;
            var titleFrom = titleOriginal + (deactivateOnComplete ? Vector3.zero : offset);
            var labelFrom = labelOriginal + (deactivateOnComplete ? Vector3.zero : offset);
            var titleTo = titleOriginal + (deactivateOnComplete ? offset : Vector3.zero);
            var labelTo = labelOriginal + (deactivateOnComplete ? offset : Vector3.zero);

            titleTransform.localPosition = titleFrom;
            labelTransform.localPosition = labelFrom;
            titleTransform.localRotation = Quaternion.Euler(0, 0, startAngle);
            labelTransform.localRotation = Quaternion.Euler(0, 0, -startAngle);

            var seq = DOTween.Sequence();
            seq.Join(titleTransform.DOLocalMove(titleTo, moveDuration).SetEase(moveEase));
            seq.Join(titleTransform.DOLocalRotate(Vector3.forward * endAngle, moveDuration).SetEase(moveEase));

            seq.Join(labelTransform.DOLocalMove(labelTo, moveDuration).SetEase(moveEase));
            seq.Join(labelTransform.DOLocalRotate(Vector3.forward * -endAngle, moveDuration).SetEase(moveEase));

            if (deactivateOnComplete)
            {
                seq.OnComplete(() => gameObject.SetActive(false));
            }

            gameObject.SetActive(true);
            seq.Play();
        }
    }
}