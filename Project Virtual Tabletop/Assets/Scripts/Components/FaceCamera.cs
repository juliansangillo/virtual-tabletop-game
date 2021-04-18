using UnityEngine;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
    public class FaceCamera : MonoBehaviour {
        [SerializeField] private Camera targetCamera;

        public Camera TargetCamera {
            set {
                this.targetCamera = value;
            }
        }

        public void Update() {
            transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward, targetCamera.transform.rotation * Vector3.up);
        }
    }
}