using Zenject;

namespace Features.NetworkedModelRuntime
{
	public interface INetworkedModelShadowInstaller
	{
		void Install(DiContainer container);
	}
}
