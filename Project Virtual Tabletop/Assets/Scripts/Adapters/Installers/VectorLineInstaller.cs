using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Installers {
	public class VectorLineInstaller : Installer<VectorLineInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<VectorLineAdapter>()
                .FromNew()
                .AsTransient();
		}
	}
}