using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces {
	public interface IVectorLine : IDisposable {
        VectorLine Line { get; set; }
        List<Vector3> points3 { get; set; }
        LineType lineType { get; set; }
        Joins joins { get; set; }
        Texture texture { get; set; }
        string endCap { get; set; }
        bool continuousTexture { get; set; }
        Material material { get; set; }

        void SetCamera3D(Camera camera);
        void SetEndCap(string name, EndCap capType, float offsetFront, float offsetBack, float scaleFront, float scaleBack, params Texture2D[] textures);

        void SetColor(Color color);
        void Draw3D();
	}
}