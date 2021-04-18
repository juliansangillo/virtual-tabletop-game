using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Entities/Installers/Grid Details Mono Installer")]
	public class GridDetailsMonoInstaller : MonoInstaller<GridDetailsMonoInstaller> {
        public override void InstallBindings() {
            GridDetailsInstaller.Install(Container);
        }
	}
}