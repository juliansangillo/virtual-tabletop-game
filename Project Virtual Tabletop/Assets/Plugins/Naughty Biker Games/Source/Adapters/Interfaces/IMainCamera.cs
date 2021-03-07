using UnityEngine;

namespace NaughtyBikerGames.SDK.Adapters.Interfaces {
	/**
	* An adapter for Unity's Camera.main. Camera is an API that gives back data about the cameras in the scenes and 
	* Camera.main specifically targets the main camera.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public interface IMainCamera {
		/**
		* Converts a screen point given as a Vector3 to a Ray. A Ray is an imaginary line that can pierce objects and
		* return data about them when it is cast. This just returns a Ray to use. It does not cast the Ray.
		*
		* @param pos A Vector3 representing a point on the camera screen
		*
		* @return The resulting Ray
		*/
        Ray ScreenPointToRay(Vector3 pos);
	}
}