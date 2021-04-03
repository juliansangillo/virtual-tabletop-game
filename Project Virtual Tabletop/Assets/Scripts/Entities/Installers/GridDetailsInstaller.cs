using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities.Installers {
	public class GridDetailsInstaller : MonoInstaller<GridDetailsInstaller> {
        public override void InstallBindings() {
            GridDetailsBaseInstaller.Install(Container);
        }
	}
}