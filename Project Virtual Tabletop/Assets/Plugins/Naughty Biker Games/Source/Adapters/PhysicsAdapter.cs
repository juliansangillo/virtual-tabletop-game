using System;
using System.Linq;
using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Adapters {
	/**
    * Default implementation of IPhysics
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
    */
	public class PhysicsAdapter : IPhysics {
		/**
		* Default implementation of IPhysics.RaycastAllFirstOrDefault(Ray ray)
		*
		* @param ray The Ray to cast
		*
		* @return An IRaycastHit that has details on the object that was hit if any
		*/
		public IRaycastHit RaycastAllFirstOrDefault(Ray ray) {
			return new RaycastHitAdapter(Physics.RaycastAll(ray).FirstOrDefault());
		}

		/**
		* Default implementation of IPhysics.RaycastAllFirstOrDefault(Ray ray, Func<RaycastHit, bool> predicate)
		*
		* @param ray The Ray to cast
		* @param predicate A lambda expression that returns true on "matching" hits, or false otherwise
		*
		* @return An IRaycastHit that has details on the object that was hit if any
		*/
		public IRaycastHit RaycastAllFirstOrDefault(Ray ray, Func<RaycastHit, bool> predicate) {
			return new RaycastHitAdapter(Physics.RaycastAll(ray).FirstOrDefault(predicate));
		}
	}
}