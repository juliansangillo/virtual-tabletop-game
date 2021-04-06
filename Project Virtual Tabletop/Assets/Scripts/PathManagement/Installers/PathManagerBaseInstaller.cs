using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Installers {
	public class PathManagerBaseInstaller : Installer<PathManagerBaseInstaller> {
        private readonly GridDetails gridDetails;
        private readonly IPathFinder pathFinder;
        private readonly SignalBus signalBus;

        [Inject]
        public PathManagerBaseInstaller(GridDetails gridDetails, SignalBus signalBus) {
            this.gridDetails = gridDetails;
            this.pathFinder = new PathFinderAdapter();
            this.signalBus = signalBus;
        }

		public override void InstallBindings() {
			Container.BindInterfacesTo<IPathManager>()
                .FromInstance(new PathManager(gridDetails, pathFinder, signalBus))
                .AsSingle();
		}
	}
}