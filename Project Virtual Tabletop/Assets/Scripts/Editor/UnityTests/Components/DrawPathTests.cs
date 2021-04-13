using System.Collections;
using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
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
        VectorLine line;

        Camera lineCamera;
        Color lineColor;
        Texture2D lineTexture;
        Texture2D frontTexture;
        Texture2D backTexture;
        Texture2D pointTexture;

        IPathManager pathManager;
        SignalBus signalBus;

        GameObject drawPathObject;
        DrawPath drawPath;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            TokenSignalsInstaller.Install(Container);

            line = Substitute.For<VectorLine>("", new List<Vector3>(), 0);

            lineCamera = new GameObject().AddComponent<Camera>();
            lineColor = Color.yellow;
            lineTexture = new Texture2D(32, 32);
            frontTexture = new Texture2D(32, 32);
            backTexture = new Texture2D(32, 32);
            pointTexture = new Texture2D(32, 32);

            pathManager = Substitute.For<IPathManager>();
            signalBus = Container.Resolve<SignalBus>();

            drawPathObject = new GameObject("Token");
            drawPath = drawPathObject.AddComponent<DrawPath>();

            drawPath.LineCamera = lineCamera;
            drawPath.LineColor = lineColor;
            drawPath.LineTexture = lineTexture;
            drawPath.FrontTexture = frontTexture;
            drawPath.BackTexture = backTexture;
            drawPath.PointTexture = pointTexture;

            drawPath.Construct(pathManager, signalBus);
        }

        [UnityTest]
        public IEnumerator Start_GivenLineIsNotSet_ThenSetLineWithUniqueName_EmptyListOfVector3s_AndAWidth() {
            yield return null;

            Assert.IsNotNull(drawPath.Line);
            Assert.AreEqual("Token Line", drawPath.Line.name);
            Assert.AreEqual(0, drawPath.Line.points3.Count);
            Assert.AreEqual(AppConstants.PATH_WIDTH_IN_PIXELS, drawPath.Line.lineWidth);
        }

        /*[UnityTest]
        public IEnumerator OnTokenSelect_WhenTriggered_DrawDot() {
            VectorLine oldLine = drawPath.Line;
            drawPath.Line = line;
            VectorLine.Destroy(ref oldLine);

            yield return null;
        }*/
	}
}