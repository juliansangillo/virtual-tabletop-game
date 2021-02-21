using Zenject;

namespace ProjectVirtualTabletop.GameController.Installers {
	public class SpaceManagerBaseInstaller : Installer<SpaceManagerBaseInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<SpaceManager>().AsSingle();
		}
	}
}