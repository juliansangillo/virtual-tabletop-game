using NaughtyBiker.Wrappers.Interfaces;
using UnityEngine.SceneManagement;

namespace NaughtyBiker.Wrappers {
	/**
    * Default implementation of ISceneManager
    *
    * @author Julian Sangillo
    * @version 2.0
    */
	public class SceneManagerWrapper : ISceneManager {
		/// Default implementation of ISceneManager.sceneCountInBuildSettings
		public int sceneCountInBuildSettings { 
            get {
                return SceneManager.sceneCountInBuildSettings;
            }
        }

		/**
        * Default implementation of ISceneManager.GetActiveScene()
        *
        * @return The active scene
        */
		public Scene GetActiveScene() {
			return SceneManager.GetActiveScene();
		}

		/**
        * Default implementation of ISceneManager.GetActiveSceneBuildIndex()
        *
        * @return The build index of the active scene
        */
		public int GetActiveSceneBuildIndex() {
			return SceneManager.GetActiveScene().buildIndex;
		}

		/**
        * Default implementation of ISceneManager.LoadScene(int buildIndex)
        *
        * @param buildIndex The build index to load
        */
		public void LoadScene(int buildIndex) {
			SceneManager.LoadScene(buildIndex);
		}
	}
}