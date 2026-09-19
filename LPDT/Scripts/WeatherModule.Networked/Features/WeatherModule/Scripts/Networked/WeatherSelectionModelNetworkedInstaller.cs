using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.WeatherModule.Scripts.Networked
{
	public class WeatherSelectionModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<WeatherSelectionModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(WeatherSelectionModel), ModelScope.Level, ModelOwnership.Shared, "WeatherSelectionNetworkObject", typeof(WeatherSelectionNetworkObject), (NetworkedModelBase model) => new WeatherSelectionModelBridge((WeatherSelectionModel)model))).AsCached();
		}
	}
}
