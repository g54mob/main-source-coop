using Features.GamePhasesModule.Scripts.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.GamePhasesModule.Scripts.Networked
{
	public class GamePhasesModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<GamePhasesModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(GamePhasesModel), ModelScope.Level, ModelOwnership.Shared, "GamePhasesNetworkObject", typeof(GamePhasesNetworkObject), (NetworkedModelBase model) => new GamePhasesModelBridge((GamePhasesModel)model))).AsCached();
		}
	}
}
