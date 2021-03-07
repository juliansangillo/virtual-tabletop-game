using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.SDK.Destroy;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.Destroy {
	public class DestroyAfterSecondsTests : MonobehaviourTests {
		[UnityTest]
		public IEnumerator DestroyAfterSeconds_DestroyAfterPoint2SecondsAndOnlyPoint1SecondPasses_GameObjectShouldNotBeDestroyed() {
			float secondsToWait = 0.1f;
            
            GameObject obj = new GameObject();
            DestroyAfterSeconds destroyAfterSeconds = obj.AddComponent<DestroyAfterSeconds>();

            destroyAfterSeconds.Seconds = 0.2f;

			yield return new WaitForSeconds(secondsToWait);

            Assert.False(IsDestroyed(obj));
		}

        [UnityTest]
		public IEnumerator DestroyAfterSeconds_DestroyAfterPoint2SecondsAndPoint3SecondsPass_GameObjectShouldBeDestroyed() {
            float secondsToWait = 0.3f;

			GameObject obj = new GameObject();
            DestroyAfterSeconds destroyAfterSeconds = obj.AddComponent<DestroyAfterSeconds>();

            destroyAfterSeconds.Seconds = 0.2f;

			yield return new WaitForSeconds(secondsToWait);

            Assert.True(IsDestroyed(obj));
		}

        private bool IsDestroyed(GameObject obj) {
            return obj == null;
        }
	}
}
