using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;
using NaughtyBikerGames.SDK.Editor.UnityTests;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Components {
	public class CreateAfterimageTests : ZenjectMonobehaviourTests {
        SignalBus signalBus;

        Material transparent;
        MeshRenderer renderer;
        
        GameObject gameObject1;
        GameObject gameObject2;

        CreateAfterimage createAfterimage;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            TokenSignalsInstaller.Install(Container);

            signalBus = Container.Resolve<SignalBus>();

            transparent = new Material(Shader.Find("Unlit/Transparent"));

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

            createAfterimage = new GameObject("Token").AddComponent<CreateAfterimage>();

            renderer = new GameObject("Model").AddComponent<MeshRenderer>();
            renderer.transform.parent = createAfterimage.transform;

            createAfterimage.Model = renderer.gameObject;
            createAfterimage.Transparent = transparent;

            createAfterimage.Construct(signalBus);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_InstantiateACloneAtCorrectVector3() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual(new Vector3(-1, 0, -1), createAfterimage.Clone.transform.position);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_InstantiateACloneWithCorrectName() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual("Token_Clone", createAfterimage.Clone.name);
        }

        [UnityTest]
        public IEnumerator OnTokenSelect_WhenTokenSelectedSignalIsFired_InstantiateACloneWithTransparentMaterialInAllChildMeshRenderers() {
            yield return null;

            signalBus.Fire(new TokenSelectedSignal(new GridSpace(0, 1)));

            yield return null;

            Assert.AreEqual("Unlit/Transparent", createAfterimage.Clone.GetComponentInChildren<MeshRenderer>().material.shader.name);
        }

        [UnityTest]
        public IEnumerator OnTokenRelease_WhenTokenReleasedSignalIsFired_DestroyClone() {
            createAfterimage.Clone = GameObject.Instantiate(renderer.gameObject, new Vector3(0, 0, 0), createAfterimage.transform.rotation);
            
            yield return null;

            signalBus.Fire(new TokenReleasedSignal(new GridSpace(0, 1), new GridSpace(1, 1)));

            yield return null;

            Assert.True(createAfterimage.Clone == null);
        }
	}
}