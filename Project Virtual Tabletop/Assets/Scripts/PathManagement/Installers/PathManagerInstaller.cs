using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Installers {
	public class PathManagerInstaller : MonoInstaller<PathManagerInstaller> {
        public override void InstallBindings() {
            PathManagerBaseInstaller.Install(Container);
        }
	}
}