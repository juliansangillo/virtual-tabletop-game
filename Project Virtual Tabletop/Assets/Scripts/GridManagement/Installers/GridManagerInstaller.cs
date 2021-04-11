using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Factories;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
	public class GridManagerInstaller : Installer<GridManagerInstaller> {
		private GridDetails gridDetails;
        private SignalBus signalBus;

		[Inject]
		public GridManagerInstaller(GridDetails gridDetails, SignalBus signalBus) {
			this.gridDetails = gridDetails;
            this.signalBus = signalBus;
		}

		public override void InstallBindings() {
			Container.BindInterfacesTo<GridManager>()
				.FromInstance(new GridManagerFactory().CreateGridManager(gridDetails, signalBus))
				.AsSingle();
		}
	}
}