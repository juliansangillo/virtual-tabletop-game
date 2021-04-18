using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class GridSignalsInstaller : Installer<GridSignalsInstaller> {
		public override void InstallBindings() {
			Container.DeclareSignal<GridInitializedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridMovedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridAddedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridRemovedSignal>().OptionalSubscriber();
		}
	}
}