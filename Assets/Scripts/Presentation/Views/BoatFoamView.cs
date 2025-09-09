using UnityEngine;

namespace SailingBoat.Presentation.Views
{
    public class BoatFoamView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem foamParticles;

        private void Awake()
        {
            if (foamParticles == null)
                foamParticles = GetComponent<ParticleSystem>();

            ActivateEmission(false);
        }

        public void ActivateEmission(bool shouldEmit)
        {
            if (foamParticles == null)
                return;

            var emission = foamParticles.emission;
            emission.enabled = shouldEmit;
        }
    }
}
