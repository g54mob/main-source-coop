using System;
using System.Runtime.InteropServices;
using FIMSpace.FProceduralAnimation;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;

namespace NomadDrive.Features.Player.Animation
{
	[DefaultExecutionOrder(200)]
	public class PlayerAnimationsManager : NetworkBehaviour, IPlayerComponent
	{
		private Animator _animator;

		[SyncVar(hook = "OnSyncedAnimStateChanged")]
		private byte _syncedAnimState;

		[SyncVar]
		private float _syncedMoveDirection;

		[SyncVar]
		private float _syncedMoveStrafe;

		[SyncVar]
		private float _syncedSpeed;

		private PlayerAnimationStateMachine _stateMachine;

		private AnimationStateFactory _stateFactory;

		private FirstPersonController _firstPersonController;

		private LegsAnimator _legsAnimator;

		private Renderer _bodyRenderer;

		private bool _isLocalPlayer;

		private PlayerState _lastSyncedState;

		private LegsAnimator.EGlueMode _lastGlueMode;

		private float _animSyncTimer;

		private float _lastSyncedMoveDir;

		private float _lastSyncedMoveStrafe;

		private float _lastSyncedSpeed;

		private bool _lastUseGluing = true;

		private const float AnimSyncInterval = 1f / 15f;

		private const float AnimParamEpsilon = 0.05f;

		private static readonly int PickUpTriggerHash;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__syncedAnimState;

		public int SetupPriority => 20;

		public byte Network_syncedAnimState
		{
			get
			{
				return _syncedAnimState;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedAnimState, 1uL, _Mirror_SyncVarHookDelegate__syncedAnimState);
			}
		}

