using System.Collections.Generic;
using Zenject;

namespace NaughtyBikerGames.SDK.LevelManagement.Installers {
	/**
    * A Zenject base installer that installs bindings for the LevelManager API dependencies.
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 2.0
    * 
    * @see LevelManager
    */
	public class LevelManagerBaseInstaller : Installer<LevelManagerBaseInstaller> {
        /**
        * A callback from Zenject that binds LevelManager and its dependencies to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
		public override void InstallBindings() {
			Container.BindInterfacesTo<LevelManager>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesTo<Dictionary<string, int>>()
                .AsTransient()
                .WhenInjectedInto<LevelManager>()
                .NonLazy();
		}
	}
}