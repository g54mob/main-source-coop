using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Networked
{
	public class MonkeyPorterRunModelNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<MonkeyPorterRunModel>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(MonkeyPorterRunModel), ModelScope.Run, ModelOwnership.Shared, "MonkeyPorterRunNetworkObject", typeof(MonkeyPorterRunNetworkObject), (NetworkedModelBase model) => new MonkeyPorterRunModelBridge((MonkeyPorterRunModel)model))).AsCached();
		}
	}
}
