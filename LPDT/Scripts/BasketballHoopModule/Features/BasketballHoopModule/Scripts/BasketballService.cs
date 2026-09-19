using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.TeleportModule.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BasketballHoopModule.Scripts
{
	public sealed class BasketballService : IBasketballService, ITickable
	{
		private sealed class BallSession
		{
			public NetworkObject Ball;

			public Vector3 SpawnPosition;

			public Vector3 DistanceTrackPosition;

			public BasketballBallTeleport Teleport;

			public ITeleportable Teleportable;

			public SimplePointGrabable Grabable;

			public float ResetTimerRemaining = -1f;

			public Action GrabbedPlayersChangedHandler;
		}

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ITeleportService _teleportService;

		private readonly BasketballConfiguration _configuration;

		private readonly Dictionary<NetworkId, BallSession> _sessions = new Dictionary<NetworkId, BallSession>();

		public BasketballService(MultiplayerModel multiplayerModel, ITeleportService teleportService, BasketballConfiguration configuration)
		{
			_multiplayerModel = multiplayerModel;
			_teleportService = teleportService;
			_configuration = configuration;
		}

		public void RegisterBall(NetworkObject ball, Vector3 spawnPosition, Vector3 trackPosition, BasketballBallTeleport teleport)
		{
			if (ball == null || teleport == null)
			{
				return;
			}
			NetworkId id = ball.Id;
			if (_sessions.TryGetValue(id, out var value))
			{
				value.SpawnPosition = spawnPosition;
				value.DistanceTrackPosition = trackPosition;
				value.Teleport = teleport;
				value.Teleportable = teleport;
				return;
			}
			ball.TryGetComponent<SimplePointGrabable>(out var component);
			BallSession ballSession = new BallSession
			{
				Ball = ball,
				SpawnPosition = spawnPosition,
				DistanceTrackPosition = trackPosition,
				Teleport = teleport,
				Teleportable = teleport,
				Grabable = component
			};
			if (component != null)
			{
				ballSession.GrabbedPlayersChangedHandler = delegate
				{
					OnGrabbedPlayersChanged(id);
				};
				component.OnGrabbedPlayersChanged += ballSession.GrabbedPlayersChangedHandler;
			}
			teleport.OnDespawned += UnregisterBall;
			_sessions[id] = ballSession;
		}

		public void UnregisterBall(NetworkObject ball)
		{
			if (!(ball == null) && _sessions.TryGetValue(ball.Id, out var value))
			{
				if (value.Grabable != null && value.GrabbedPlayersChangedHandler != null)
				{
					value.Grabable.OnGrabbedPlayersChanged -= value.GrabbedPlayersChangedHandler;
				}
				if (value.Teleport != null)
				{
					value.Teleport.OnDespawned -= UnregisterBall;
				}
				_sessions.Remove(ball.Id);
			}
		}

		public void Tick()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			foreach (BallSession value in _sessions.Values)
			{
				TickSession(value);
			}
		}

		private void TickSession(BallSession session)
		{
			if (session.Ball == null || !session.Ball.IsValid || !session.Ball.HasStateAuthority)
			{
				return;
			}
			if (IsGrabbed(session))
			{
				session.ResetTimerRemaining = -1f;
				return;
			}
			if (Vector3.Distance(session.Ball.transform.position, session.DistanceTrackPosition) <= _configuration.MaxDistanceFromSpawnBeforeReset)
			{
				session.ResetTimerRemaining = -1f;
				return;
			}
			if (session.ResetTimerRemaining < 0f)
			{
				session.ResetTimerRemaining = _configuration.OutOfZoneResetDelaySeconds;
				return;
			}
			session.ResetTimerRemaining -= Time.deltaTime;
			if (!(session.ResetTimerRemaining > 0f))
			{
				session.ResetTimerRemaining = -1f;
				TeleportToSpawnAsync(session).Forget();
			}
		}

		private void OnGrabbedPlayersChanged(NetworkId ballId)
		{
			if (_sessions.TryGetValue(ballId, out var value) && value.Ball.HasStateAuthority)
			{
				if (IsGrabbed(value))
				{
					value.ResetTimerRemaining = -1f;
				}
				else if (Vector3.Distance(value.Ball.transform.position, value.DistanceTrackPosition) > _configuration.MaxDistanceFromSpawnBeforeReset)
				{
					value.ResetTimerRemaining = _configuration.OutOfZoneResetDelaySeconds;
				}
			}
		}

		private static bool IsGrabbed(BallSession session)
		{
			if (session.Grabable != null)
			{
				return session.Grabable.GrabbedByPlayers.Count > 0;
			}
			return false;
		}

		private async UniTaskVoid TeleportToSpawnAsync(BallSession session)
		{
			if (!(session.Ball == null) && session.Ball.IsValid && session.Teleportable != null)
			{
				await _teleportService.TeleportObject(session.Teleportable, session.SpawnPosition);
			}
		}
	}
}
