using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.InfoObjects.Components;
using NaughtyBikerGames.SDK.InfoObjects.Delegates;
using NaughtyBikerGames.SDK.InfoObjects.Interfaces;

namespace NaughtyBikerGames.SDK.InfoObjects.Installers {
	/**
    * A Zenject installer that installs bindings for the InfoObject API dependencies.
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 2.0
    */
    public class InfoObjectBaseInstaller : Installer<InfoObjectBaseInstaller> {
        /**
        * A callback from Zenject that binds InfoObject dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
        public override void InstallBindings() {
            Container.Bind<CreateInfoObject>().FromMethod(CreateCallbackForInfoObject).AsSingle();
            Container.Bind<IInfo>().To<Info>().FromMethod(CreateInfo).AsTransient();
        }

        private CreateInfoObject CreateCallbackForInfoObject() {
            return (prefab, tag) => OnCreateInfoObject(prefab, tag);
        }

        private InfoObject OnCreateInfoObject(GameObject infoObjectPrefab, string objectTag) {
            GameObject obj = Container.InstantiatePrefab(infoObjectPrefab);
            
            obj.tag = objectTag;
            
            return obj.GetComponent<InfoObject>();
        }

        private Info CreateInfo() {
            return new Info("Default");
        }
    }
}