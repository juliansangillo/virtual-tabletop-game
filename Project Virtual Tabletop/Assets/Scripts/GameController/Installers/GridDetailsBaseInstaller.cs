using System.Collections.Generic;
using ProjectVirtualTabletop.Entities;
using Zenject;

namespace ProjectVirtualTabletop.GameController.Installers {
	public class GridDetailsBaseInstaller : Installer<GridDetailsBaseInstaller> {
		public override void InstallBindings() {
			Container.Bind<GridDetails>()
                .FromInstance(GetTemporaryGridDetails())
                .AsSingle();
		}

        private GridDetails GetTemporaryGridDetails() {
            Token token = new Token(new GridSpace(9, 9));

            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 10;
            gridDetails.NumberOfColumns = 10;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token);

            return gridDetails;
        }
	}
}