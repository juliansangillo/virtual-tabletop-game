using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.Interpolation.Installers {
    /**
    * A Zenject mono installer that installs bindings for the Lerper dependencies. To use, attach this installer 
    * to a game object and reference it in the scene context or project context.
    *
    * Component Menu: "Naughty Biker Games / SDK / Interpolation / Installers / Lerper Mono Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 4.0
    * @since 3.0
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Interpolation/Installers/Lerper Mono Installer")]
	public class LerperMonoInstaller : MonoInstaller<LerperMonoInstaller> {
        public override void InstallBindings() {
            LerperInstaller.Install(Container);
        }
	}
}