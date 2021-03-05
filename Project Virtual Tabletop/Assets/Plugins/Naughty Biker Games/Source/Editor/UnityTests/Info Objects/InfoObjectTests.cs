using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;
using NaughtyBikerGames.SDK.InfoObjects;
using NaughtyBikerGames.SDK.InfoObjects.Components;
using NaughtyBikerGames.SDK.InfoObjects.Installers;
using NaughtyBikerGames.SDK.InfoObjects.Interfaces;
using NaughtyBikerGames.SDK.Signals;
using NaughtyBikerGames.SDK.Signals.Installers;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.InfoObjects {
	public class InfoObjectTests : ZenjectMonobehaviourTests {

		GameObject obj;
		InfoObject infoObject;

		IInfo info;
		SignalBus signalBus;

		[SetUp]
		public void SetUp() {
			InfoObjectBaseInstaller.Install(Container);
			SignalsBaseInstaller.Install(Container);

			info = Container.Resolve<IInfo>() as Info;
			signalBus = Container.Resolve<SignalBus>();

			obj = new GameObject("Object 1");
			infoObject = obj.AddComponent<InfoObject>();
		}

		[UnityTest]
		public IEnumerator InfoObject_CreatingNewGameObjectWithTagPlayer_InjectsIInfoWithIdAsPlayer() {
			obj.tag = "Player";

			infoObject.Construct(info, signalBus);

			yield return null;

			Assert.IsNotNull(infoObject.GetInfo());
			Assert.AreEqual("Player", infoObject.GetInfo().Id);
		}

		[UnityTest]
		public IEnumerator InfoObject_CreatingNewGameObjectWithTagRespawn_InjectsIInfoWithIdAsRespawn() {
			obj.tag = "Respawn";

			infoObject.Construct(info, signalBus);

			yield return null;

			Assert.IsNotNull(infoObject.GetInfo());
			Assert.AreEqual("Respawn", infoObject.GetInfo().Id);
		}

		[UnityTest]
		public IEnumerator InfoObject_SubscribingToStateChangeSignalAndMakingChangeToInfoObject_CallsSubscribingMethodWithValidParams() {
			string actualId = "";
			string actualKey = "";
			string actualValue = "";

			obj.tag = "Player";
			infoObject.Construct(info, signalBus);

			signalBus.Subscribe<StateChangeSignal>((s) => {
				actualId = s.ObjectId;
				actualKey = s.Key;
				actualValue = (string)s.Value;
			});

			info["foo"] = "bar";

			yield return null;

			Assert.AreEqual("Player", actualId);
			Assert.AreEqual("foo", actualKey);
			Assert.AreEqual("bar", actualValue);
		}

		[UnityTest]
		public IEnumerator InfoObject_CallingFireAllWith2KeyValuePairsInTheInfoObject_FiresTwoSignalsOneForEachPair() {
			List<string> actualIds = new List<string>();
			List<string> actualKeys = new List<string>();
			List<string> actualValues = new List<string>();

			obj.tag = "Player";
			infoObject.Construct(info, signalBus);

			info["foo"] = "bar";
			info["foofoo"] = "barbar";

			signalBus.Subscribe<StateChangeSignal>((s) => {
				actualIds.Add(s.ObjectId);
				actualKeys.Add(s.Key);
				actualValues.Add((string)s.Value);
			});

			infoObject.FireAll();

			yield return null;

			Assert.AreEqual(2, actualIds.Count);
			Assert.AreEqual(2, actualIds.FindAll((id) => id == "Player").Count);

			Assert.AreEqual(2, actualKeys.Count);
			Assert.AreEqual(1, actualKeys.FindAll((key) => key == "foo").Count);
			Assert.AreEqual(1, actualKeys.FindAll((key) => key == "foofoo").Count);

			Assert.AreEqual(2, actualValues.Count);
			Assert.AreEqual(1, actualValues.FindAll((val) => val == "bar").Count);
			Assert.AreEqual(1, actualValues.FindAll((val) => val == "barbar").Count);
		}
	}
}
