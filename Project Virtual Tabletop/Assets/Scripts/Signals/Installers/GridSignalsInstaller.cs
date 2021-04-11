using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class GridSignalsInstaller : Installer<GridSignalsInstaller> {
		public override void InstallBindings() {
			Container.DeclareSignal<GridInitializeSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridMoveSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridAddSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridRemoveSignal>().OptionalSubscriber();
		}
	}
}