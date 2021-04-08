using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class GridSignalsInstaller : MonoInstaller<GridSignalsInstaller> {
        public override void InstallBindings() {
            GridSignalsBaseInstaller.Install(Container);
        }
	}
}