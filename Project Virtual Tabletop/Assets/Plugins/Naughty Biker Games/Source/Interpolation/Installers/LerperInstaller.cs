using Zenject;

namespace NaughtyBikerGames.SDK.Interpolation.Installers {
	/**
    * A Zenject installer that installs bindings for the Lerper dependencies. To use, call
    * LerperInstaller.Install(Container).
    * 
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 4.0
    * @since 3.0
    */
	public class LerperInstaller : Installer<LerperInstaller> {
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