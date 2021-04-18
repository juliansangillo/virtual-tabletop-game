using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Signals/Installers/Token Signals Mono Installer")]
	public class TokenSignalsMonoInstaller : MonoInstaller<TokenSignalsMonoInstaller> {
        public override void InstallBindings() {
            TokenSignalsInstaller.Install(Container);
        }
	}
}