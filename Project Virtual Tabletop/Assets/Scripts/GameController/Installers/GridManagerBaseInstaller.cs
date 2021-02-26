using Zenject;

namespace ProjectVirtualTabletop.GameController.Installers {
	public class GridManagerBaseInstaller : Installer<GridManagerBaseInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<GridManager>().AsSingle();
		}
	}
}