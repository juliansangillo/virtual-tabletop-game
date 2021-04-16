using System;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using UnityEngine;
using Vectrosity;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters {
	public class VectorLineAdapter : IVectorLine, IDisposable {
        private VectorLine line;
        public VectorLine Line {
            get {
                return line;
            } 
            set {
                Dispose();
                line = value;
            }
        }

		public LineType lineType { get => line.lineType; set => line.lineType = value; }
		public Joins joins { get => line.joins; set => line.joins = value; }
		public Texture texture { get => line.texture; set => line.texture = value; }
		public string endCap { get => line.endCap; set => line.endCap = value; }
		public bool continuousTexture { get => line.continuousTexture; set => line.continuousTexture = value; }
		public Material material { get => line.material; set => line.material = value; }

		public void SetCamera3D(Camera camera) {
			VectorLine.SetCamera3D(camera);
		}

		public void SetEndCap(string name, EndCap capType, float offset, params Texture2D[] textures) {
			VectorLine.SetEndCap(name, capType, offset, textures);
		}

        public void SetColor(Color color) {
			line.SetColor(color);
		}

        public void Draw3D() {
			line.Draw3D();
		}

        public void Dispose() {
			VectorLine.Destroy(ref line);
		}
	}
}