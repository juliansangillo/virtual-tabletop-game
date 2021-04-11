using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.SDK.Signals.Installers {
    /**
    * A Zenject mono installer that installs bindings for the Zenject signal bus. This is required in order to use
    * Zenject Signals. To use, attach this installer to a game object and reference it in the scene context or 
    * project context.
    *
    * Component Menu: "Naughty Biker Games / SDK / Signals / Installers / Signal Bus Mono Installer"
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 4.0
    * @since 4.0
    */
    [AddComponentMenu("Naughty Biker Games/SDK/Signals/Installers/Signal Bus Mono Installer")]
	public class SignalBusMonoInstaller : MonoInstaller<SignalBusMonoInstaller> {
        public override void InstallBindings() {
            SignalBusInstaller.Install(Container);
        }
	}
}