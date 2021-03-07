using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
	public class GridManagerInstaller : MonoInstaller<GridManagerInstaller> {
        public override void InstallBindings() {
            GridManagerBaseInstaller.Install(Container);
        }
	}
}