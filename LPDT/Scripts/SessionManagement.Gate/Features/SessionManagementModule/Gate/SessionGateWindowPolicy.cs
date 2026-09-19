using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.SessionManagementModule.Gate
{
	public sealed class SessionGateWindowPolicy : ISessionGateWindowPolicy
	{
		private const int GENERIC_GATE_WINDOW_SECONDS = 15;

		private readonly MultiplayerModel _multiplayerModel;

		public int GenericWindowTicks => 15 * _multiplayerModel.NetworkRunner.TickRate;

		public SessionGateWindowPolicy(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}
	}
}
