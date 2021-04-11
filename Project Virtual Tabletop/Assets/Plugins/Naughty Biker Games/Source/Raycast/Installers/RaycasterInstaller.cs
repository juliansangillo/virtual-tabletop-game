using Zenject;
using NaughtyBikerGames.SDK.Adapters;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Raycast.Installers {
    /**
    * A Zenject installer that installs bindings for the Raycaster API dependencies. To use, call
    * RaycasterInstaller.Install(Container).
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 4.0
    * @since 3.0
    *
    * @see Raycaster
    */
	public class RaycasterInstaller : Installer<RaycasterInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesTo<Raycaster>()
                .AsSingle();
            
            Container.Bind<IInput>()
                .To<InputAdapter>()
                .AsSingle()
                .WhenInjectedInto<Raycaster>();
            Container.Bind<IMainCamera>()
                .To<MainCameraAdapter>()
                .AsSingle()
                .WhenInjectedInto<Raycaster>();
            Container.Bind<IPhysics>()
                .To<PhysicsAdapter>()
                .AsSingle()
                .WhenInjectedInto<Raycaster>();
		}
	}
}