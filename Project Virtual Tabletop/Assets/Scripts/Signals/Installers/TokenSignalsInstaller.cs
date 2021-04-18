using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers {
	public class TokenSignalsInstaller : Installer<TokenSignalsInstaller> {
		public override void InstallBindings() {
			Container.DeclareSignal<TokenSelectedSignal>().OptionalSubscriber();
            Container.DeclareSignal<TokenDraggedSignal>().OptionalSubscriber();
            Container.DeclareSignal<TokenReleasedSignal>().OptionalSubscriber();
		}
	}
}