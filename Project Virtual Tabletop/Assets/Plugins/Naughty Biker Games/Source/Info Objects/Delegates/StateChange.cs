namespace NaughtyBikerGames.SDK.InfoObjects.Delegates {
	/**
    * A delegate function that will fire the StateChangeSignal when a callback is recieved from the IInfo. This will
    * happen when the value of one of its keys was modified.
    *
    * @param id The unique identifier (the gameObject tag) that fired the signal so the subscribers to the signal know which
    * info object had their state changed
    * @param k The key that uniquely identifies the value that changed
    * @param v The new value of the key. May be of any type
    */
    public delegate void StateChange(string id, string k, object v);
}