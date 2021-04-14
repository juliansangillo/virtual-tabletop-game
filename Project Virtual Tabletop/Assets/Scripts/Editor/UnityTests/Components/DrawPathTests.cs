using System.Collections;
using System.Collections.Generic;
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

            drawPath.Construct(pathManagerMock, signalBus);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_GivenLineIsNotAlreadySet_ThenSetLineWithUniqueName_AListOfVector3s_AndAWidth() {
            drawPath.Points.Add(new Vector3());
            
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.IsNotNull(drawPath.Line);
            Assert.AreEqual("Token_Line", drawPath.Line.name);
            Assert.AreEqual(drawPath.Points, drawPath.Line.points3);
            Assert.AreEqual(AppConstants.PATH_WIDTH_IN_PIXELS, drawPath.Line.lineWidth);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_SetCurrentSpaceAsOnlyPoint() {
            drawPath.Line = lineMock;

            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual(1, drawPath.Points.Count);
            Assert.Contains(new Vector3(-1, 0, -1), drawPath.Points);
        }
	}
}