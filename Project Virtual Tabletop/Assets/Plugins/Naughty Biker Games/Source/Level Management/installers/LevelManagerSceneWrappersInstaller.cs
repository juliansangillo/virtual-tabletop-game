using NaughtyBiker.Wrappers;
using NaughtyBiker.Wrappers.Interfaces;

namespace NaughtyBiker.LevelManagement.Installers {
	/**
    * A Zenject installer that installs bindings for the LevelManager API plus the SceneManager and SceneUtility wrappers.
    * 
    * @author Julian Sangillo
    * @version 2.0
    */
    public class LevelManagerSceneWrappersInstaller : LevelManagerBaseInstaller {
        /**
        * A callback from Zenject that binds LevelManager and its dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
        public override void InstallBindings() {
            base.InstallBindings();

            Container.Bind<ISceneManager>()
                .To<SceneManagerWrapper>()
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
            Container.Bind<ISceneUtility>()
                .To<SceneUtilityWrapper>()
                .AsSingle()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
        }
    }
}