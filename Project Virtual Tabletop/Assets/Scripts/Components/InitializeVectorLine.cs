using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using UnityEngine;
using Vectrosity;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
    public class InitializeVectorLine : MonoBehaviour {
        [Header("Camera 3D")]
        [SerializeField] private Camera lineCamera;
        [Header("End Cap")]
        [SerializeField] private string endCapName;
        [SerializeField] private EndCap type = EndCap.None;
        [SerializeField] private float offset = 0;
        [SerializeField] private float scaleFront = 1.0f;
        [SerializeField] private float scaleBack = 1.0f;
        [SerializeField] private Texture2D lineTexture;
        [SerializeField] private Texture2D frontTexture;
        [SerializeField] private Texture2D backTexture;

        private IVectorLine vectorLine;

        public Camera LineCamera {
            set {
                this.lineCamera = value;
            }
        }

        public string EndCapName {
            set {
                this.endCapName = value;
            }
        }

        public EndCap Type {
            set {
                this.type = value;
            }
        }

        public float Offset {
            set {
                this.offset = value;
            }
        }

        public float ScaleFront {
            set {
                this.scaleFront = value;
            }
        }

        public float ScaleBack {
            set {
                this.scaleBack = value;
            }
        }

        public Texture2D LineTexture {
            set {
                this.lineTexture = value;
            }
        }

        public Texture2D FrontTexture {
            set {
                this.frontTexture = value;
            }
        }

        public Texture2D BackTexture {
            set {
                this.backTexture = value;
            }
        }

        [Inject]
        public void Construct(IVectorLine vectorLine) {
            this.vectorLine = vectorLine;
        }

        public void Start() {
            vectorLine.SetCamera3D(lineCamera);
            vectorLine.SetEndCap(endCapName, type, offset, offset, scaleFront, scaleBack, lineTexture, frontTexture, backTexture);
        }
    }
}