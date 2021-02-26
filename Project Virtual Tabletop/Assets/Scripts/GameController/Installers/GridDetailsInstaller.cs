using Zenject;

namespace ProjectVirtualTabletop.GameController.Installers {
	public class GridDetailsInstaller : MonoInstaller<GridDetailsInstaller> {
        public override void InstallBindings() {
            GridDetailsBaseInstaller.Install(Container);
        }
	}
}