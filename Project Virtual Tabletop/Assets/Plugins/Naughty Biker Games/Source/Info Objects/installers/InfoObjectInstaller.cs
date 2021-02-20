using UnityEngine;
using Zenject;

namespace NaughtyBiker.InfoObjects.Installers {
	/**
    * A Zenject monoinstaller that installs bindings for the InfoObject API dependencies. To use, attach this installer 
    * to a game object and reference it in the scene context or project context.
    *
    * Component Menu: "Naughty Biker Games / Zenject Installers / Info Object Installer"
    * 
    * @author Julian Sangillo
    * @version 2.0
    */
	[AddComponentMenu("Naughty Biker Games/Zenject Installers/Info Object Installer")]
    public class InfoObjectInstaller : MonoInstaller<InfoObjectInstaller> {
        /**
        * A callback from Zenject that binds InfoObject dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
		public override void InstallBindings() {
            InfoObjectBaseInstaller.Install(Container);
		}
    }    
}