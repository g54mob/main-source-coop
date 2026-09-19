using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(6)]
	public class AerohockeyBeachInteractableBehaviour : NetworkBehaviour
	{
		private enum AerohockeySoundKind : byte
		{
			Paddle = 0,
			Wall = 1,
			Goal = 2
		}

		private const int NO_HOLDER = -1;

		[SerializeField]
		private Transform _playfield;

		[SerializeField]
		private AerohockeyPaddle _blackPaddle;

		[SerializeField]
		private AerohockeyPaddle _redPaddle;

		[SerializeField]
		private AerohockeyPuck _puck;

		[SerializeField]
		private AerohockeyGoalTrigger _blackGoal;

		[SerializeField]
		private AerohockeyGoalTrigger _redGoal;

		[SerializeField]
		private AerohockeyPaddleZone _blackPaddleZone;

		[SerializeField]
		private AerohockeyPaddleZone _redPaddleZone;

		[SerializeField]
		private Transform _servePoint;

		[SerializeField]
		private float _puckLocalY = 0.05f;

		[Header("Goal VFX (enable on the side that scored)")]
		[SerializeField]
		private GameObject _blackScoreVfx1;

		[SerializeField]
		private GameObject _blackScoreVfx2;

		[SerializeField]
		private GameObject _redScoreVfx1;

		[SerializeField]
		private GameObject _redScoreVfx2;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		private float _puckMaxSpeed = 4f;

		private float _paddleMoveSpeed = 25f;

		private float _puckLinearDamping = 0.65f;

		private float _puckAngularDamping = 1.2f;

		private float _wallRestitution = 0.72f;

		private float _wallTangentRetain = 0.85f;

		private float _paddleHitVelocityScale = 0.85f;

		private float _puckRespawnDelaySeconds = 1.5f;

		private int _scoreToWin;

		private EventReference _paddleHitSound;

		private EventReference _wallHitSound;

		private EventReference _goalSound;

		private bool _playfieldBound;

		private bool _paddleSpawnPosesPublished;

		private float _paddleHitSoundCooldown;

		private float _wallHitSoundCooldown;

		[WeaverGenerated]
		[DefaultForProperty("BlackHolderPlayerId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BlackHolderPlayerId;

		[WeaverGenerated]
		[DefaultForProperty("RedHolderPlayerId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RedHolderPlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BlackScore", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BlackScore;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("RedScore", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RedScore;

		[WeaverGenerated]
		[DefaultForProperty("IsMatchActive", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsMatchActive;

		[WeaverGenerated]
		[DefaultForProperty("PuckRespawnTimer", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TickTimer _PuckRespawnTimer;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int BlackHolderPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.BlackHolderPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.BlackHolderPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int RedHolderPlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.RedHolderPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.RedHolderPlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe int BlackScore
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.BlackScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.BlackScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe int RedScore
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.RedScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[3];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.RedScore. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[3] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe NetworkBool IsMatchActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.IsMatchActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.IsMatchActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		private unsafe TickTimer PuckRespawnTimer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.PuckRespawnTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(TickTimer*)(Ptr + 5);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AerohockeyBeachInteractableBehaviour.PuckRespawnTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(TickTimer*)(Ptr + 5) = value;
			}
		}

		public Transform PlayfieldTransform => _playfield;

		public AerohockeyPuck Puck => _puck;

		[Inject]
		public void InjectDependencies(AerohockeyConfiguration configuration, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
			ApplyConfiguration(configuration);
		}

		private void ApplyConfiguration(AerohockeyConfiguration configuration)
		{
			_puckMaxSpeed = configuration.PuckMaxSpeed;
			_paddleMoveSpeed = configuration.PaddleMoveSpeed;
			_puckLinearDamping = configuration.PuckLinearDamping;
			_puckAngularDamping = configuration.PuckAngularDamping;
			_wallRestitution = configuration.WallRestitution;
			_wallTangentRetain = configuration.WallTangentRetain;
			_paddleHitVelocityScale = configuration.PaddleHitVelocityScale;
			_puckRespawnDelaySeconds = configuration.PuckRespawnDelaySeconds;
			_scoreToWin = configuration.ScoreToWin;
			_paddleHitSound = configuration.PaddleHitSound;
			_wallHitSound = configuration.WallHitSound;
			_goalSound = configuration.GoalSound;
		}

		public override void Spawned()
		{
			base.Spawned();
			BindPlayfieldArt();
			WirePaddles();
			WirePuck();
			if (base.HasStateAuthority)
			{
				BlackHolderPlayerId = -1;
				RedHolderPlayerId = -1;
				BlackScore = 0;
				RedScore = 0;
				IsMatchActive = true;
				PlacePuck();
			}
			DisableAllGoalVfx();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_blackPaddle.DisposeGrabCallbacks();
			_redPaddle.DisposeGrabCallbacks();
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (_paddleHitSoundCooldown > 0f)
			{
				_paddleHitSoundCooldown -= base.Runner.DeltaTime;
			}
			if (_wallHitSoundCooldown > 0f)
			{
				_wallHitSoundCooldown -= base.Runner.DeltaTime;
			}
			TryPublishPaddleSpawnPoses();
			TryRespawnPuckAfterGoal();
			if ((bool)IsMatchActive && _puck != null && _puck.Object != null && _puck.Object.IsValid && _puck.HasStateAuthority)
			{
				_puck.ClampSpeed();
				_puck.SnapToPlayfieldHeight();
			}
		}

		private void TryRespawnPuckAfterGoal()
		{
			if (base.HasStateAuthority && PuckRespawnTimer.IsRunning && PuckRespawnTimer.Expired(base.Runner))
			{
				PuckRespawnTimer = default(TickTimer);
				PlacePuck();
			}
		}

		private void TryPublishPaddleSpawnPoses()
		{
			if (!_paddleSpawnPosesPublished && base.HasStateAuthority && _blackPaddle.PublishSpawnNetworkPose() && _redPaddle.PublishSpawnNetworkPose())
			{
				_paddleSpawnPosesPublished = true;
			}
		}

		public bool TryClaimPaddle(AerohockeySide side, int playerId)
		{
			if (!base.Object.IsValid)
			{
				return false;
			}
			AerohockeyPaddle aerohockeyPaddle = ((side == AerohockeySide.Black) ? _redPaddle : _blackPaddle);
			if (aerohockeyPaddle.SimplePointGrabable.Initialized && aerohockeyPaddle.SimplePointGrabable.GrabbedByPlayers.Contains(playerId))
			{
				return false;
			}
			ClaimPaddleRpc(side, playerId);
			return true;
		}

		public void ReleasePaddleClaim(AerohockeySide side, int playerId)
		{
			if (base.Object.IsValid)
			{
				ReleasePaddleRpc(side, playerId);
			}
		}

		public void ClampPaddleXZ(AerohockeySide side, float localX, float localZ, out float clampedX, out float clampedZ)
		{
			((side == AerohockeySide.Black) ? _blackPaddleZone : _redPaddleZone).ClampPlayfieldLocal(_playfield, localX, localZ, out clampedX, out clampedZ);
		}

		public void NotifyPuckHitPaddle()
		{
			if (!(_paddleHitSoundCooldown > 0f))
			{
				_paddleHitSoundCooldown = 0.08f;
				PlaySoundRpc(AerohockeySoundKind.Paddle);
			}
		}

		public void NotifyPuckHitWall()
		{
			if (!(_wallHitSoundCooldown > 0f))
			{
				_wallHitSoundCooldown = 0.08f;
				PlaySoundRpc(AerohockeySoundKind.Wall);
			}
		}

		public void NotifyGoalScored(AerohockeySide goalSide)
		{
			if (base.Object.IsValid && (bool)IsMatchActive)
			{
				if (base.HasStateAuthority)
				{
					RegisterGoal(goalSide);
				}
				else
				{
					NotifyGoalScoredRpc(goalSide);
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 1699954226u)]
		private void NotifyGoalScoredRpc([RpcPayload(1)] AerohockeySide goalSide)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1699954226u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour::NotifyGoalScoredRpc(Features.AerohockeyBeachInteractableModule.Scripts.AerohockeySide)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(goalSide, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if ((bool)IsMatchActive)
			{
				RegisterGoal(goalSide);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2569140947u)]
		private void ClaimPaddleRpc([RpcPayload(1)] AerohockeySide side, [RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(1);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2569140947u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour::ClaimPaddleRpc(Features.AerohockeyBeachInteractableModule.Scripts.AerohockeySide,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(side, 1);
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (CanPlayerClaim(side, playerId))
			{
				if (side == AerohockeySide.Black)
				{
					BlackHolderPlayerId = playerId;
				}
				else
				{
					RedHolderPlayerId = playerId;
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3262968796u)]
		private void ReleasePaddleRpc([RpcPayload(1)] AerohockeySide side, [RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(1);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3262968796u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour::ReleasePaddleRpc(Features.AerohockeyBeachInteractableModule.Scripts.AerohockeySide,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(side, 1);
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (side == AerohockeySide.Black && BlackHolderPlayerId == playerId)
			{
				BlackHolderPlayerId = -1;
			}
			else if (side == AerohockeySide.Red && RedHolderPlayerId == playerId)
			{
				RedHolderPlayerId = -1;
			}
		}

		private bool CanPlayerClaim(AerohockeySide side, int playerId)
		{
			int num = ((side == AerohockeySide.Black) ? BlackHolderPlayerId : RedHolderPlayerId);
			if (((side == AerohockeySide.Black) ? RedHolderPlayerId : BlackHolderPlayerId) == playerId)
			{
				return false;
			}
			if (num != -1 && num != playerId)
			{
				return false;
			}
			return true;
		}

		private void BindPlayfieldArt()
		{
			if (!_playfieldBound)
			{
				_blackPaddle.CaptureRestPose();
				_redPaddle.CaptureRestPose();
				_puckLocalY = _playfield.InverseTransformPoint(_puck.transform.position).y;
				_playfieldBound = true;
			}
		}

		private void WirePaddles()
		{
			_blackPaddle.BindTable(this, _paddleMoveSpeed);
			_blackPaddle.InitializeGrabCallbacks();
			_redPaddle.BindTable(this, _paddleMoveSpeed);
			_redPaddle.InitializeGrabCallbacks();
		}

		private void WirePuck()
		{
			_puck.Bind(this, _playfield, _puckLocalY, _puckMaxSpeed, _puckLinearDamping, _puckAngularDamping, _wallRestitution, _wallTangentRetain, _paddleHitVelocityScale);
		}

		private void RegisterGoal(AerohockeySide goalSide)
		{
			AerohockeySide aerohockeySide = ((goalSide == AerohockeySide.Black) ? AerohockeySide.Red : AerohockeySide.Black);
			if (aerohockeySide == AerohockeySide.Black)
			{
				BlackScore++;
			}
			else
			{
				RedScore++;
			}
			PlaySoundRpc(AerohockeySoundKind.Goal);
			PlayGoalVfx(aerohockeySide);
			PlayGoalVfxRpc(aerohockeySide);
			if (_scoreToWin > 0 && (BlackScore >= _scoreToWin || RedScore >= _scoreToWin))
			{
				IsMatchActive = false;
			}
			BeginPuckRespawn();
		}

		private void BeginPuckRespawn()
		{
			HidePuck();
			if (_puckRespawnDelaySeconds <= 0f)
			{
				PlacePuck();
			}
			else
			{
				PuckRespawnTimer = TickTimer.CreateFromSeconds(base.Runner, _puckRespawnDelaySeconds);
			}
		}

		private void HidePuck()
		{
			if (!(_puck.Object == null) && _puck.Object.IsValid)
			{
				if (_puck.HasStateAuthority)
				{
					_puck.HideForRespawn();
				}
				else
				{
					_puck.HideForRespawnRpc();
				}
			}
		}

		private void PlacePuck()
		{
			if (!(_puck.Object == null) && _puck.Object.IsValid)
			{
				Vector3 position = _servePoint.position;
				if (_puck.HasStateAuthority)
				{
					_puck.PlaceAt(position);
				}
				else
				{
					_puck.PlaceAtRpc(position);
				}
			}
		}

		private void PlayGoalVfx(AerohockeySide scoredBy)
		{
			if (scoredBy == AerohockeySide.Black)
			{
				RestartVfx(_blackScoreVfx1);
				RestartVfx(_blackScoreVfx2);
			}
			else
			{
				RestartVfx(_redScoreVfx1);
				RestartVfx(_redScoreVfx2);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3932002045u)]
		private void PlayGoalVfxRpc([RpcPayload(1)] AerohockeySide scoredBy)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3932002045u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour::PlayGoalVfxRpc(Features.AerohockeyBeachInteractableModule.Scripts.AerohockeySide)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(scoredBy, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!base.HasStateAuthority)
			{
				PlayGoalVfx(scoredBy);
			}
		}

		private void DisableAllGoalVfx()
		{
			_blackScoreVfx1.SetActive(value: false);
			_blackScoreVfx2.SetActive(value: false);
			_redScoreVfx1.SetActive(value: false);
			_redScoreVfx2.SetActive(value: false);
		}

		private void RestartVfx(GameObject vfxRoot)
		{
			vfxRoot.SetActive(value: false);
			vfxRoot.SetActive(value: true);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2312198718u)]
		private void PlaySoundRpc([RpcPayload(1)] AerohockeySoundKind kind)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2312198718u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour::PlaySoundRpc(Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyBeachInteractableBehaviour/AerohockeySoundKind)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(kind, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			EventReference reference = kind switch
			{
				AerohockeySoundKind.Paddle => _paddleHitSound, 
				AerohockeySoundKind.Wall => _wallHitSound, 
				AerohockeySoundKind.Goal => _goalSound, 
				_ => default(EventReference), 
			};
			if (!reference.IsNull)
			{
				PlayOneShot(reference, new GenericSoundSource(GetEmitterPosition(), GetInstanceID()));
			}
		}

		private void PlayOneShot(EventReference reference, ISoundSource soundSource)
		{
			if (_audioService != null && soundSource != null && !reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private Vector3 GetEmitterPosition()
		{
			if (_soundSourceBehaviour != null && _soundSourceBehaviour.SoundSourceTransform != null)
			{
				return _soundSourceBehaviour.SoundSourceTransform.position;
			}
			if (_playfield != null)
			{
				return _playfield.position;
			}
			return base.transform.position;
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 emitterPosition = GetEmitterPosition();
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = emitterPosition - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			BlackHolderPlayerId = _BlackHolderPlayerId;
			RedHolderPlayerId = _RedHolderPlayerId;
			BlackScore = _BlackScore;
			RedScore = _RedScore;
			IsMatchActive = _IsMatchActive;
			PuckRespawnTimer = _PuckRespawnTimer;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_BlackHolderPlayerId = BlackHolderPlayerId;
			_RedHolderPlayerId = RedHolderPlayerId;
			_BlackScore = BlackScore;
			_RedScore = RedScore;
			_IsMatchActive = IsMatchActive;
			_PuckRespawnTimer = PuckRespawnTimer;
		}

		[NetworkRpcWeavedInvoker(1699954226u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyGoalScoredRpc_0040Invoker1699954226([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out AerohockeySide value, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyBeachInteractableBehaviour)context.TargetBehaviour).NotifyGoalScoredRpc(value);
		}

		[NetworkRpcWeavedInvoker(2569140947u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ClaimPaddleRpc_0040Invoker2569140947([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out AerohockeySide value, 1);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyBeachInteractableBehaviour)context.TargetBehaviour).ClaimPaddleRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(3262968796u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReleasePaddleRpc_0040Invoker3262968796([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out AerohockeySide value, 1);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyBeachInteractableBehaviour)context.TargetBehaviour).ReleasePaddleRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(3932002045u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayGoalVfxRpc_0040Invoker3932002045([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out AerohockeySide value, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyBeachInteractableBehaviour)context.TargetBehaviour).PlayGoalVfxRpc(value);
		}

		[NetworkRpcWeavedInvoker(2312198718u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaySoundRpc_0040Invoker2312198718([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out AerohockeySoundKind value, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyBeachInteractableBehaviour)context.TargetBehaviour).PlaySoundRpc(value);
		}
	}
}
