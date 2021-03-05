using Zenject;
using NaughtyBikerGames.SDK.Adapters;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Raycast.Installers {
    /**
    * A Zenject base installer that installs bindings for the Raycaster API dependencies.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    *
    * @see Raycaster
    */
	public class RaycasterBaseInstaller : Installer<RaycasterBaseInstaller> {
        /**
        * A callback from Zenject that binds the object and its dependencies. Do NOT call this method directly!
        * Instead, use RaycasterBaseInstaller.Install(Container) to install these bindings.
        */
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