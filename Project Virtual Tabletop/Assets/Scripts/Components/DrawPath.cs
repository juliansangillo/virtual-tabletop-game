using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using UnityEngine;
using Vectrosity;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
    public class DrawPath : MonoBehaviour {
        [SerializeField] private Camera lineCamera;
        [SerializeField] private Color lineColor;
        [SerializeField] private Texture2D lineTexture;
        [SerializeField] private Texture2D frontTexture;
        [SerializeField] private Texture2D backTexture;
        [SerializeField] private Texture2D pointTexture;

        private IPathManager pathManager;
        private SignalBus signalBus;

        private readonly List<Vector3> points = new List<Vector3>();

        public VectorLine Line { get; internal set; }

        public Camera LineCamera {
            set {
                this.lineCamera = value;
            }
        }

        public Color LineColor {
            set {
                this.lineColor = value;
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

        public Texture2D PointTexture {
            set {
                this.pointTexture = value;
            }
        }

        [Inject]
        public void Construct(IPathManager pathManager, SignalBus signalBus) {
            this.pathManager = pathManager;
            this.signalBus = signalBus;
        }

        public void Start() {
            VectorLine.SetCamera3D(lineCamera);
            VectorLine.SetEndCap("Ray", EndCap.Both, -1.0f, lineTexture, frontTexture, backTexture);

            Line = new VectorLine($"{gameObject.name} Line", new List<Vector3>(), AppConstants.PATH_WIDTH_IN_PIXELS);
        }

        public void OnDestroy() {
            VectorLine line = Line;
            Line = null;

            VectorLine.Destroy(ref line);
        }

        /* public void Start() {
            VectorLine.SetCamera3D(lineCamera);
            VectorLine.SetEndCap("Ray", EndCap.Both, -1.0f, lineTexture, frontTexture, backTexture);

            List<Vector3> points = new List<Vector3>() {
                new Vector3(-4.5f, 0, -3.5f),
                new Vector3(-4.5f, 0, -2.5f),
                new Vector3(-4.5f, 0, -1.5f),
                new Vector3(-3.5f, 0, -0.5f)
            };

            List<Vector3> points = new List<Vector3>() {
                new Vector3(-4.5f, 0, -3.5f),
                new Vector3(-3.5f, 0, -3.5f)
            };

            List<Vector3> points = new List<Vector3>() {
                new Vector3(-4.5f, 0, -3.5f)
            };

            List<Vector3> points = new List<Vector3>();

            line = new VectorLine("Line", points, 5.0f);
            
            if(points.Count > 1) {
                line.lineType = LineType.Continuous;
                line.joins = Joins.Weld;
                line.endCap = "Ray";
                line.continuousTexture = true;
            }
            else {
                line.lineType = LineType.Points;
                line.joins = Joins.None;
                line.texture = pointTexture;
            }

            line.material = Resources.Load<Material>("PathMaterial");
            line.SetColor(lineColor);
            
            line.Draw3D();
        } */
    }
}