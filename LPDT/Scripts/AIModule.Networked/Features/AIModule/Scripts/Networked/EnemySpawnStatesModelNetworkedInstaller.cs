using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.AIModule.Scripts.Networked
{
	public class EnemySpawnStatesModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<EnemySpawnStatesModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(EnemySpawnStatesModel), ModelScope.Level, ModelOwnership.Shared, "EnemySpawnStatesNetworkObject", typeof(EnemySpawnStatesNetworkObject), (NetworkedModelBase model) => new EnemySpawnStatesModelBridge((EnemySpawnStatesModel)model))).AsCached();
		}
	}
}
