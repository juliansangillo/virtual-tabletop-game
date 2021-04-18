using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
	public class CreateAfterimage : MonoBehaviour {
        [SerializeField] private GameObject model;
        [SerializeField] private Material transparent;

        private SignalBus signalBus;

        public GameObject Model {
            set {
                this.model = value;
            }
        }

        public Material Transparent {
            set {
                this.transparent = value;
            }
        }

        public GameObject Clone { get; internal set; }

        [Inject]
        public void Construct(SignalBus signalBus) {
            this.signalBus = signalBus;
        }

        public void Start() {
            signalBus?.Subscribe<TokenSelectedSignal>(OnTokenSelect);
            signalBus?.Subscribe<TokenReleasedSignal>(OnTokenRelease);
        }

        public void OnTokenSelect(TokenSelectedSignal signal) {
			Vector3 originalPosition = GameObjectUtils.FindGameObjectFromGridSpace(signal.CurrentSpace).transform.position;
			originalPosition.y = 0;

			Clone = Instantiate(model, originalPosition, gameObject.transform.rotation);
			Clone.name = $"{gameObject.name}_Clone";

			SetMaterialInAllChildrenOnClone();
		}

        public void OnTokenRelease(TokenReleasedSignal signal) {
            Destroy(Clone);
        }

		public void OnDestroy() {
            signalBus.Unsubscribe<TokenReleasedSignal>(OnTokenRelease);
            signalBus.Unsubscribe<TokenSelectedSignal>(OnTokenSelect);
        }

        private void SetMaterialInAllChildrenOnClone() {
			IEnumerable<MeshRenderer> renderers = Clone.GetComponentsInChildren<MeshRenderer>().AsEnumerable();
			foreach (MeshRenderer renderer in renderers)
				renderer.material = transparent;
		}
    }
}
