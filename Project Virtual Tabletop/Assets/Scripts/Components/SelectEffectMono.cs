using UnityEngine;
using NaughtyBikerGames.ProjectVirtualTabletop.Components.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
    public class SelectEffectMono : MonoBehaviour, ISelectEffect {
        [SerializeField] private ParticleSystem selectEffect;

        public ParticleSystem SelectEffect {
            set {
                this.selectEffect = value;
            }
        }

        public void Start() {
            selectEffect.Stop();
        }

        public void Play() {
            selectEffect.Play();
        }

        public void Stop() {
            selectEffect.Stop();
        }
    }
}