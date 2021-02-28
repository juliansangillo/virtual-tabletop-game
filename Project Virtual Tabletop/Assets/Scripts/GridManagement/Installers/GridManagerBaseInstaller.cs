using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Factories;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
	public class GridManagerBaseInstaller : Installer<GridManagerBaseInstaller> {
		private GridDetails gridDetails;

		[Inject]
		public GridManagerBaseInstaller(GridDetails gridDetails) {
			this.gridDetails = gridDetails;
		}

		public override void InstallBindings() {
			Container.BindInterfacesTo<GridManager>()
				.FromInstance(new GridManagerFactory().CreateGridManager(gridDetails))
				.AsSingle();
		}
	}
}