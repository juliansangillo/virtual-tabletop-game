using System.Collections;
using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;
using NaughtyBikerGames.SDK.Editor.UnityTests;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Vectrosity;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Components {
	public class DrawPathTests : ZenjectMonobehaviourTests {
        VectorLine lineMock;

        Camera lineCamera;
        Color lineColor;
        Texture2D lineTexture;
        Texture2D frontTexture;
        Texture2D backTexture;
        Texture2D pointTexture;

        IPathManager pathManagerMock;
        IVectorLine vectorLineMock;
        SignalBus signalBus;

        GameObject gameObject1;
        GameObject gameObject2;

        GameObject drawPathObject;
        DrawPath drawPath;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            TokenSignalsInstaller.Install(Container);

            lineMock = Substitute.For<VectorLine>("", new List<Vector3>(), 0);

            lineCamera = new GameObject().AddComponent<Camera>();
            lineColor = Color.yellow;
            lineTexture = new Texture2D(32, 32);
            frontTexture = new Texture2D(32, 32);
            backTexture = new Texture2D(32, 32);
            pointTexture = new Texture2D(32, 32);

            pathManagerMock = Substitute.For<IPathManager>();
            vectorLineMock = Substitute.For<IVectorLine>();
            signalBus = Container.Resolve<SignalBus>();

            gameObject1 = new GameObject("Tile 1");
            gameObject1.tag = AppConstants.GRID_SPACE_TAG;
            gameObject1.transform.position = new Vector3(-1, 0, -1);
            GridSpaceMono gridSpaceMono = gameObject1.AddComponent<GridSpaceMono>();
            gridSpaceMono.Row = 0;
            gridSpaceMono.Col = 1;
            gridSpaceMono.Awake();

            gameObject2 = new GameObject("Tile 2");
            gameObject2.tag = AppConstants.GRID_SPACE_TAG;
            gameObject2.transform.position = new Vector3(1, 0, -1);
            gridSpaceMono = gameObject2.AddComponent<GridSpaceMono>();
            gridSpaceMono.Row = 1;
            gridSpaceMono.Col = 1;
            gridSpaceMono.Awake();

            drawPathObject = new GameObject("Token");
            drawPath = drawPathObject.AddComponent<DrawPath>();

            drawPath.LineCamera = lineCamera;
            drawPath.LineColor = lineColor;
            drawPath.LineTexture = lineTexture;
            drawPath.FrontTexture = frontTexture;
            drawPath.BackTexture = backTexture;
            drawPath.PointTexture = pointTexture;

            drawPath.Construct(pathManagerMock, vectorLineMock, signalBus);
        }

        [UnityTest]
        public IEnumerator Start_WhenCalled_SetVectorLineCamera3D() {
            yield return null;

            vectorLineMock.Received(1).SetCamera3D(lineCamera);
        }

        [UnityTest]
        public IEnumerator Start_WhenCalled_SetVectorLineEndCap() {
            yield return null;

            vectorLineMock.Received(1).SetEndCap("Ray", EndCap.Both, -1.0f, lineTexture, frontTexture, backTexture);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_ThenSetLineWithUniqueName_AListOfVector3s_AndAWidth() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.IsNotNull(vectorLineMock.Line);
            Assert.AreEqual("Token_Line", vectorLineMock.Line.name);
            Assert.AreEqual(drawPath.Points, vectorLineMock.Line.points3);
            Assert.AreEqual(AppConstants.PATH_WIDTH_IN_PIXELS, vectorLineMock.Line.lineWidth);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetCurrentSpaceAsOnlyPoint() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual(1, drawPath.Points.Count);
            Assert.Contains(new Vector3(-1, 0, -1), drawPath.Points);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetLineTypeToPoints() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).lineType = LineType.Points;
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetTextureToPointTexture() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).texture = pointTexture;
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetMaterialToPathMaterialInResources() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).material = Resources.Load<Material>("PathMaterial");
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetColorToLineColor() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).SetColor(lineColor);
        }
        
        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_CallDraw3D() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).Draw3D();
        }
	}
}