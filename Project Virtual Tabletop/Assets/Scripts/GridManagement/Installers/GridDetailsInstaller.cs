using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
	public class GridDetailsInstaller : MonoInstaller<GridDetailsInstaller> {
        public override void InstallBindings() {
            GridDetailsBaseInstaller.Install(Container);
        }
	}
}