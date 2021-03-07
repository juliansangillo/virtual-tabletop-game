using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.Interpolation.Installers {
    /**
    * A Zenject monoinstaller that installs bindings for the Lerper dependencies. To use, attach this installer 
    * to a game object and reference it in the scene context or project context.
    *
    * Component Menu: "Naughty Biker Games / SDK / Interpolation / Installers / Lerper Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Interpolation/Installers/Lerper Installer")]
	public class LerperInstaller : MonoInstaller<LerperInstaller> {
        /**
        * A callback from Zenject that binds the Lerper to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
        public override void InstallBindings() {
            LerperBaseInstaller.Install(Container);
        }
	}
}