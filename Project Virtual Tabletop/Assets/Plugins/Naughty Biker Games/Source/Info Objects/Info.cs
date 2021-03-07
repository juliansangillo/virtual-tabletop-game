using System;
using System.Collections.Generic;
using NaughtyBikerGames.SDK.InfoObjects.Delegates;
using NaughtyBikerGames.SDK.InfoObjects.Interfaces;

namespace NaughtyBikerGames.SDK.InfoObjects {
    /**
    * Default implementation of IInfo
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    *
    * @see InfoObject
    */
    public class Info : IInfo {
        private readonly IDictionary<string, object> data;
        
        private StateChange stateChanged;
        private string id;

        /// Default implementation of IInfo.Data
        public IDictionary<string, object> Data {
            get {
                return data;
            }
        }

        /// Default implementation of IInfo.StateChanged
        public StateChange StateChanged {
            set {
                stateChanged = value;
            }
        }

        /// Default implementation of IInfo.Id
        public string Id {
            get {
                return id;
            }
            set {
                id = value;
            }
        }

        /**
        * Default implementation of IInfo.this[string key]
        *
        * @param key The key string that uniquely identifies a stored value
        * @param value The value to assign to the given key. May be of any type
        *
        * @return The value identified by its key
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        * @throws KeyNotFoundException Raised when the key parameter doesn't exist in this info object
        */
        public object this[string key] {
            get {
                return Get(key);
            }
            set {
                Set(key, value);
            }
        }

        /**
        * @param infoId A string that can be used as an identifier for this collection of info. Should be unique
        */
        public Info(string infoId) {
            this.Id = infoId;
            this.data = new Dictionary<string, object>();
            this.StateChanged = (id, key, value) => {};
        }

        /**
        * @param infoId A string that can be used as an identifier for this collection of info. Should be unique
        * @param data A dictionary where all info will be hashed
        */
        public Info(string infoId, IDictionary<string, object> data) {
            this.Id = infoId;
            this.data = data;
            this.StateChanged = (id, key, value) => {};
        }

        /**
        * @param infoId A string that can be used as an identifier for this collection of info. Should be unique
        * @param stateChanged The delegate that gets called when the state of this info object has been changed
        */
        public Info(string infoId, StateChange stateChanged) {
            this.Id = infoId;
            this.data = new Dictionary<string, object>();
            this.StateChanged = stateChanged;
        }

        /**
        * @param infoId A string that can be used as an identifier for this collection of info. Should be unique
        * @param data A dictionary where all info will be hashed
        * @param stateChanged The delegate that gets called when the state of this info object has been changed
        */
        public Info(string infoId, IDictionary<string, object> data, StateChange stateChanged) {
            this.Id = infoId;
            this.data = data;
            this.StateChanged = stateChanged;
        }

        /**
        * Default implementation of IInfo.Get(string key)
        *
        * @param key The key string that uniquely identifies a stored value
        * 
        * @return The value of the key
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        * @throws KeyNotFoundException Raised when the key parameter doesn't exist in this info object
        */
        public object Get(string key) {
            if(string.IsNullOrWhiteSpace(key))
                if(key == null)
                    throw new ArgumentNullException("Cannot find value when key is 'null'");
                else
                    throw new ArgumentException("Cannot find value when key is an 'empty_string'");

            object value;

            if(!data.TryGetValue(key, out value))
                throw new KeyNotFoundException("Unable to return value of an unknown key!");

            return value;
        }

        /**
        * Default implementation of IInfo.Set(string key, object value)
        * 
        * @param key The key string that uniquely identifies a stored value
        * @param value The value to assign to the given key. May be of any type
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        */
        public void Set(string key, object value) {
            if(string.IsNullOrWhiteSpace(key))
                if(key == null)
                    throw new ArgumentNullException("Cannot set value of a 'null' key");
                else
                    throw new ArgumentException("Cannot set value of an 'empty_string' key");

            if(data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);

            stateChanged(this.Id, key, value);
        }

        /**
        * Default implementation of IInfo.Exists(string key)
        *
        * @param key The key string that uniquely identifies a stored value
        *
        * @return True if info contains the key or false otherwise
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        */
        public bool Exists(string key) {
            if(string.IsNullOrEmpty(key))
                if(key == null)
                    throw new ArgumentNullException("Cannot find a 'null' key");
                else
                    throw new ArgumentException("Cannot find an 'empty_string' key");
                    
            return data.ContainsKey(key);
        }
    }
}