using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.InfoObjects.Installers {
	/**
    * A Zenject monoinstaller that installs bindings for the InfoObject API dependencies. To use, attach this installer 
    * to a game object and reference it in the scene context or project context.
    *
    * Component Menu: "Naughty Biker Games / SDK / Info Objects / Installers / Info Object Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 1.0
    */
	[AddComponentMenu("Naughty Biker Games/SDK/Info Objects/Installers/Info Object Installer")]
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