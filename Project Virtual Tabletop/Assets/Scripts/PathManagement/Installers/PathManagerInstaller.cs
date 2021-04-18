using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Installers {
	public class PathManagerInstaller : Installer<PathManagerInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<PathManager>()
                .FromNew()
                .AsTransient();

            Container.BindInterfacesTo<PathFinderAdapter>()
                .FromNew()
                .AsSingle();
		}
	}
}