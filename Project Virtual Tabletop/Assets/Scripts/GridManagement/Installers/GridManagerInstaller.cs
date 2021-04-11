using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
	public class GridManagerInstaller : Installer<GridManagerInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<GridManager>()
				.FromNew()
				.AsSingle();
		}
	}
}