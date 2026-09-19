using Zenject;

namespace Features.NetworkTelemetry.Scripts.Installers
{
	public class NetworkTelemetryInstaller : Installer<NetworkTelemetryInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<TelemetryService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<TelemetryMediator>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<TelemetryEventBusObserver>().AsSingle();
		}
	}
}
