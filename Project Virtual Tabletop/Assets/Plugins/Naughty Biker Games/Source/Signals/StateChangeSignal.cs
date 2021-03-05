namespace NaughtyBikerGames.SDK.Signals {
    /**
    * A Zenject signal that can be fired from the signal bus every time the state of an object with key-value pairs change.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    *
    * @see SignalsInstaller
    */
    public class StateChangeSignal {

        private readonly string objectId;
        private readonly string key;
        private readonly object value;

        /// The object's identifier
        public string ObjectId {
            get {
                return objectId;
            }
        }

        /// The key whose value was changed
        public string Key {
            get {
                return key;
            }
        }

        /// The new value
        public object Value {
            get {
                return value;
            }
        }

        /**
        * @param objectId Unique identifier of the object that changed state
        * @param key The key in the object whose value was changed
        * @param value The new value
        */
        public StateChangeSignal(string objectId, string key, object value) {
            this.objectId = objectId;
            this.key = key;
            this.value = value;
        }
        
    }
}