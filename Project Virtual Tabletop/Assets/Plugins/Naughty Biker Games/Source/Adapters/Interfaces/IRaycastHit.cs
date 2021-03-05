using UnityEngine;

namespace NaughtyBikerGames.SDK.Adapters.Interfaces {
	/**
	* An adapter for Unity's RaycastHit. A RaycastHit, or more commonly known as a 'hit', is a struct that provides details on a 
	* collider that was hit by a Raycast. For the sake of convenience, this adapter obtains the GameObject the collider was attached
	* to, since that is more valuable data in most circumstances.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public interface IRaycastHit {
		/// The GameObject that was hit by the Raycast
		GameObject GameObject { get; }

		/**
		* A validator that checks if the Raycast really was a hit or not. This is used for convenience instead of relying on a null
		* check.
		*
		* @return True if the Raycast did hit a matching GameObject, or false otherwise
		*/
		bool IsAHit();
	}
}