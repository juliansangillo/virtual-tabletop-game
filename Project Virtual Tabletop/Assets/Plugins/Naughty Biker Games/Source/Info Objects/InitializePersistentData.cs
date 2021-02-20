using UnityEngine;
using Zenject;
using NaughtyBiker.Signals;

namespace NaughtyBiker.InfoObjects {
    /**
    * A monobehaviour that will fire an InitializeSignal during Unity's start phase once attached to a game object. You can use
    * this signal to initialize data, including info object data and firing other signals, across your game at the start of each scene.<br>
    *
    * Component Menu: "Naughty Biker Games / Info Objects / Initialize Persistent Data"
    *
    * @author Julian Sangillo
    * @version 2.0
    */
    [AddComponentMenu("Naughty Biker Games/Info Objects/Initialize Persistent Data")]
    public class InitializePersistentData : MonoBehaviour {
        private SignalBus signalBus;

        /**
        * Construction method. Used to initialize monobehaviours
        *
        * @param signalBus The Zenject signal bus. Used to fire Zenject signals
        */
        [Inject]
        public void Construct(SignalBus signalBus) {
            this.signalBus = signalBus;
        }

        private void Start() {
            signalBus.Fire(new InitializeSignal());
        }
    }
}