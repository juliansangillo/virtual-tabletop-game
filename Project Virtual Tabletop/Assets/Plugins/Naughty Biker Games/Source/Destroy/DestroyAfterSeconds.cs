using UnityEngine;

namespace NaughtyBiker.Destroy {
	/**
    * A monobehaviour that destroys the game object it is attached to after some period of time. The user
    * can decide how many seconds until object is destroyed.<br>
    *
    * Component Menu: "Naughty Biker Games / Destroy / Destroy After Seconds"
    *
    * @author   Julian Sangillo
    * @version  2.0
    */
	[AddComponentMenu("Naughty Biker Games/Destroy/Destroy After Seconds")]
    public class DestroyAfterSeconds : MonoBehaviour {
        [SerializeField] private float seconds = 0;

        private float timer = 0f;

        /// Length of time in seconds until game object is destroyed.
        public float Seconds {
            set {
                this.seconds = value;
            }
        }

        private void Start() {
            timer = seconds;
        }

        private void Update() {
            timer -= Time.deltaTime;
            if(timer <= 0f)
                Destroy(gameObject);
        }
    }
}