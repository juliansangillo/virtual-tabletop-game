using Zenject;

namespace NaughtyBiker.Signals.Installers {
	/**
    * A Zenject base installer that installs bindings for the Zenject signal bus and declares the InitializeSignal and 
    * StateChangeSignal.
    * 
    * @author Julian Sangillo
    * @version 2.0
    */
	public class SignalsBaseInstaller : Installer<SignalsBaseInstaller> {
		/**
        * A callback from Zenject that installs the signal bus and its dependencies to the DI Container for future dependency injection.
        * It also declares the Utility API signals such as InitializeSignal and StateChangeSignal. This is called by Zenject during 
        * binding and should NOT be called directly!
        */
		public override void InstallBindings() {
			SignalBusInstaller.Install(Container);

			Container.DeclareSignal<InitializeSignal>();
			Container.DeclareSignal<StateChangeSignal>().OptionalSubscriber();
		}
	}
}