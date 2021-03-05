using System;
using System.Collections;
using UnityEngine;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;

namespace NaughtyBikerGames.SDK.Interpolation {
	/**
    * Default implementation of ILerpable.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    */
	public class Lerper : ILerpable {
		/**
        * Default implementation of ILerpable.Lerp(Vector3 source, Vector3 destination, float duration, Action<Vector3> update).
        *
        * @param source The starting value of the Lerp as a Vector3
        * @param destination The end value of the Lerp as a Vector3
        * @param duration The duration of the Lerp in seconds
        * @param update A callback that gets called each frame the Lerp iterates
        *
        * @return An IEnumerator that is used to start a coroutine
        */
		public IEnumerator Lerp(Vector3 source, Vector3 destination, float duration, Action<Vector3> update) {
			float timeElapsed = 0f;
			while(timeElapsed < duration) {
				update(Vector3.Lerp(source, destination, timeElapsed / duration));
				timeElapsed += Time.deltaTime;

				yield return null;
			}

			update(destination);
		}
	}
}