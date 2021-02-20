using NaughtyBiker.InfoObjects.Interfaces;
using UnityEngine;

namespace NaughtyBiker.InfoObjects.Delegates {
	/**
    * A delegate function to call when requesting Zenject to provide us with a new info object.
    *
    * @param infoObjectPrefab The reference to the info object prefab Zenject should use to instantiate the info object
    * @param objectTag The tag to set on the new info object used to identify that object.
    */
    public delegate IInfoObject CreateInfoObject(GameObject infoObjectPrefab, string objectTag);
}