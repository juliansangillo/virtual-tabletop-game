using Zenject;

namespace NaughtyBikerGames.SDK.Interpolation.Installers {
	/**
    * A Zenject installer that installs bindings for the Lerper dependencies.
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    */
	public class LerperBaseInstaller : Installer<LerperBaseInstaller> {
		/**
        * A callback from Zenject that binds the Lerper to the DI Container for future dependency injection. This is 
        * called by Zenject during binding and should NOT be called directly!
        */
		public override void InstallBindings() {
			Container.BindInterfacesTo<Lerper>()
                .AsSingle();
		}
	}
}