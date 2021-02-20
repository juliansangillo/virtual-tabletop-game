using NaughtyBiker.Wrappers.Interfaces;
using UnityEngine.SceneManagement;

namespace NaughtyBiker.Wrappers {
	/**
    * Default implementation of ISceneUtility
    *
    * @author Julian Sangillo
    * @version 2.0
    */
	public class SceneUtilityWrapper : ISceneUtility {
		/**
		* Default implementation of ISceneUtility.GetScenePathByBuildIndex(int buildIndex)
		*
		* @param buildIndex The build index of the scene
		*
		* @return The path to the *.unity file
		*/
		public string GetScenePathByBuildIndex(int buildIndex) {
			return SceneUtility.GetScenePathByBuildIndex(buildIndex);
		}
	}
}