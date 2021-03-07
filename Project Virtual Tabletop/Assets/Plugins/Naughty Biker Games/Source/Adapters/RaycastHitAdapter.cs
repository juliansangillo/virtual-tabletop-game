using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Adapters {
    /**
	* Default implementation of IRaycastHit.
	*
	* @author Julian Sangillo \<https://github.com/juliansangillo\>
	* @version 3.0
	* @since 3.0
	*/
	public class RaycastHitAdapter : IRaycastHit {
        private RaycastHit hit;

        /// Default implementation of IRaycastHit.GameObject.
		public GameObject GameObject {
            get {
                return hit.collider.gameObject;
            }
        }

        /**
        * @param hit The RaycastHit that will be wrapped by this adapter
        */
        public RaycastHitAdapter(RaycastHit hit) {
            this.hit = hit;
        }

        /**
		* Default implementation of IRaycastHit.IsAHit().
		*
		* @return True if the Raycast did hit a matching GameObject, or false otherwise
		*/
		public bool IsAHit() {
			return hit.collider != null;
		}
	}
}