using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkTelemetry.Scripts.Internal;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryMediator : ITickable
	{
		private readonly TelemetryService _service;

		private MultiplayerModel _multiplayerModel;

		private PlayerMovableModel _playerMovableModel;

		private EnemyTransformsModel _enemyTransformsModel;

		private TelemetryTrackablesModel _trackablesModel;

		private float _positionSampleTimer;

		private float _enemyPositionSampleTimer;

		private float _grabObjectPositionSampleTimer;

		private float _fusionTelemetryTimer;

		private long _lastPhotonBytesIn = -1L;

		private long _lastPhotonBytesOut = -1L;

		private readonly FrameTiming[] _frameTimingScratch = new FrameTiming[1];

		public TelemetryMediator(TelemetryService service)
		{
			_service = service;
		}

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel, EnemyTransformsModel enemyTransformsModel, TelemetryTrackablesModel trackablesModel)
		{
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_enemyTransformsModel = enemyTransformsModel;
			_trackablesModel = trackablesModel;
		}

		public void Tick()
		{
			if (!_service.Configuration.IsEnabled)
			{
				return;
			}
			TelemetryConfiguration configuration = _service.Configuration;
			if (configuration.CollectPositions && configuration.PositionSampleRateHz > 0f)
			{
				_positionSampleTimer += Time.deltaTime;
				if (_positionSampleTimer >= 1f / configuration.PositionSampleRateHz)
				{
					_positionSampleTimer = 0f;
					SampleLocalPosition();
				}
			}
			if (_service.IsTelemetryActiveForGameSampling && configuration.CollectEnemyPositions && configuration.EnemyPositionSampleRateHz > 0f)
			{
				NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
				if (networkRunner != null && networkRunner.IsRunning)
				{
					_enemyPositionSampleTimer += Time.deltaTime;
					if (_enemyPositionSampleTimer >= 1f / configuration.EnemyPositionSampleRateHz)
					{
						_enemyPositionSampleTimer = 0f;
						SampleAuthorityEnemies();
					}
				}
			}
			if (_service.IsTelemetryActiveForGameSampling && configuration.CollectGrabObjectPositions && configuration.GrabObjectPositionSampleRateHz > 0f)
			{
				_grabObjectPositionSampleTimer += Time.deltaTime;
				if (_grabObjectPositionSampleTimer >= 1f / configuration.GrabObjectPositionSampleRateHz)
				{
					_grabObjectPositionSampleTimer = 0f;
					SampleTrackables();
				}
			}
			if (configuration.CollectFusionTelemetry || configuration.CollectPhotonTransport)
			{
				_fusionTelemetryTimer += Time.deltaTime;
				if (_fusionTelemetryTimer >= 1f)
				{
					_fusionTelemetryTimer = 0f;
					TrySampleFusionTelemetry();
				}
			}
			bool flag = configuration.CollectNetworkMetrics || configuration.CollectFrameTimings;
			if (_service.IsNetworkSessionPhase && flag)
			{
				NetworkRunner networkRunner2 = _multiplayerModel?.NetworkRunner;
				if (networkRunner2 != null && networkRunner2.IsRunning)
				{
					SamplePerFrameSessionRow(networkRunner2);
				}
			}
		}

		private void SamplePerFrameSessionRow(NetworkRunner runner)
		{
			TelemetryConfiguration configuration = _service.Configuration;
			int sessionTimeMs = _service.CurrentSessionTimeMs();
			int pingMs = 0;
			if (configuration.CollectNetworkMetrics)
			{
				pingMs = (int)Math.Round((double)(float)runner.GetPlayerRtt(runner.LocalPlayer) * 1000.0);
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float fps = (((configuration.CollectNetworkMetrics || configuration.CollectFrameTimings) && unscaledDeltaTime > 0f) ? (1f / unscaledDeltaTime) : 0f);
			double gpuFrameTimeMs = 0.0;
			double cpuMainThreadFrameTimeMs = 0.0;
			double cpuRenderThreadFrameTimeMs = 0.0;
			double cpuMainThreadPresentWaitTimeMs = 0.0;
			if (configuration.CollectFrameTimings && FrameTimingManager.GetLatestTimings(1u, _frameTimingScratch) != 0)
			{
				FrameTiming frameTiming = _frameTimingScratch[0];
				gpuFrameTimeMs = frameTiming.gpuFrameTime;
				cpuMainThreadFrameTimeMs = frameTiming.cpuMainThreadFrameTime;
				cpuRenderThreadFrameTimeMs = frameTiming.cpuRenderThreadFrameTime;
				cpuMainThreadPresentWaitTimeMs = frameTiming.cpuMainThreadPresentWaitTime;
			}
			ProcessCpuAndRamSampler.Read(out var normalizedCpuPercentOfAllLogicalProcessors, out var workingSetBytes);
			_service.SubmitFrameTimingSample(new FrameTimingSample
			{
				SessionTimeMs = sessionTimeMs,
				PingMs = pingMs,
				Fps = fps,
				GpuFrameTimeMs = gpuFrameTimeMs,
				CpuMainThreadFrameTimeMs = cpuMainThreadFrameTimeMs,
				CpuRenderThreadFrameTimeMs = cpuRenderThreadFrameTimeMs,
				CpuMainThreadPresentWaitTimeMs = cpuMainThreadPresentWaitTimeMs,
				ProcessCpuUsagePct = normalizedCpuPercentOfAllLogicalProcessors,
				ProcessRamUsageBytes = workingSetBytes
			});
		}

		private void SampleLocalPosition()
		{
			PlayerCharacterMovableBase playerCharacterMovableBase = _playerMovableModel?.LocalMovable;
			if (!(playerCharacterMovableBase == null) && !(playerCharacterMovableBase.RotatoblePart == null))
			{
				Vector3 position = playerCharacterMovableBase.transform.position;
				_service.SubmitPositionSample(new PositionSample
				{
					SessionTimeMs = _service.CurrentSessionTimeMs(),
					X = position.x,
					Y = position.y,
					Z = position.z,
					Yaw = playerCharacterMovableBase.RotatoblePart.eulerAngles.y
				});
			}
		}

		private void SampleAuthorityEnemies()
		{
			if (_enemyTransformsModel == null)
			{
				return;
			}
			int sessionTimeMs = _service.CurrentSessionTimeMs();
			foreach (KeyValuePair<EnemyType, List<NetworkObject>> enemyNetworkObject in _enemyTransformsModel.EnemyNetworkObjects)
			{
				string enemyKind = enemyNetworkObject.Key.ToString();
				foreach (NetworkObject item in enemyNetworkObject.Value)
				{
					try
					{
						if (!(item == null) && item.IsValid && item.HasStateAuthority)
						{
							Transform transform = item.transform;
							Vector3 position = transform.position;
							int currentState = 0;
							if (item.TryGetComponent<IEnemyStateProvider>(out var component))
							{
								currentState = (int)component.CurrentState;
							}
							_service.SubmitEnemyPositionSample(new EnemyPositionSample
							{
								SessionTimeMs = sessionTimeMs,
								EnemyKind = enemyKind,
								NetworkObjectId = item.Id.ToString(),
								X = position.x,
								Y = position.y,
								Z = position.z,
								Yaw = transform.eulerAngles.y,
								CurrentState = currentState
							});
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private void SampleTrackables()
		{
			if (_trackablesModel == null)
			{
				return;
			}
			int sessionTimeMs = _service.CurrentSessionTimeMs();
			foreach (TelemetryTrackable trackable in _trackablesModel.Trackables)
			{
				try
				{
					if (!(trackable == null) && trackable.isActiveAndEnabled)
					{
						NetworkObject networkObject = trackable.NetworkObject;
						if (!(networkObject == null) && networkObject.IsValid)
						{
							Transform transform = trackable.transform;
							Vector3 position = transform.position;
							_service.SubmitGrabObjectPositionSample(new GrabObjectPositionSample
							{
								SessionTimeMs = sessionTimeMs,
								GrabObjectType = trackable.TrackableType,
								NetworkObjectId = networkObject.Id.ToString(),
								X = position.x,
								Y = position.y,
								Z = position.z,
								Yaw = transform.eulerAngles.y,
								IsAuthority = networkObject.HasStateAuthority
							});
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		private void TrySampleFusionTelemetry()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				_lastPhotonBytesIn = -1L;
				_lastPhotonBytesOut = -1L;
				return;
			}
			int sessionTimeMs = _service.CurrentSessionTimeMs();
			FusionTelemetryStatSample sample = new FusionTelemetryStatSample
			{
				SessionTimeMs = sessionTimeMs
			};
			if (_service.Configuration.CollectFusionTelemetry)
			{
				sample.LocalRttSeconds = (float)networkRunner.GetPlayerRtt(networkRunner.LocalPlayer);
				sample.CloudRttSeconds = (float)networkRunner.GetRttToPhotonCloud().average;
				try
				{
					int num = 0;
					foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
					{
						if (num >= 4)
						{
							break;
						}
						if (networkRunner.IsPlayerValid(activePlayer))
						{
							float num2 = (float)networkRunner.GetPlayerRtt(activePlayer);
							int playerId = activePlayer.PlayerId;
							switch (num)
							{
							case 0:
								sample.Player1Id = playerId;
								sample.Player1Rtt = num2;
								break;
							case 1:
								sample.Player2Id = playerId;
								sample.Player2Rtt = num2;
								break;
							case 2:
								sample.Player3Id = playerId;
								sample.Player3Rtt = num2;
								break;
							case 3:
								sample.Player4Id = playerId;
								sample.Player4Rtt = num2;
								break;
							}
							num++;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			if (_service.Configuration.CollectPhotonTransport && PhotonRealtimePeerLocator.TryGetPhotonPeer(networkRunner, out var peer))
			{
				long bytesIn = peer.BytesIn;
				long bytesOut = peer.BytesOut;
				long photonBytesInDelta = ((_lastPhotonBytesIn >= 0) ? (bytesIn - _lastPhotonBytesIn) : 0);
				long photonBytesOutDelta = ((_lastPhotonBytesOut >= 0) ? (bytesOut - _lastPhotonBytesOut) : 0);
				_lastPhotonBytesIn = bytesIn;
				_lastPhotonBytesOut = bytesOut;
				try
				{
					sample.PhotonBytesInTotal = bytesIn;
					sample.PhotonBytesOutTotal = bytesOut;
					sample.PhotonBytesInDelta = photonBytesInDelta;
					sample.PhotonBytesOutDelta = photonBytesOutDelta;
					sample.PhotonPeerRttMs = peer.RoundTripTime;
					sample.PhotonPacketLossByCrc = peer.PacketLossByCrc;
					sample.PhotonResentReliableCommands = peer.ResentReliableCommands;
				}
				catch (Exception)
				{
				}
			}
			_service.SubmitFusionSample(sample);
		}
	}
}
