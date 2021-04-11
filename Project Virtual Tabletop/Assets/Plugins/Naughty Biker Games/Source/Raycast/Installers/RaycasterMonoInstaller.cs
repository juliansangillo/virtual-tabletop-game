using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.Raycast.Installers {
    /**
    * A Zenject mono installer that installs bindings for the Raycaster API dependencies. To use, attach this installer 
    * to a game object and reference it in the scene context or project context.
    *
    * Component Menu: "Naughty Biker Games / SDK / Raycast / Installers / Raycaster Mono Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 4.0
    * @since 3.0
    * 
    * @see Raycaster
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Raycast/Installers/Raycaster Mono Installer")]
	public class RaycasterMonoInstaller : MonoInstaller<RaycasterMonoInstaller> {
        public override void InstallBindings() {
            RaycasterInstaller.Install(Container);
        }
	}
}