using UnityEngine;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Installers {
    [AddComponentMenu("Naughty Biker Games/Project Virtual Tabletop/Adapters/Installers/Vector Line Mono Installer")]
	public class VectorLineMonoInstaller : MonoInstaller<VectorLineMonoInstaller> {
        public override void InstallBindings() {
            VectorLineInstaller.Install(Container);
        }
	}
}