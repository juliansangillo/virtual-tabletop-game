namespace NaughtyBikerGames.SDK.InfoObjects.Interfaces {
    /**
    * An info object for other game objects and is a wrapper for the IInfo objects. If this object 
    * is to persist between scenes, it must have DontDestroyOnLoad attached as well.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    *
    * @see InfoObjectControl
    */
	public interface IInfoObject {
        /**
        * Fires a StateChangeSignal of all stored keys and their values in this info object. So, if there are 5 key-value pairs,
        * the StateChangeSignal will be fired 5 times. This is used to initialize everything in the game that might need that data
        * at the start of each scene when objects and scripts are loaded for the first time.
        */
        void FireAll();
	}
}