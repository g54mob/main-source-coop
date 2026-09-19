using Cysharp.Threading.Tasks;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.NetworkedModelRuntime
{
	public interface INetworkedModelScopeController
	{
		UniTask OpenGlobalScopeAsync(ModelScope scope);

		void CloseGlobalScope(ModelScope scope);

		UniTask OpenLocalScopeAsync(ModelScope scope);

		void CloseLocalScope(ModelScope scope);
	}
}
