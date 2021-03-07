namespace NaughtyBikerGames.SDK.Adapters.Interfaces {
	/**
    * An adapter for Unity's SceneUtility. SceneUtility is a utility class for getting deatils about scenes that are not
    * provided by SceneManager. SceneUtility is a static class and is coupled with Unity's systems. This decouples
    * the SceneUtility from the classes that use it. This way, stubs and mocks of this adapter can be injected for
    * testing purposes.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
	* @since 2.0
    */
	public interface ISceneUtility {
		/**
		* Gets the path to the *.unity file of a scene when given that scene's build index.
		*
		* @param buildIndex The build index of the scene
		*
		* @return The path to the *.unity file
		*/
        string GetScenePathByBuildIndex(int buildIndex);
	}
}