using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Zenject;
using Vectrosity;
using NaughtyBikerGames.SDK.Editor.UnityTests;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Enums;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Components {
	public class DrawPathTests : ZenjectMonobehaviourTests {
        Color lineColor;
        float lineWidthInPixels;
        string endCapName;
        Texture2D pointTexture;
        Canvas canvas;
        Text lengthText;
        Text distanceText;

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

            lineColor = Color.yellow;
            lineWidthInPixels = 5.0f;
            endCapName = "Ray";
            pointTexture = new Texture2D(32, 32);
            canvas = new GameObject().AddComponent<Canvas>();
            lengthText = new GameObject().AddComponent<Text>();
            distanceText = new GameObject().AddComponent<Text>();

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

            drawPath.LineColor = lineColor;
            drawPath.LineWidthInPixels = lineWidthInPixels;
            drawPath.EndCapName = endCapName;
            drawPath.PointTexture = pointTexture;
            drawPath.Canvas = canvas;
            drawPath.LengthText = lengthText;
            drawPath.DefaultLength = 0;
            drawPath.DistanceText = distanceText;
            drawPath.DefaultDistance = 0;
            drawPath.DecimalPrecision = 3;

            drawPath.Construct(pathManagerMock, vectorLineMock, signalBus);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_ThenReconnectCurrentSpaceToPathToEnablePathFinding() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            pathManagerMock.Received(1).Reconnect(new GridSpace(0, 1));
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_ThenSetLineWithUniqueName_AListOfVector3s_AndAWidth() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.IsNotNull(vectorLineMock.Line);
            Assert.AreEqual("Token_Line", vectorLineMock.Line.name);
            Assert.AreEqual(drawPath.Points, vectorLineMock.Line.points3);
            Assert.AreEqual(lineWidthInPixels, vectorLineMock.Line.lineWidth);
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

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_ActivateUICanvasObject() {
            canvas.gameObject.SetActive(false);

            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.True(canvas.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetLengthUITextToDefault() {
            lengthText.text = "1";

            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual("0", lengthText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFiredAndDistanceIsInMeters_SetDistanceUITextToDefaultInMeters() {
            drawPath.UnitOfDistance = Distance.METERS;
            distanceText.text = "1m";

            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual("0m", distanceText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFiredAndDistanceIsInFeet_SetDistanceUITextToDefaultInFeet() {
            drawPath.UnitOfDistance = Distance.FEET;
            distanceText.text = "1ft";

            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual("0ft", distanceText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_CallPathManagerFindOnSourceAndDestination() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            pathManagerMock.Received(1).Find(new GridSpace(0, 1), new GridSpace(1, 1));
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_SetPointsToTheListOfVector3PositionsOfEachGridSpaceInPath() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.AreEqual(2, drawPath.Points.Count);
            Assert.Contains(new Vector3(-1, 0, -1), drawPath.Points);
            Assert.Contains(new Vector3(1, 0, -1), drawPath.Points);
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_SetVectorLinePoints3ToTheNewPoints() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).points3 = drawPath.Points;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenMoreThanOnePointOnPath_SetLineTypeToContinuous() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).lineType = LineType.Continuous;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenMoreThanOnePointOnPath_SetJoinsToWeld() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).joins = Joins.Weld;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenMoreThanOnePointOnPath_SetEndCap() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).endCap = endCapName;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenMoreThanOnePointOnPath_SetContinuousTextureToTrue() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).continuousTexture = true;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenOnlyOnePointOnPath_SetLineTypeToPoints() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(0, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).lineType = LineType.Points;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_GivenOnlyOnePointOnPath_SetTextureToPointTexture() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(0, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(0, 1)));

            yield return null;

            vectorLineMock.Received(1).texture = pointTexture;
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_SetColorToLineColor() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).SetColor(lineColor);
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_CallDraw3D() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).Draw3D();
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFired_SetLengthUITextToGridPathLength() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.AreEqual("1", lengthText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFiredAndUnitOfDistanceIsMeters_SetDistanceUITextToGridPathDistanceInMeters() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);
            drawPath.UnitOfDistance = Distance.METERS;

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.AreEqual("1.524m", distanceText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenDrag_WhenTokenDraggedSignalIsFiredAndUnitOfDistanceIsFeet_SetDistanceUITextToGridPathDistanceInFeet() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(0, 1));
            spaces.Add(new GridSpace(1, 1));
            GridPath fakePath = new GridPath(1, AppConstants.SPACE_WIDTH_IN_METERS, spaces);
            drawPath.UnitOfDistance = Distance.FEET;

            pathManagerMock.Find(new GridSpace(0, 1), new GridSpace(1, 1)).Returns(fakePath);

            yield return null;

            signalBus.Fire(new TokenDraggedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.AreEqual($"5ft", distanceText.text);
        }

        [UnityTest]
        public IEnumerator OnTokenRelease_WhenTokenReleasedSignalIsFired_CallVectorLineDispose() {
            yield return null;

            signalBus.Fire(new TokenReleasedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            vectorLineMock.Received(1).Dispose();
        }

        [UnityTest]
        public IEnumerator OnTokenRelease_WhenTokenReleasedSignalIsFired_ClearPoints() {
            drawPath.Points.Add(new Vector3(-1, 0, -1));
            drawPath.Points.Add(new Vector3(1, 0, -1));
            
            yield return null;

            signalBus.Fire(new TokenReleasedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.AreEqual(0, drawPath.Points.Count);
        }

        [UnityTest]
        public IEnumerator OnTokenRelease_WhenTokenReleasedSignalIsFired_DeactivateUICanvasObject() {
            canvas.gameObject.SetActive(true);

            yield return null;

            signalBus.Fire(new TokenReleasedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.False(canvas.gameObject.activeSelf);
        }
	}
}