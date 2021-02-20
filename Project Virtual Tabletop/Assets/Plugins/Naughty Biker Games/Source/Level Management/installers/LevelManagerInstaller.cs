using UnityEngine;
using Zenject;

namespace NaughtyBiker.LevelManagement.Installers {
	/**
    * A Zenject monoinstaller that installs bindings for the LevelManager API dependencies. It is recommended to attach 
    * this installer to the project context.
    *
    * Component Menu: "Naughty Biker Games / Zenject Installers / Level Manager Installer"
    * 
    * @author Julian Sangillo
    * @version 2.0
    * 
    * @see LevelManager
    */
	[AddComponentMenu("Naughty Biker Games/Zenject Installers/Level Manager Installer")]
    public class LevelManagerInstaller : MonoInstaller<LevelManagerInstaller> {
        /**
        * A callback from Zenject that binds LevelManager and its dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
        public override void InstallBindings() {
            LevelManagerSceneWrappersInstaller.Install(Container);
        }
    }
}