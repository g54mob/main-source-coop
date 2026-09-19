using Fusion;

namespace Features.NetworkedModelRuntime
{
	public interface INetworkedModelShadowBridge
	{
		void BindObject(NetworkObject networkObject);

		void Unbind();
	}
}
