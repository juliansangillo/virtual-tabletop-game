using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Grid Management/Installers/Grid Manager Mono Installer")]
	public class GridManagerMonoInstaller : MonoInstaller<GridManagerMonoInstaller> {
        public override void InstallBindings() {
            GridManagerInstaller.Install(Container);
        }
	}
}