		public float Network_syncedMoveDirection
		{
			get
			{
				return _syncedMoveDirection;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedMoveDirection, 2uL, null);
			}
		}

		public float Network_syncedMoveStrafe
		{
			get
			{
				return _syncedMoveStrafe;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedMoveStrafe, 4uL, null);
			}
		}

		public float Network_syncedSpeed
		{
			get
			{
				return _syncedSpeed;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedSpeed, 8uL, null);
			}
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			if (isLocalPlayer)
			{
				base.enabled = true;
				if (_animator != null)
				{
					_animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				}
			}
			else if (_animator != null)
			{
				_animator.cullingMode = AnimatorCullingMode.CullCompletely;
			}
		}

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_stateMachine = GetComponent<PlayerAnimationStateMachine>();
			_legsAnimator = GetComponentInChildren<LegsAnimator>();
			_bodyRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
			_firstPersonController = GetComponent<FirstPersonController>();
			_stateFactory = new AnimationStateFactory();
			InitializeStates();
		}

		private void Start()
		{
			PlayerState playerState = (PlayerState)((!_isLocalPlayer && _syncedAnimState != 0) ? _syncedAnimState : 0);
			_stateMachine.ChangeState(playerState, _syncedMoveDirection, _syncedMoveStrafe, _syncedSpeed);
			bool useGluing = (_lastUseGluing = IsGluingActiveState(playerState));
			if (_legsAnimator != null)
			{
				_legsAnimator.UseGluing = useGluing;
			}
		}

		private void InitializeStates()
		{
			foreach (PlayerState value in Enum.GetValues(typeof(PlayerState)))
			{
				_stateMachine.AddState(value, _stateFactory.CreateState(value));
			}
		}

		private void Update()
		{
			if (!base.isClient)
			{
				return;
			}
			if (!_isLocalPlayer)
			{
				PlayerState syncedAnimState = (PlayerState)_syncedAnimState;
				_stateMachine.ChangeState(syncedAnimState, _syncedMoveDirection, _syncedMoveStrafe, _syncedSpeed);
				if (!(_animator != null) || !_animator.enabled || (!(_bodyRenderer == null) && !_bodyRenderer.isVisible))
				{
					return;
				}
				bool flag = IsGluingActiveState(syncedAnimState);
				if (flag != _lastUseGluing)
				{
					_lastUseGluing = flag;
					if (_legsAnimator != null)
					{
						_legsAnimator.UseGluing = flag;
					}
				}
				return;
			}
			PlayerState playerState = _firstPersonController.PlayerState;
			float moveVertical = OnFootInputs.GetMoveVertical();
			float moveHorizontal = OnFootInputs.GetMoveHorizontal();
			Vector3 velocity = _firstPersonController.GetVelocity();
			float magnitude = new Vector3(velocity.x, 0f, velocity.z).magnitude;
			_stateMachine.ChangeState(playerState, moveVertical, moveHorizontal, magnitude);
			LegsAnimator.EGlueMode eGlueMode = (IsMovingOrAirborne(playerState) ? LegsAnimator.EGlueMode.Moving : LegsAnimator.EGlueMode.Idle);
			if (eGlueMode != _lastGlueMode)
			{
				_lastGlueMode = eGlueMode;
				_legsAnimator.GlueMode = eGlueMode;
				CmdSetLegsAnimatorGluingState(eGlueMode);
			}
			bool flag2 = IsGluingActiveState(playerState);
			if (flag2 != _lastUseGluing)
			{
				_lastUseGluing = flag2;
				if (_legsAnimator != null)
				{
					_legsAnimator.UseGluing = flag2;
				}
			}
			if (playerState != _lastSyncedState)
			{
				_lastSyncedState = playerState;
				_lastSyncedMoveDir = moveVertical;
				_lastSyncedMoveStrafe = moveHorizontal;
				_lastSyncedSpeed = magnitude;
				_animSyncTimer = 0f;
				CmdSyncAnimState((byte)playerState, moveVertical, moveHorizontal, magnitude);
				return;
			}
			_animSyncTimer += Time.deltaTime;
			if (!(_animSyncTimer < 1f / 15f) && (Mathf.Abs(moveVertical - _lastSyncedMoveDir) > 0.05f || Mathf.Abs(moveHorizontal - _lastSyncedMoveStrafe) > 0.05f || Mathf.Abs(magnitude - _lastSyncedSpeed) > 0.05f))
			{
				_animSyncTimer = 0f;
				_lastSyncedMoveDir = moveVertical;
				_lastSyncedMoveStrafe = moveHorizontal;
				_lastSyncedSpeed = magnitude;
				CmdSyncAnimParams(moveVertical, moveHorizontal, magnitude);
			}
		}

		private static bool IsMovingOrAirborne(PlayerState state)
		{
			if (state != PlayerState.Walk && state != PlayerState.Sprint && state != PlayerState.CrouchedWalk && state != PlayerState.CrouchedSprint && state != PlayerState.Jump && state != PlayerState.Falling)
			{
				return state == PlayerState.Landing;
			}
			return true;
		}

		private static bool IsGluingActiveState(PlayerState state)
		{
			if (state != PlayerState.Idle)
			{
				return state == PlayerState.CrouchedIdle;
			}
			return true;
		}

		public void TriggerPickUp()
		{
			_animator.SetTrigger(PickUpTriggerHash);
		}

		private void OnSyncedAnimStateChanged(byte oldValue, byte newValue)
		{
			if (!_isLocalPlayer && !(_stateMachine == null))
			{
				_stateMachine.ChangeState((PlayerState)newValue, _syncedMoveDirection, _syncedMoveStrafe, _syncedSpeed);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncAnimState(byte stateId, float moveDirection, float moveStrafe, float speed)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, stateId);
			writer.WriteFloat(moveDirection);
			writer.WriteFloat(moveStrafe);
			writer.WriteFloat(speed);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSyncAnimState(System.Byte,System.Single,System.Single,System.Single)", 1459955322, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncAnimParams(float moveDirection, float moveStrafe, float speed)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(moveDirection);
			writer.WriteFloat(moveStrafe);
			writer.WriteFloat(speed);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSyncAnimParams(System.Single,System.Single,System.Single)", 141852442, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetLegsAnimatorGluingState(LegsAnimator.EGlueMode glueMode)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(writer, glueMode);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSetLegsAnimatorGluingState(FIMSpace.FProceduralAnimation.LegsAnimator/EGlueMode)", 206582447, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcSetLegsAnimatorGluingState(LegsAnimator.EGlueMode glueMode)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(writer, glueMode);
			SendRPCInternal("System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::RpcSetLegsAnimatorGluingState(FIMSpace.FProceduralAnimation.LegsAnimator/EGlueMode)", -1608679940, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		public PlayerAnimationsManager()
		{
			_Mirror_SyncVarHookDelegate__syncedAnimState = OnSyncedAnimStateChanged;
		}

		static PlayerAnimationsManager()
		{
			PickUpTriggerHash = Animator.StringToHash("PickUpTrigger");
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerAnimationsManager), "System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSyncAnimState(System.Byte,System.Single,System.Single,System.Single)", InvokeUserCode_CmdSyncAnimState__Byte__Single__Single__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerAnimationsManager), "System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSyncAnimParams(System.Single,System.Single,System.Single)", InvokeUserCode_CmdSyncAnimParams__Single__Single__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerAnimationsManager), "System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::CmdSetLegsAnimatorGluingState(FIMSpace.FProceduralAnimation.LegsAnimator/EGlueMode)", InvokeUserCode_CmdSetLegsAnimatorGluingState__EGlueMode, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerAnimationsManager), "System.Void NomadDrive.Features.Player.Animation.PlayerAnimationsManager::RpcSetLegsAnimatorGluingState(FIMSpace.FProceduralAnimation.LegsAnimator/EGlueMode)", InvokeUserCode_RpcSetLegsAnimatorGluingState__EGlueMode);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSyncAnimState__Byte__Single__Single__Single(byte stateId, float moveDirection, float moveStrafe, float speed)
		{
			Network_syncedAnimState = stateId;
			Network_syncedMoveDirection = moveDirection;
			Network_syncedMoveStrafe = moveStrafe;
			Network_syncedSpeed = speed;
		}

		protected static void InvokeUserCode_CmdSyncAnimState__Byte__Single__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncAnimState called on client.");
			}
			else
			{
				((PlayerAnimationsManager)obj).UserCode_CmdSyncAnimState__Byte__Single__Single__Single(NetworkReaderExtensions.ReadByte(reader), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSyncAnimParams__Single__Single__Single(float moveDirection, float moveStrafe, float speed)
		{
			Network_syncedMoveDirection = moveDirection;
			Network_syncedMoveStrafe = moveStrafe;
			Network_syncedSpeed = speed;
		}

		protected static void InvokeUserCode_CmdSyncAnimParams__Single__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncAnimParams called on client.");
			}
			else
			{
				((PlayerAnimationsManager)obj).UserCode_CmdSyncAnimParams__Single__Single__Single(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSetLegsAnimatorGluingState__EGlueMode(LegsAnimator.EGlueMode glueMode)
		{
			RpcSetLegsAnimatorGluingState(glueMode);
		}

		protected static void InvokeUserCode_CmdSetLegsAnimatorGluingState__EGlueMode(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetLegsAnimatorGluingState called on client.");
			}
			else
			{
				((PlayerAnimationsManager)obj).UserCode_CmdSetLegsAnimatorGluingState__EGlueMode(GeneratedNetworkCode._Read_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(reader));
			}
		}

		protected void UserCode_RpcSetLegsAnimatorGluingState__EGlueMode(LegsAnimator.EGlueMode glueMode)
		{
			_legsAnimator.GlueMode = glueMode;
		}

		protected static void InvokeUserCode_RpcSetLegsAnimatorGluingState__EGlueMode(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetLegsAnimatorGluingState called on server.");
			}
			else
			{
				((PlayerAnimationsManager)obj).UserCode_RpcSetLegsAnimatorGluingState__EGlueMode(GeneratedNetworkCode._Read_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(reader));
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _syncedAnimState);
				writer.WriteFloat(_syncedMoveDirection);
				writer.WriteFloat(_syncedMoveStrafe);
				writer.WriteFloat(_syncedSpeed);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _syncedAnimState);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteFloat(_syncedMoveDirection);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteFloat(_syncedMoveStrafe);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteFloat(_syncedSpeed);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedAnimState, _Mirror_SyncVarHookDelegate__syncedAnimState, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _syncedMoveDirection, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedMoveStrafe, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedSpeed, null, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedAnimState, _Mirror_SyncVarHookDelegate__syncedAnimState, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedMoveDirection, null, reader.ReadFloat());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedMoveStrafe, null, reader.ReadFloat());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedSpeed, null, reader.ReadFloat());
			}
		}
	}
}
