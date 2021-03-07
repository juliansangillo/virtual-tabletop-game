using UnityEngine;

namespace NaughtyBikerGames.SDK.Raycast.Interfaces {
	/**
	* Casts rays for different targets and handles the results.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public interface IRaycastable {
		/**
		* Casts a ray for the given target from the current mouse position. The target is the first Game Object hit by the ray
		* that matches the given tag.
		*
		* @param tag The existing tag to target
		*
		* @return The first Game Object hit by the ray that matches the tag, or null if no matching Game Object was hit
		*/
        GameObject CastRayForTag(string tag);
	}
}