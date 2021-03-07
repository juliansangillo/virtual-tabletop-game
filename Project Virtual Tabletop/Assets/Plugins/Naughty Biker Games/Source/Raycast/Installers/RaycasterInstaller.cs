using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.Raycast.Installers {
    /**
    * A Zenject monoinstaller that installs bindings for the Raycaster API dependencies.
    *
    * Component Menu: "Naughty Biker Games / SDK / Raycast / Installers / Raycaster Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    * 
    * @see Raycaster
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Raycast/Installers/Raycaster Installer")]
	public class RaycasterInstaller : MonoInstaller<RaycasterInstaller> {
        /**
        * A callback from Zenject that binds the object and its dependencies. Do NOT call this method directly!
        * Instead, attach this installer to a Game Object as a Component and reference it in either the scene
        * context or the project context.
        */
        public override void InstallBindings() {
            RaycasterBaseInstaller.Install(Container);
        }
	}
}