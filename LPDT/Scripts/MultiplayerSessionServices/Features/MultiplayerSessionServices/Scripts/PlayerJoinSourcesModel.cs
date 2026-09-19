using System.Collections.Generic;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.MultiplayerSessionServices.Scripts
{
	[NetworkedModel(ModelScope.Session, ModelOwnership.Shared)]
	public sealed class PlayerJoinSourcesModel : NetworkedModelBase
	{
		private const int PlayerIdShift = 8;

		private const int JoinSourceMask = 255;

		[NetworkedCapacity(4)]
		public NetworkedDictionary<int, JoinSource> Sources { get; } = new NetworkedDictionary<int, JoinSource>();

		public NetworkedSignal<int> ReportSignal { get; } = new NetworkedSignal<int>();

		public PlayerJoinSourcesModel()
		{
			ReportSignal.Received += OnReportReceived;
		}

		public void Report(int playerId, JoinSource joinSource)
		{
			if (base.IsAuthority)
			{
				Sources.Set(playerId, joinSource);
			}
			else
			{
				ReportSignal.Raise(Pack(playerId, joinSource));
			}
		}

		public void Remove(int playerId)
		{
			if (base.IsAuthority)
			{
				Sources.Remove(playerId);
			}
		}

		public void ClearForLobbyEnter()
		{
			if (base.IsAuthority)
			{
				Sources.Clear();
			}
		}

		public bool TryGet(int playerId, out JoinSource joinSource)
		{
			return Sources.TryGetValue(playerId, out joinSource);
		}

		public int CountMatchmakingAmong(IEnumerable<int> playerIds)
		{
			int num = 0;
			foreach (int playerId in playerIds)
			{
				if (Sources.TryGetValue(playerId, out var value) && value == JoinSource.Matchmaking)
				{
					num++;
				}
			}
			return num;
		}

		private void OnReportReceived(int packed)
		{
			Unpack(packed, out var playerId, out var joinSource);
			Sources.Set(playerId, joinSource);
		}

		private static int Pack(int playerId, JoinSource joinSource)
		{
			return (playerId << 8) | (int)(joinSource & (JoinSource)255);
		}

		private static void Unpack(int packed, out int playerId, out JoinSource joinSource)
		{
			playerId = packed >> 8;
			joinSource = (JoinSource)(packed & 0xFF);
		}
	}
}
