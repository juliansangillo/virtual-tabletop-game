using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Signals/Installers/Grid Signals Mono Installer")]
	public class GridSignalsMonoInstaller : MonoInstaller<GridSignalsMonoInstaller> {
        public override void InstallBindings() {
            GridSignalsInstaller.Install(Container);
        }
	}
}