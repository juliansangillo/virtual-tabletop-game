using Zenject;

namespace ProjectVirtualTabletop.GameController.Installers {
	public class GridManagerInstaller : MonoInstaller<GridManagerInstaller> {
        public override void InstallBindings() {
            GridManagerBaseInstaller.Install(Container);
        }
	}
}