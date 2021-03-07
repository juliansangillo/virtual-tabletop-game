using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;
using NaughtyBikerGames.SDK.Editor.UnityTests.Stubs;
using NaughtyBikerGames.SDK.InfoObjects.Components;
using NaughtyBikerGames.SDK.InfoObjects.Delegates;
using NaughtyBikerGames.SDK.Signals;
using NaughtyBikerGames.SDK.Signals.Installers;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.InfoObjects {
	public class InfoObjectControlTests : ZenjectMonobehaviourTests {
		SignalBus signalBus;

		[SetUp]
		public void SetUp() {
			SignalsBaseInstaller.Install(Container);
			signalBus = Container.Resolve<SignalBus>();
		}

		[UnityTest]
		public IEnumerator InfoObjectControl_InfoObjectWithTagDoesntExist_CallCreateInfoObjectOnStart() {
			bool isCreated = false;
			
			CreateInfoObject createInfoObject = (prefab, tag) => {
				GameObject obj = new GameObject();
				obj.tag = tag;
				InfoObjectStub infoObj = obj.AddComponent<InfoObjectStub>();
				isCreated = true;
				return infoObj;
			};

			InfoObjectControl infoObjectControl = new GameObject().AddComponent<InfoObjectControl>();
			infoObjectControl.Construct(createInfoObject, signalBus);
			infoObjectControl.ObjectTag = "Player";

			yield return null;

			Assert.True(isCreated);
		}

		[UnityTest]
		public IEnumerator InfoObjectControl_InfoObjectWithTagDoesExist_GetExistingInfoObjectOnStart() {
			bool isCreated = false;

			GameObject obj1 = new GameObject();
			obj1.tag = "Player";
			InfoObjectStub infoObj1 = obj1.AddComponent<InfoObjectStub>();
			infoObj1.Id = 1;
			
			CreateInfoObject createInfoObject = (prefab, tag) => {
				GameObject obj = new GameObject();
				obj.tag = tag;
				InfoObjectStub infoObj = obj.AddComponent<InfoObjectStub>();
				isCreated = true;
				return infoObj;
			};

			InfoObjectControl infoObjectControl = new GameObject().AddComponent<InfoObjectControl>();
			infoObjectControl.Construct(createInfoObject, signalBus);
			infoObjectControl.ObjectTag = "Player";

			yield return null;

			Assert.False(isCreated);
			Assert.AreEqual(infoObj1.Id, ((InfoObjectStub)infoObjectControl.InfoObj).Id);
		}

		[UnityTest]
		public IEnumerator InfoObjectControl_FiringInitializeSignal_CallsFireAllOnInfoObject() {
			InfoObjectStub infoObj = null;

			CreateInfoObject createInfoObject = (prefab, tag) => {
				GameObject obj = new GameObject();
				obj.tag = tag;
				infoObj = obj.AddComponent<InfoObjectStub>();
				return infoObj;
			};

			InfoObjectControl infoObjectControl = new GameObject().AddComponent<InfoObjectControl>();
			infoObjectControl.Construct(createInfoObject, signalBus);
			infoObjectControl.ObjectTag = "Player";

			yield return null;

			signalBus.Fire(new InitializeSignal());

			yield return null;

			Assert.AreEqual(1, infoObj.CallCount);
		}
	}
}
