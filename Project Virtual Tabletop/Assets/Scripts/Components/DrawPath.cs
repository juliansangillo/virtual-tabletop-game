using System.Collections.Generic;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;
using UnityEngine;
using Vectrosity;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
	public class DrawPath : MonoBehaviour {
		[SerializeField] private Color lineColor;
        [SerializeField] private float lineWidthInPixels;
        [SerializeField] private string endCapName;
		[SerializeField] private Texture2D pointTexture;

		private IPathManager pathManager;
		private IVectorLine vectorLine;
		private SignalBus signalBus;

		public List<Vector3> Points { get; private set; } = new List<Vector3>();

		public Color LineColor {
			set {
				this.lineColor = value;
			}
		}

        public float LineWidthInPixels {
            set {
                this.lineWidthInPixels = value;
            }
        }

        public string EndCapName {
            set {
                this.endCapName = value;
            }
        }

		public Texture2D PointTexture {
			set {
				this.pointTexture = value;
			}
		}

		[Inject]
		public void Construct(IPathManager pathManager, IVectorLine vectorLine, SignalBus signalBus) {
			this.pathManager = pathManager;
			this.vectorLine = vectorLine;
			this.signalBus = signalBus;
		}

		public void Start() {
			signalBus.Subscribe<TokenSelectedSignal>(OnTokenSelect);
            signalBus.Subscribe<TokenDraggedSignal>(OnTokenDrag);
            signalBus.Subscribe<TokenReleasedSignal>(OnTokenRelease);
		}

		public void OnTokenSelect(TokenSelectedSignal signal) {
			GridSpace current = signal.CurrentSpace;

            pathManager.Reconnect(current);

			vectorLine.Line = new VectorLine($"{gameObject.name}_Line", Points, lineWidthInPixels);

			Vector3 currentPosition = GameObjectUtils.FindGameObjectFromGridSpace(current).transform.position;
			currentPosition.y = 0;

			Points.Add(currentPosition);

			vectorLine.lineType = LineType.Points;
			vectorLine.texture = pointTexture;

			vectorLine.material = Resources.Load<Material>("PathMaterial");
			vectorLine.SetColor(lineColor);

			vectorLine.Draw3D();
		}

		public void OnTokenDrag(TokenDraggedSignal signal) {
			GridSpace source = signal.Source;
			GridSpace destination = signal.Destination;

			GridPath path = pathManager.Find(source, destination);

            Points = path.Spaces
                .Select(space => {
                    Vector3 position = GameObjectUtils.FindGameObjectFromGridSpace(space).transform.position;
                    position.y = 0;

                    return position;
                })
                .ToList();

            vectorLine.points3 = Points;

            if (Points.Count > 1) {
                vectorLine.lineType = LineType.Continuous;
                vectorLine.joins = Joins.Weld;
                vectorLine.endCap = endCapName;
                vectorLine.continuousTexture = true;
            }
            else {
                vectorLine.lineType = LineType.Points;
                vectorLine.texture = pointTexture;
            }

            vectorLine.SetColor(lineColor);

            vectorLine.Draw3D();
		}

        public void OnTokenRelease(TokenReleasedSignal signal) {
            vectorLine.Dispose();
            Points = new List<Vector3>();
        }

		public void OnDestroy() {
            signalBus.Unsubscribe<TokenReleasedSignal>(OnTokenRelease);
            signalBus.Unsubscribe<TokenDraggedSignal>(OnTokenDrag);
			signalBus.Unsubscribe<TokenSelectedSignal>(OnTokenSelect);
		}
	}
}