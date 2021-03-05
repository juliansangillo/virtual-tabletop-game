using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.Signals;

namespace NaughtyBikerGames.SDK.InfoObjects.Components {
    /**
    * A monobehaviour that will fire an InitializeSignal during Unity's start phase once attached to a game object. You can use
    * this signal to initialize data, including info object data and firing other signals, across your game at the start of each scene.<br>
    *
    * Component Menu: "Naughty Biker Games / SDK / Info Objects / Components / Initialize Persistent Data"
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Info Objects/Components/Initialize Persistent Data")]
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

        public void Start() {
            signalBus.Fire(new InitializeSignal());
        }
    }
}