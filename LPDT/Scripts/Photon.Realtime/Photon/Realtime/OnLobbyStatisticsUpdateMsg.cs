using System.Collections.Generic;

namespace Photon.Realtime
{
	public class OnLobbyStatisticsUpdateMsg
	{
		public List<TypedLobbyInfo> lobbyStatistics;

		internal OnLobbyStatisticsUpdateMsg(List<TypedLobbyInfo> lobbyStatistics)
		{
			this.lobbyStatistics = lobbyStatistics;
		}
	}
}
