using System.Collections.Generic;
using NaughtyBikerGames.SDK.InfoObjects.Delegates;

namespace NaughtyBikerGames.SDK.InfoObjects.Interfaces {
    /**
    * Stores info on another game object for the purpose of persisting this data across scenes. Uses
    * key-value pairs to store the data. Keys must be strings while values could be of any type.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    *
    * @see IInfoObject
    */
    public interface IInfo {
        /// The current dictionary where all info is hashed
        IDictionary<string, object> Data { get; }

        /// The delegate that gets called when the state of this info object has been changed
        StateChange StateChanged { set; }

        /// A string that can be used as an identifier for this collection of info. Should be unique
        string Id { get; set; }

        /**
        * A custom indexer for the data in this info object. When getting values with the indexer, the result will need 
        * to be cast to the appropriate type. When setting values, if the key does not already exist, it will add it.
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
        object this[string key] { get; set; }

        /**
        * Gets the value of the provided key. Returned value must be manually cast to the needed type.
        *
        * @param key The key string that uniquely identifies a stored value
        * 
        * @return The value of the key
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        * @throws KeyNotFoundException Raised when the key parameter doesn't exist in this info object
        */
        object Get(string key);

        /**
        * Sets the value of the provided key. When setting values, if the key does not already exist, it will add
        * it.
        *
        * @param key The key string that uniquely identifies a stored value
        * @param value The value to assign to the given key. May be of any type
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        */
        void Set(string key, object value);

        /**
        * Check if the key exists in this info object
        *
        * @param key The key string that uniquely identifies a stored value
        *
        * @return True if info contains the key or false otherwise
        *
        * @throws ArgumentNullException Raised when the key parameter is null
        * @throws ArgumentException Raised when the key parameter is an empty string
        */
        bool Exists(string key);
    }
}