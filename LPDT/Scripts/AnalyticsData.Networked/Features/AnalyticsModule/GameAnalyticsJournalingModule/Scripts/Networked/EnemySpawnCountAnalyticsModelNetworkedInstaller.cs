using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Networked
{
	public class EnemySpawnCountAnalyticsModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<EnemySpawnCountAnalyticsModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(EnemySpawnCountAnalyticsModel), ModelScope.Level, ModelOwnership.Shared, "EnemySpawnCountAnalyticsNetworkObject", typeof(EnemySpawnCountAnalyticsNetworkObject), (NetworkedModelBase model) => new EnemySpawnCountAnalyticsModelBridge((EnemySpawnCountAnalyticsModel)model))).AsCached();
		}
	}
}
