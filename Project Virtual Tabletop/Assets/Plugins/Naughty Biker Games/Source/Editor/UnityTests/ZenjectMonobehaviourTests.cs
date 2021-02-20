using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace NaughtyBiker.Editor.UnityTests {
	public abstract class ZenjectMonobehaviourTests : ZenjectUnitTestFixture {
		[UnityTearDown]
		public virtual IEnumerator UnityTearDown() {

			GameObject.FindObjectsOfType<GameObject>().ToList()
				.ForEach(gameObj => GameObject.DestroyImmediate(gameObj));

			Container.UnbindAll();

			yield return null;
		}
	}
}
