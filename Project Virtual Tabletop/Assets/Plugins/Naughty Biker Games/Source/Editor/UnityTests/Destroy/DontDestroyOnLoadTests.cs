using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.SDK.Destroy;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.Destroy {
	public class DontDestroyOnLoadTests : MonobehaviourTests {
        private const string tag = "Player";

		[UnityTest]
		public IEnumerator DontDestroy_PreserveDuplicatesIsFalseAndOnly1GameObjectWithTagExists_MakeObjectDontDestroyOnLoad() {
			GameObject obj = new GameObject("foo");
			obj.tag = tag;
			DontDestroyOnLoad dontDestroyOnLoad = obj.AddComponent<DontDestroyOnLoad>();
			dontDestroyOnLoad.PreserveDuplicates = false;

			dontDestroyOnLoad.DontDestroy();

			yield return null;

			Assert.True(IsDontDestroyOnLoad(obj));
		}

		[UnityTest]
		public IEnumerator DontDestroy_PreserveDuplicatesIsFalseAnd2GameObjectsWithTagExist_MakeOneObjectDontDestroyOnLoadAndDestroyOtherObject() {
			GameObject obj1 = new GameObject("foo");
			obj1.tag = tag;
            DontDestroyOnLoad dontDestroyOnLoad1 = obj1.AddComponent<DontDestroyOnLoad>();
			dontDestroyOnLoad1.PreserveDuplicates = false;
            dontDestroyOnLoad1.DontDestroy();

            yield return null;

			GameObject obj2 = new GameObject("bar");
			obj2.tag = tag;
			DontDestroyOnLoad dontDestroyOnLoad2 = obj2.AddComponent<DontDestroyOnLoad>();
			dontDestroyOnLoad2.PreserveDuplicates = false;
			dontDestroyOnLoad2.DontDestroy();

			yield return null;

            List<GameObject> objList = new List<GameObject>();
            objList.Add(obj1);
			objList.Add(obj2);

			Assert.AreEqual(1, objList.FindAll(obj => IsDontDestroyOnLoad(obj)).Count);
			Assert.AreEqual(1, objList.FindAll(obj => IsDestroyed(obj)).Count);
		}

		private bool IsDontDestroyOnLoad(GameObject obj) {
			bool result;

			if (obj != null)
				result = obj.scene.buildIndex == -1;
			else
				result = false;

			return result;
		}

		private bool IsDestroyed(GameObject obj) {
			return obj == null;
		}
	}
}
