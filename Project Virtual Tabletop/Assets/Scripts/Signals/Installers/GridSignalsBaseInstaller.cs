using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class GridSignalsBaseInstaller : Installer<GridSignalsBaseInstaller> {
		public override void InstallBindings() {
			Container.DeclareSignal<GridInitializeSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridMoveSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridAddSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridRemoveSignal>().OptionalSubscriber();
		}
	}
}