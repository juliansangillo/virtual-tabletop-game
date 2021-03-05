using UnityEngine;

namespace NaughtyBikerGames.SDK.Adapters.Interfaces {
	/**
	* An adapter for Unity's Input. Input is an API that gives back data about a user's input, such as their current mouse position.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public interface IInput {
		/// The current mouse position in screen coordinates as a Vector3
        Vector3 MousePosition { get; }
	}
}