using System;
using System.Collections.Generic;
using Features.PlayerSpawner.Scripts;

namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IDetectionContext
	{
		List<PlayerDataHolder> DetectedPlayers { get; }

		List<PlayerDataHolder> VisiblePlayers { get; }

		Dictionary<PlayerDataHolder, float> DetectedPlayersDistance { get; }

		Dictionary<PlayerDataHolder, float> DetectedPlayerFirstDetectedTime { get; }

		PlayerDataHolder PriorityPlayer { get; }

		float TargetSearchRange { get; set; }

		float AvailablePointRange { get; set; }

		event Action OnDetectedPlayersChanged;

		event Action OnVisiblePlayersChanged;

		event Action OnPriorityPlayerChanged;

		void SetDetectedPlayers(List<PlayerDataHolder> players);

		void SetVisiblePlayers(List<PlayerDataHolder> players);

		void SetPriorityPlayer(PlayerDataHolder priorityPlayer);

		void SetDetectedPlayerDistance(PlayerDataHolder player, float distance);

		void RemoveDetectedPlayerDistance(PlayerDataHolder player);

		void ClearDetectedPlayersDistance();

		void SetDetectedPlayerFirstSeenTime(PlayerDataHolder player, float time);

		void RemoveDetectedPlayerFirstSeenTime(PlayerDataHolder player);

		void ClearDetectedPlayersFirstSeenTime();
	}
}
