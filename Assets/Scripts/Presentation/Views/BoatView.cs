using SailingBoat.Application.Services;
using UnityEngine;

namespace SailingBoat.Presentation.Views
{
    public class BoatView : MonoBehaviour
    {
        [SerializeField] private BoatFoamView boatFoamView;

        private BoatService _boatService;

        public void Initialize(BoatService boatService)
        {
            if (_boatService != null)
            {
                _boatService.OnBoatMoved -= HandleMoved;
                _boatService.OnBoatRotated -= HandleRotated;
                _boatService.OnBoatStartedMoving -= HandleBoatStartedMoving;
                _boatService.OnBoatStoppedMoving -= HandleBoatStoppedMoving;
            }

            _boatService = boatService;

            _boatService.OnBoatMoved += HandleMoved;
            _boatService.OnBoatRotated += HandleRotated;
            _boatService.OnBoatStartedMoving += HandleBoatStartedMoving;
            _boatService.OnBoatStoppedMoving += HandleBoatStoppedMoving;
        }

        private void HandleMoved(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        private void HandleRotated(Quaternion newRotation)
        {
            transform.rotation = newRotation;
        }

        private void HandleBoatStartedMoving()
        {
            boatFoamView.ActivateEmission(true);
        }

        private void HandleBoatStoppedMoving()
        {
            boatFoamView.ActivateEmission(false);
        }

        private void OnDestroy()
        {
            if (_boatService != null)
            {
                _boatService.OnBoatMoved -= HandleMoved;
                _boatService.OnBoatRotated -= HandleRotated;
            }
        }
    }
}