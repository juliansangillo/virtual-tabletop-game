using System.Collections.Generic;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities.Installers {
	public class GridDetailsInstaller : Installer<GridDetailsInstaller> {
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