using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace NaughtyBikerGames.SDK.Editor.UnityTests {
	public abstract class MonobehaviourTests {
		[UnityTearDown]
        public virtual IEnumerator UnityTearDown() {
            GameObject.FindObjectsOfType<GameObject>().ToList()
                .ForEach(gameObj => GameObject.DestroyImmediate(gameObj));

            yield return null;
        }
	}
}
