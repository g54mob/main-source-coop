using System;
using System.Collections.Generic;
using Features.PlayerStatesModule.Scripts;

namespace Features.LevelModule.Scripts
{
	public class TeleportationPointsEventClass
	{
		public List<TeleportationPoint> BeachTeleportationPoints = new List<TeleportationPoint>();

		public readonly Dictionary<PlayerState, List<TeleportationPoint>> TeleportByPlayerStatePoints = new Dictionary<PlayerState, List<TeleportationPoint>>();

		public event Action<List<TeleportationPoint>> OnBeachTeleportationRequested;

		public event Action OnPlayerTeleported;

		public event Action OnCancelPendingTeleportsRequested;

		public void InvokeTeleportationToBeach()
		{
			this.OnBeachTeleportationRequested?.Invoke(BeachTeleportationPoints);
		}

		public void RegisterBeachPoints(List<TeleportationPoint> teleportationPoints)
		{
			BeachTeleportationPoints.AddRange(teleportationPoints);
		}

		public void UnregisterBeachPoints(List<TeleportationPoint> teleportationPoints)
		{
			foreach (TeleportationPoint teleportationPoint in teleportationPoints)
			{
				BeachTeleportationPoints.Remove(teleportationPoint);
			}
		}

		public void RegisterPoint(PlayerState playerState, List<TeleportationPoint> teleportationPoints)
		{
			if (TeleportByPlayerStatePoints.TryGetValue(playerState, out var value))
			{
				value.AddRange(teleportationPoints);
				return;
			}
			value = new List<TeleportationPoint>();
			value.AddRange(teleportationPoints);
			TeleportByPlayerStatePoints[playerState] = value;
		}

		public void UnRegisterPoint(PlayerState playerState, List<TeleportationPoint> teleportationPoints)
		{
			if (!TeleportByPlayerStatePoints.TryGetValue(playerState, out var value))
			{
				return;
			}
			foreach (TeleportationPoint teleportationPoint in teleportationPoints)
			{
				value.Remove(teleportationPoint);
			}
		}

		public void InvokeOnPlayerTeleported()
		{
			this.OnPlayerTeleported?.Invoke();
		}

		public void InvokeCancelPendingTeleports()
		{
			this.OnCancelPendingTeleportsRequested?.Invoke();
		}
	}
}
