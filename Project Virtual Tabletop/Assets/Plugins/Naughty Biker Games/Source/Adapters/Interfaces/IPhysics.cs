using System;
using UnityEngine;

namespace NaughtyBikerGames.SDK.Adapters.Interfaces {
	/**
	* An adapter for Unity's Physics. Physics is an API that allows users to perform physics operations with
	* Unity's physics engine.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public interface IPhysics {
		/**
		* Casts a given Ray, identifies all the objects that were intersected by the Raycast, and
		* returns the first one as a RaycastHit.
		*
		* @param ray The Ray to cast
		*
		* @return An IRaycastHit that has details on the object that was hit if any
		*/
		IRaycastHit RaycastAllFirstOrDefault(Ray ray);

		/**
		* Casts a given Ray, identifies all the objects that were intersected by the Raycast, and
		* returns the first one as a RaycastHit that matches the given predicate.
		*
		* @param ray The Ray to cast
		* @param predicate A lambda expression that returns true on "matching" hits, or false otherwise
		*
		* @return An IRaycastHit that has details on the object that was hit if any
		*/
		IRaycastHit RaycastAllFirstOrDefault(Ray ray, Func<RaycastHit, bool> predicate);
	}
}