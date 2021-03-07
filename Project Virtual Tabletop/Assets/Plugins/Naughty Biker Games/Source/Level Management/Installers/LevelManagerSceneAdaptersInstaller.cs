using NaughtyBikerGames.SDK.Adapters;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.LevelManagement.Installers {
	/**
    * A Zenject installer that installs bindings for the LevelManager API plus the SceneManager and SceneUtility adapters.
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 2.0
    */
    public class LevelManagerSceneAdaptersInstaller : LevelManagerBaseInstaller {
        /**
        * A callback from Zenject that binds LevelManager and its dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
        public override void InstallBindings() {
            base.InstallBindings();

            Container.Bind<ISceneManager>()
                .To<SceneManagerAdapter>()
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
            Container.Bind<ISceneUtility>()
                .To<SceneUtilityAdapter>()
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
        }
    }
}