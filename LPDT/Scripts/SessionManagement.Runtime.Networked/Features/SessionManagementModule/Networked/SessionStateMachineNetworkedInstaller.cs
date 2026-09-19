using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	public class SessionStateMachineNetworkedInstaller : INetworkedModelShadowInstaller
	{
		public void Install(DiContainer container)
		{
			container.Bind<SessionStateMachine>().AsSingle().NonLazy();
			container.Bind<NetworkedModelShadowDescriptor>().FromInstance(new NetworkedModelShadowDescriptor(typeof(SessionStateMachine), ModelScope.Session, ModelOwnership.Shared, "SessionStateMachineNetworkObject", typeof(SessionStateMachineNetworkObject), (NetworkedModelBase model) => new SessionStateMachineBridge((SessionStateMachine)model))).AsCached();
		}
	}
}
