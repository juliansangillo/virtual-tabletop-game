using System.Collections.Generic;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;
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
		private IVectorLine vectorLine;
		private SignalBus signalBus;

		public List<Vector3> Points { get; private set; } = new List<Vector3>();

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
		public void Construct(IPathManager pathManager, IVectorLine vectorLine, SignalBus signalBus) {
			this.pathManager = pathManager;
			this.vectorLine = vectorLine;
			this.signalBus = signalBus;
		}

		public void Start() {
			vectorLine.SetCamera3D(lineCamera);
			vectorLine.SetEndCap("Ray", EndCap.Both, -1.0f, lineTexture, frontTexture, backTexture);

			signalBus.Subscribe<TokenSelectedSignal>(OnTokenSelect);
            signalBus.Subscribe<TokenDraggedSignal>(OnTokenDrag);
		}

		public void OnTokenSelect(TokenSelectedSignal signal) {
			GridSpace current = signal.CurrentSpace;

			vectorLine.Line = new VectorLine($"{gameObject.name}_Line", Points, AppConstants.PATH_WIDTH_IN_PIXELS);

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

			/* int count = Points.Count >= path.Count ? Points.Count : path.Count;
            for(int i = 0; i < count; i++) {
                if(i >= path.Count) {
                    Points.RemoveRange(i, Points.Count - i);
                    break;
                }

                if(i >= Points.Count) {
                    //Points.AddRange()
                }
                
                Vector3 position = GameObjectUtils.FindGameObjectFromGridSpace(path.Spaces[i]).transform.position;
                position.y = 0;

                if(i >= Points.Count)
                    Points.Add(position);
            } */
		}

		public void OnDestroy() {
            signalBus.Unsubscribe<TokenDraggedSignal>(OnTokenDrag);
			signalBus.Unsubscribe<TokenSelectedSignal>(OnTokenSelect);
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