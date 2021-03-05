using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.Adapters.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;

namespace NaughtyBikerGames.SDK.Raycast {
	/**
	* Default implementation of IRaycastable.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*
	* @see RaycasterInstaller
	*/
	public class Raycaster : IRaycastable {
		private readonly IInput input;
		private readonly IMainCamera mainCamera;
		private readonly IPhysics physics;

		/**
		* @param input The adapter for Unity's Input API
		* @param mainCamera The adapter for Unity's main Camera API
		* @param physics The adapter for Unity's Physics API
		*/
		[Inject]
		public Raycaster(IInput input, IMainCamera mainCamera, IPhysics physics) {
			this.input = input;
			this.mainCamera = mainCamera;
			this.physics = physics;
		}

		/**
		* Default implementation of IRaycastable.CastRayForTag(string tag).
		*
		* @param tag The existing tag to target
		*
		* @return The first Game Object hit by the ray that matches the tag, or null if no matching Game Object was hit
		*/
		public GameObject CastRayForTag(string tag) {
			Ray ray = mainCamera.ScreenPointToRay(input.MousePosition);
			IRaycastHit hit = physics.RaycastAllFirstOrDefault(ray, h => h.collider.tag == tag);

			GameObject result = null;

			if(hit.IsAHit())
				result = hit.GameObject;

			return result;
		}
	}
}