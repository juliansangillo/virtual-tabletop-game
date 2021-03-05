using UnityEngine;

namespace NaughtyBikerGames.SDK.Destroy {
	/**
    * A monobehaviour that marks the game object it is attached to as "Don't Destroy On Load." 
    * Setting an object as DontDestroyOnLoad in Unity means the object will persist between scenes
    * as opposed to being destroyed when the scene is unloaded from memory.<br>
    *
    * Component Menu: "Naughty Biker Games / SDK / Destroy / Don't Destroy On Load"
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    */
	[AddComponentMenu("Naughty Biker Games/SDK/Destroy/Don't Destroy On Load")]
    public class DontDestroyOnLoad : MonoBehaviour {
        [SerializeField] private bool preserveDuplicates = false;

        /// When disabled, it will search for duplicate objects via their tags and will destroy itsef if found. If enabled, it skips this process.
		public bool PreserveDuplicates {
            set {
                this.preserveDuplicates = value;
            }
        }

        public void Start() {
            DontDestroy();
        }

        /// Marks this game object as DontDestroyOnLoad or destroys itself if another game object with the same tag exists.
        public void DontDestroy() {
			if(!preserveDuplicates) {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);

                if(objects.Length > 1) {
                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);
		}
    }
}