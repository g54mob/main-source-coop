using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts.Networked
{
	public class LevelPlayersGameStatisticsModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<LevelPlayersGameStatisticsModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(LevelPlayersGameStatisticsModel), ModelScope.Session, ModelOwnership.Shared, "LevelPlayersGameStatisticsNetworkObject", typeof(LevelPlayersGameStatisticsNetworkObject), (NetworkedModelBase model) => new LevelPlayersGameStatisticsModelBridge((LevelPlayersGameStatisticsModel)model))).AsCached();
		}
	}
}
