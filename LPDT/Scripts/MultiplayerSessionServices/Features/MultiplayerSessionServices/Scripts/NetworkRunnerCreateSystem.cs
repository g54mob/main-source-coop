using Zenject;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class NetworkRunnerCreateSystem : IInitializable
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMultiplayerFactory _multiplayerFactory;

		public NetworkRunnerCreateSystem(MultiplayerModel multiplayerModel, IMultiplayerFactory multiplayerFactory)
		{
			_multiplayerModel = multiplayerModel;
			_multiplayerFactory = multiplayerFactory;
		}

		public void Initialize()
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				_multiplayerModel.NetworkRunner = _multiplayerFactory.CreateNetworkRunner();
			}
		}
	}
}
