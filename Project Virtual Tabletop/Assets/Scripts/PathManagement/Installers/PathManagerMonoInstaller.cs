using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Path Management/Installers/Path Manager Mono Installer")]
	public class PathManagerMonoInstaller : MonoInstaller<PathManagerMonoInstaller> {
        public override void InstallBindings() {
            PathManagerInstaller.Install(Container);
        }
	}
}