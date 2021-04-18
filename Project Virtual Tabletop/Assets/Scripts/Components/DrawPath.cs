using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Vectrosity;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Enums;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
	public class DrawPath : MonoBehaviour {
		[Header("VectorLine")]
		[SerializeField] private Color lineColor;
		[SerializeField] private float lineWidthInPixels;
		[SerializeField] private string endCapName;
		[SerializeField] private Texture2D pointTexture;
		[Header("UI")]
		[SerializeField] private Canvas canvas;
		[SerializeField] private Text lengthText;
		[SerializeField] private int defaultLength;
		[SerializeField] private Text distanceText;
		[SerializeField] private Distance unitOfDistance = Distance.METERS;
		[SerializeField] private float defaultDistance;
        [SerializeField] private int decimalPrecision;

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

		public Canvas Canvas {
			set {
				this.canvas = value;
			}
		}

		public Text LengthText {
			set {
				this.lengthText = value;
			}
		}

		public int DefaultLength {
			set {
				this.defaultLength = value;
			}
		}

		public Text DistanceText {
			set {
				this.distanceText = value;
			}
		}

		public Distance UnitOfDistance {
			set {
				this.unitOfDistance = value;
			}
		}

		public float DefaultDistance {
			set {
				this.defaultDistance = value;
			}
		}

        public int DecimalPrecision {
            set {
                this.decimalPrecision = value;
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

			InitializeVectorLine(current);
			InitializeUI();
		}

		public void OnTokenDrag(TokenDraggedSignal signal) {
			GridSpace source = signal.Source;
			GridSpace destination = signal.Destination;

			GridPath path = pathManager.Find(source, destination);

			UpdateVectorLine(path);
			UpdateUI(path);
		}

		public void OnTokenRelease(TokenReleasedSignal signal) {
			vectorLine.Dispose();
			Points = new List<Vector3>();
            canvas.gameObject.SetActive(false);
		}

		public void OnDestroy() {
			signalBus.Unsubscribe<TokenReleasedSignal>(OnTokenRelease);
			signalBus.Unsubscribe<TokenDraggedSignal>(OnTokenDrag);
			signalBus.Unsubscribe<TokenSelectedSignal>(OnTokenSelect);
		}
        
        private void InitializeVectorLine(GridSpace current) {
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

        private void InitializeUI() {
			canvas.gameObject.SetActive(true);

			lengthText.text = $"{defaultLength}";
            distanceText.text = ToText(defaultDistance);
		}

        private void UpdateVectorLine(GridPath path) {
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
			} else {
				vectorLine.lineType = LineType.Points;
				vectorLine.texture = pointTexture;
			}

			vectorLine.SetColor(lineColor);

			vectorLine.Draw3D();
		}

        private void UpdateUI(GridPath path) {
			lengthText.text = $"{path.Length}";
			distanceText.text = ToText(path.DistanceInMeters);
		}

		private string ToText(float distanceInMeters) {
            string result = "";
            float distance = (float) Math.Round(ConvertDistance.From(Distance.METERS, distanceInMeters).To(unitOfDistance), decimalPrecision);

			switch (unitOfDistance) {
				case Distance.METERS:
					result = $"{distance}m";
					break;
				case Distance.FEET:
					result = $"{distance}ft";
					break;
			}

            return result;
		}
	}
}