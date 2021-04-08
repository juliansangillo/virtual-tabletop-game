using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class GridSignalsBaseInstaller : Installer<GridSignalsBaseInstaller> {
		public override void InstallBindings() {
			Container.DeclareSignal<GridInitializeSignal>();
            Container.DeclareSignal<GridMoveSignal>();
            Container.DeclareSignal<GridAddSignal>();
            Container.DeclareSignal<GridRemoveSignal>();
		}
	}
}