using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.Signals;
using NaughtyBikerGames.SDK.InfoObjects.Interfaces;
using NaughtyBikerGames.SDK.InfoObjects.Delegates;

namespace NaughtyBikerGames.SDK.InfoObjects.Components {
    /**
    * A monobehaviour that is the info object control script. Its responsibility is to maintain an info object.
    * When loaded, it will search the scene for its info object. If found, it will reference that object so others can
    * use it. If not found, it will request Zenject to instantiate a default one for it to use.<br>
    *
    * Component Menu: "Naughty Biker Games / SDK / Info Objects / Components / Info Object Control"
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    * 
    * @see InfoObjectInstaller
    * @see InitializePersistentData
    * @see NaughtyBiker.Signals.InitializeSignal
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Info Objects/Components/Info Object Control")]
    public class InfoObjectControl : MonoBehaviour {
        [SerializeField] private GameObject infoObjectPrefab = null;
        [SerializeField] private string objectTag = "";

        private IInfoObject infoObj;
        private CreateInfoObject createInfoObject;
        private SignalBus signalBus;

        /** 
        * A reference to the prefab of an info object. By default, this 
        * references the default prefab provided by the SDK located in 
        * the Resources folder. This can be overriden with a custom prefab.
        */
        public GameObject InfoObjectPrefab {
            set {
                this.infoObjectPrefab = value;
            }
        }

        /**
        * The tag to set on a new info object used to identify that object. 
        * Please verify that such a tag exists in Unity's tag list before 
        * applying. This must be unique!
        */
        public string ObjectTag { 
            set {
                this.objectTag = value;
            }
        }

        /// The current info object anchored to this control script
        public IInfoObject InfoObj {
            get {
                return infoObj;
            }
        }

        /**
        * Construction method. Used to initialize monobehaviours
        *
        * @param createInfoObject Delegate function from the installer that requests Zenject to create an info object
        * @param signalBus The Zenject signal bus. Used to fire Zenject signals
        */
        [Inject]
        public void Construct(CreateInfoObject createInfoObject, SignalBus signalBus) {

            this.createInfoObject = createInfoObject;
            this.signalBus = signalBus;

            infoObjectPrefab = Resources.Load<GameObject>("Info Object");

        }

        public void Start() {
            GameObject obj = GameObject.FindWithTag(objectTag);

            if(obj != null)
                this.infoObj = obj.GetComponent<IInfoObject>();
            else
                this.infoObj = createInfoObject(infoObjectPrefab, objectTag);

            signalBus.Subscribe<InitializeSignal>(this.infoObj.FireAll);
        }
    }
}