using System.Collections.Generic;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems
{
	public class MatchmakingSessionStartAnalyticsSystem : IMatchmakingSessionStartedAnalytics
	{
		private const int MaxRandomsCount = 3;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly PlayerJoinSourcesModel _playerJoinSourcesModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IMultiplayerService _multiplayerService;

		public MatchmakingSessionStartAnalyticsSystem(GameAnalyticsEventSendService gameAnalyticsEventSendService, PlayerJoinSourcesModel playerJoinSourcesModel, MultiplayerModel multiplayerModel, IMultiplayerService multiplayerService)
		{
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_playerJoinSourcesModel = playerJoinSourcesModel;
			_multiplayerModel = multiplayerModel;
			_multiplayerService = multiplayerService;
		}

		public void ReportRandoms()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (!(activePlayer == networkRunner.LocalPlayer))
				{
					list.Add(activePlayer.PlayerId);
				}
			}
			int randomsCount = Mathf.Clamp(_playerJoinSourcesModel.CountMatchmakingAmong(list), 0, 3);
			bool value;
			bool isPublic = _multiplayerService.TryGetSessionProperty<bool>(networkRunner.SessionInfo, SessionPropertyType.IsPublic, out value) && value;
			_gameAnalyticsEventSendService.TrackMatchmakingSessionStarted(randomsCount, isPublic);
		}
	}
}
