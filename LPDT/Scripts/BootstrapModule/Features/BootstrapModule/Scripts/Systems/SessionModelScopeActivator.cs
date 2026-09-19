using Cysharp.Threading.Tasks;
using Features.NetworkedModelCodegen.Scripts;
using Features.NetworkedModelRuntime;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionModelScopeActivator : IInitializable
	{
		private readonly INetworkedModelScopeController _networkedModelScopeController;

		public SessionModelScopeActivator(INetworkedModelScopeController networkedModelScopeController)
		{
			_networkedModelScopeController = networkedModelScopeController;
		}

		public void Initialize()
		{
			OpenSessionScopeAsync().Forget();
		}

		private async UniTaskVoid OpenSessionScopeAsync()
		{
			await _networkedModelScopeController.OpenGlobalScopeAsync(ModelScope.Session);
			await _networkedModelScopeController.OpenLocalScopeAsync(ModelScope.Session);
		}
	}
}
