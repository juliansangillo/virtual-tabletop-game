using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.SDK.Interpolation;
using NaughtyBikerGames.SDK.Interpolation.Installers;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.Interpolation {
	public class LerperTests : ZenjectMonobehaviourTests {
        Lerper lerper;

        [SetUp]
        public void SetUp() {
            LerperBaseInstaller.Install(Container);
            lerper = Container.Resolve<ILerpable>() as Lerper;
        }

		[UnityTest]
		public IEnumerator Lerper_Vector3_GivenSourceDestinationAndDuration_LerpOverTheValuesAndReturnTheCurrentValueEachFrameUntilTheDurationIsReached() {
			Vector3 source = new Vector3(0, 0, 0);
            Vector3 destination = new Vector3(2, 2, 2);
            float duration = 0.2f;
            
            Vector3 actual = source;
            new GameObject().AddComponent<MonoBehaviourStub>().StartCoroutine(lerper.Lerp(source, destination, duration, current => actual = current));

			yield return new WaitForSeconds(0.1f);
            
            Assert.AreEqual(1, Mathf.Round(actual.x));
            Assert.AreEqual(1, Mathf.Round(actual.y));
            Assert.AreEqual(1, Mathf.Round(actual.z));

            yield return new WaitForSeconds(0.11f);

            Assert.AreEqual(destination.x, actual.x);
            Assert.AreEqual(destination.y, actual.y);
            Assert.AreEqual(destination.z, actual.z);
		}
	}
}
