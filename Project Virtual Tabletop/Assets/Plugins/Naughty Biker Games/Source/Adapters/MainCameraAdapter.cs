using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Adapters {
	/**
	* Default implementation of IMainCamera
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public class MainCameraAdapter : IMainCamera {
		/**
		* Default implementation of IMainCamera.ScreenPointToRay(Vector3 pos)
		*
		* @param pos A Vector3 representing a point on the camera screen
		*
		* @return The resulting Ray
		*/
		public Ray ScreenPointToRay(Vector3 pos) {
			return Camera.main.ScreenPointToRay(pos);
		}
	}
}