using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Vectrosity;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Components {
	public class InitializeVectorLineTests {
        Camera lineCamera;
        string endCapName;
        EndCap type;
        float offset;
        float scaleFront;
        float scaleBack;
        Texture2D lineTexture;
        Texture2D frontTexture;
        Texture2D backTexture;
        IVectorLine vectorLineMock;

        InitializeVectorLine initialize;

        [SetUp]
        public void SetUp() {
            lineCamera = new GameObject().AddComponent<Camera>();
            endCapName = "Ray";
            type = EndCap.Both;
            offset = -1.0f;
            scaleFront = 1.0f;
            scaleBack = 1.0f;
            lineTexture = new Texture2D(32, 32);
            frontTexture = new Texture2D(32, 32);
            backTexture = new Texture2D(32, 32);

            vectorLineMock = Substitute.For<IVectorLine>();

            initialize = new GameObject().AddComponent<InitializeVectorLine>();

            initialize.LineCamera = lineCamera;
            initialize.EndCapName = endCapName;
            initialize.Type = type;
            initialize.Offset = offset;
            initialize.ScaleFront = scaleFront;
            initialize.ScaleBack = scaleBack;
            initialize.LineTexture = lineTexture;
            initialize.FrontTexture = frontTexture;
            initialize.BackTexture = backTexture;

            initialize.Construct(vectorLineMock);
        }

        [UnityTest]
        public IEnumerator Start_WhenCalled_SetVectorLineCamera3D() {
            yield return null;

            vectorLineMock.Received(1).SetCamera3D(lineCamera);
        }

        [UnityTest]
        public IEnumerator Start_WhenCalled_SetVectorLineEndCap() {
            yield return null;

            vectorLineMock.Received(1).SetEndCap(endCapName, type, offset, offset, scaleFront, scaleBack, lineTexture, frontTexture, backTexture);
        }
	}
}