using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.PhysicsVolumeModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StrechArmsModule.Scripts.RotationaAxis;
using Fusion;
using Obi;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(7)]
	public class ArmVisualsController : NetworkBehaviour
	{
		[Header("References")]
		[SerializeField]
		private RopeLengthController _ropeLengthController;

		[SerializeField]
		private Transform _armEndForRope;

		[SerializeField]
		private Transform _armEndSecondForRope;

		[SerializeField]
		private Transform _armStartForRope;

		[SerializeField]
		private Transform _shoulderTransform;

		[Header("Obi Configuration")]
		[SerializeField]
		private SnapAttachment _startArmSnapAttachment;

		[SerializeField]
		private SnapAttachment _startArmSecondSnapAttachment;

		[SerializeField]
		private SnapAttachment _endArmSnapAttachment;

		[SerializeField]
		private SnapAttachment _secondEndArmSnapAttachment;

		[SerializeField]
		private ObiParticleAttachment _particleStartAttachment;

		[SerializeField]
		private ObiParticleAttachment _particleStartSecondAttachment;

		[SerializeField]
		private ObiParticleAttachment _particleEndAttachment;

		[SerializeField]
		private ObiParticleAttachment _particleSecondEndAttachment;

		[SerializeField]
		private ObiRopeExtrudedRenderer _ropeExtrudedRenderer;

		[SerializeField]
		private ObiSolver _obiSolver;

		[SerializeField]
		private ObiArmResolutionConfiguration _obiArmResolutionConfiguration;

		[Header("Rope length configuration")]
		[SerializeField]
		private float _stretchLength = 3f;

		[SerializeField]
		private float _additionalLengthWehDraggingObjects = 2f;

		[SerializeField]
		private float _additionalLengthWehGrabbedByPlayers = 1f;

		[SerializeField]
		private float _distanceErrorOnRemovingLength = 0.5f;

		[SerializeField]
		private float _distanceErrorOnRemovingLengthIdle = 1f;

		[SerializeField]
		private float _maxRopeLength = 4f;

		[SerializeField]
		private float _maxRopeLengthIdle = 0.7f;

		[Header("ArmEndConfiguration")]
		[SerializeField]
		private Axis _sourceAxis;

		[SerializeField]
		private Axis _targetAxis;

		[SerializeField]
		private float _storeArmMinRangeNoGrabbing;

		[SerializeField]
		private float _storeArmRangeNoGrabbing = 1f;

		[SerializeField]
		private float _ragdollReturnDuration = 0.35f;

		[SerializeField]
		private Material _authorativeArmsMaterial;

		[SerializeField]
		private Material _nonAuthorativeArmsMaterial;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ArmOrientation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Arm _ArmOrientation;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TargetOwnerNetworkID", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _TargetOwnerNetworkID;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TargetHandleIndex", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _TargetHandleIndex;

		[WeaverGenerated]
		[DefaultForProperty("InIdlePose", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _InIdlePose;

		[WeaverGenerated]
		[DefaultForProperty("LineArmType", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LineArmType _LineArmType;

		[WeaverGenerated]
		[DefaultForProperty("ArmEndGrabbableNetworkId", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _ArmEndGrabbableNetworkId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ReduceSolverUpdateEnabled", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _ReduceSolverUpdateEnabled = true;

		private Transform _armEnd;

		private Transform _armStart;

		private Transform _armEndIdlePose;

		private Transform _armEndStoreIdlePose;

		private LineArmControllerBase _lineArmController;

		private IPointGrabable _armEndGrabbable;

		private Transform _currentHandle;

		private bool _armStartSet;

		private bool _armEndSet;

		private bool _armEndIdlePoseSet;

		private bool _armEndStoreIdlePoseSet;

		private bool _handsProcessingStarted;

		private NetworkId _settedTargetOwnerNetworkID;

		private int _settedTargetHandleIndex = int.MinValue;

		private Transform _targetTransform;

		private ArmEndController _armEndController;

		private ArmStartsModel _armStartsModel;

		private PlayerMovableModel _playerMovableModel;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerRagdollEntity _playerRagdollEntity;

		private CancellationTokenSource _renderStampCts;

		private bool _isArmControlledByPhysics;

		private PlayerCharacterMovableBase _playerCharacterMovableBase;

		private IPlayerStateService _playerStateService;

		private LineArmsModel _lineArmsModel;

		private PlayersArmsModel _playersArmsModel;

		private ArmVisualsControllerModel _armVisualsControllerModel;

		private int _registeredPlayerId = -1;

		private Arm _registeredArmOrientation;

		private Transform _cachedTargetTransform;

		private Transform _carrierVolumeTargetRef;

		private PhysicsInfluenceVolume _targetCarrierVolume;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private bool _isReturningFromRagdoll;

		private float _ragdollReturnElapsed;

		private Vector3 _ragdollReturnStartPosition;

		private LineArmType _prevLineArmType;

		private NetworkId _prevArmEndGrabbableNetworkId = NetworkId.None;

		private bool _prevReduceSolverUpdateEnabled = true;

		[field: SerializeField]
		public StaticColorChangerByPlayerRef ColorChanger { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe Arm ArmOrientation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Arm*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Arm*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe NetworkId TargetOwnerNetworkID
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.TargetOwnerNetworkID. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.TargetOwnerNetworkID. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe int TargetHandleIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.TargetHandleIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.TargetHandleIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe bool InIdlePose
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.InIdlePose. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.InIdlePose. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe LineArmType LineArmType
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.LineArmType. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (LineArmType)Ptr[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.LineArmType. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = (int)value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		private unsafe NetworkId ArmEndGrabbableNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ArmEndGrabbableNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 5);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ArmEndGrabbableNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 5) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		public unsafe bool ReduceSolverUpdateEnabled
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ReduceSolverUpdateEnabled. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ArmVisualsController.ReduceSolverUpdateEnabled. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 6) = new NetworkBool(value);
			}
		}

		public Transform RopeShoulderEndpoint => _armStartForRope;

		public Transform ShoulderAnchor => _armStart;

		[Inject]
		public void InjectDependencies(ArmStartsModel armStartsModel, PlayerMovableModel playerMovableModel, PlayersRagdollModel playersRagdollModel, IPlayerStateService playerStateService, LineArmsModel lineArmsModel, PlayersStatesSynchronizer playersStatesSynchronizer, PlayersArmsModel playersArmsModel, ArmVisualsControllerModel armVisualsControllerModel)
		{
			_armStartsModel = armStartsModel;
			_armVisualsControllerModel = armVisualsControllerModel;
			_playerMovableModel = playerMovableModel;
			_playersRagdollModel = playersRagdollModel;
			_playerStateService = playerStateService;
			_lineArmsModel = lineArmsModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playersArmsModel = playersArmsModel;
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (!(_lineArmController == null))
			{
				ProcessHandsState();
			}
		}

		public override void Render()
		{
			if (_lineArmController == null || _prevLineArmType != LineArmType)
			{
				SetLineArmInternal();
			}
			_prevLineArmType = LineArmType;
			if (_armEndGrabbable == null || _prevArmEndGrabbableNetworkId != ArmEndGrabbableNetworkId)
			{
				SetArmEndGrabbableInternal();
			}
			_prevArmEndGrabbableNetworkId = ArmEndGrabbableNetworkId;
			if (_prevReduceSolverUpdateEnabled != ReduceSolverUpdateEnabled && !ReduceSolverUpdateEnabled)
			{
				_obiSolver.substeps = GetAuthorityResolutionConfiguration().HighTier.Substeps;
			}
			_prevReduceSolverUpdateEnabled = ReduceSolverUpdateEnabled;
		}

		private void ProcessHandsState()
		{
			if (!_armEndSet || !_armStartSet || !_armEndIdlePoseSet)
			{
				return;
			}
			if (!_handsProcessingStarted)
			{
				if (!_playerMovableModel.AllCharacterMovables.ContainsKey(base.Object.StateAuthority))
				{
					return;
				}
				_handsProcessingStarted = true;
			}
			if (_armEndController != null && _armEndController.IsInitialized)
			{
				_armEndController.IsGrabbed = !InIdlePose;
			}
			if (_lineArmController.CurrentGrabbables.Count == 0 && _playerStateService.GetPlayerState(base.Object.InputAuthority.PlayerId) != PlayerState.Store)
			{
				if (!InIdlePose)
				{
					_currentHandle = null;
					NetworkObject networkObject = ((_armEndIdlePose != null) ? _armEndIdlePose.GetComponent<NetworkObject>() : null);
					if (!(networkObject == null))
					{
						SetTargetTransform_RPC(networkObject.Id, -1);
						InIdlePose = true;
					}
				}
			}
			else
			{
				if (ArmOrientation == Arm.Left)
				{
					return;
				}
				InIdlePose = false;
				if (_lineArmController.TryGetCurrentHandle(out var grabbable, out var handle) && !(handle == _currentHandle))
				{
					_currentHandle = handle;
					NetworkObject networkObject2 = grabbable.NetworkObject;
					if (!(networkObject2 == null))
					{
						int handleIndex = grabbable.Handles.IndexOf(handle);
						SetTargetTransform_RPC(networkObject2.Id, handleIndex);
					}
				}
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			RegisterArmVisuals();
			SnapArmStart_RPC();
			SnapArmEnd_RPC();
			_playersStatesSynchronizer.OnSomePlayerStateChanged += ProcessStoreIdlePoseEnter;
			_playersStatesSynchronizer.OnSomePlayerStateExit += ProcessStoreIdlePoseExit;
			if (base.HasStateAuthority)
			{
				if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Object.StateAuthority, out var value))
				{
					BindCharacterMovable(value);
				}
				else
				{
					_playerMovableModel.AllCharacterMovablesPlayerAdded += OnCharacterMovableAdded;
				}
			}
			if (_playersArmsModel.PlayerObiArms.TryGetValue(base.Object.InputAuthority.PlayerId, out var value2))
			{
				InitializeObiParent(base.Object.InputAuthority.PlayerId, value2);
			}
			else
			{
				_playersArmsModel.OnPlayerObiArmAdded += InitializeObiParent;
			}
			if (_playersRagdollModel.PlayersRagdoll.TryGetValue(base.Object.StateAuthority.PlayerId, out var value3))
			{
				InitializeRagdoll(base.Object.StateAuthority.PlayerId, value3);
			}
			else
			{
				_playersRagdollModel.OnPlayerRagdollAdded += InitializeRagdoll;
			}
			SwitchPlayerArmsMaterial();
			_renderStampCts?.Cancel();
			_renderStampCts?.Dispose();
			_renderStampCts = new CancellationTokenSource();
			RunRenderStampLoop(_renderStampCts.Token).Forget();
		}

		private async UniTaskVoid RunRenderStampLoop(CancellationToken token)
		{
			while (!token.IsCancellationRequested && !(await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, token).SuppressCancellationThrow()))
			{
				RestampRopeEndpointsToRenderPose();
			}
		}

		private void RestampRopeEndpointsToRenderPose()
		{
			if (_armEndSet && _armStartSet && !(_armStart == null) && !(_armEnd == null) && !(_armStartForRope == null) && !(_armEndForRope == null))
			{
				Vector3 axis = GetAxis(_armStart, _sourceAxis);
				Quaternion quaternion = Quaternion.FromToRotation(GetAxis(_armStartForRope, _targetAxis), axis);
				_armStartForRope.rotation = quaternion * _armStartForRope.rotation;
				_armEndForRope.position = _armEnd.position;
				_armStartForRope.position = _armStart.position;
			}
		}

		private void RegisterArmVisuals()
		{
			_registeredPlayerId = base.Object.InputAuthority.PlayerId;
			_registeredArmOrientation = ArmOrientation;
			_armVisualsControllerModel.RegisterArmVisualsController(_registeredPlayerId, _registeredArmOrientation, this);
		}

		private void InitializeObiParent(int inputAuthorityPlayerId, Transform obiArmTransform)
		{
			if (base.Object.InputAuthority.PlayerId == inputAuthorityPlayerId)
			{
				base.transform.SetParent(obiArmTransform);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_registeredArmOrientation != Arm.None)
			{
				_armVisualsControllerModel.UnregisterArmVisualsController(_registeredPlayerId, _registeredArmOrientation, this);
			}
			_renderStampCts?.Cancel();
			_renderStampCts?.Dispose();
			_renderStampCts = null;
			if (_playersStatesSynchronizer != null)
			{
				_playersStatesSynchronizer.OnSomePlayerStateChanged -= ProcessStoreIdlePoseEnter;
				_playersStatesSynchronizer.OnSomePlayerStateExit -= ProcessStoreIdlePoseExit;
			}
			if (_playerMovableModel != null)
			{
				_playerMovableModel.AllCharacterMovablesPlayerAdded -= OnCharacterMovableAdded;
			}
			if (base.HasStateAuthority && _playerCharacterMovableBase != null && _obiSolver != null)
			{
				_playerCharacterMovableBase.OnChangePosition -= _obiSolver.PushSolverParameters;
			}
			if (_armStartsModel != null)
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmStoreIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Remove(armStartsModel.OnArmStoreIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmStoreIdlePoseAdded));
			}
			if (_playersRagdollModel != null)
			{
				_playersRagdollModel.OnPlayerRagdollAdded -= InitializeRagdoll;
			}
			if (_playerRagdollEntity != null)
			{
				_playerRagdollEntity.OnSimulationStarted -= EnableArmControlledByPhysics;
				_playerRagdollEntity.OnSimulationStopped -= DisableArmControlledByPhysics;
				_playerRagdollEntity.OnSimulateHandsRagdollChanged -= OnHandsRagdollChanged;
			}
			_playersArmsModel.OnPlayerObiArmAdded -= InitializeObiParent;
		}

		private void OnCharacterMovableAdded(PlayerRef playerRef)
		{
			if (!(playerRef != base.Object.StateAuthority))
			{
				_playerMovableModel.AllCharacterMovablesPlayerAdded -= OnCharacterMovableAdded;
				if (_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value))
				{
					BindCharacterMovable(value);
				}
			}
		}

		private void BindCharacterMovable(PlayerCharacterMovableBase characterMovable)
		{
			_playerCharacterMovableBase = characterMovable;
			if (_playerCharacterMovableBase != null)
			{
				_playerCharacterMovableBase.OnChangePosition += _obiSolver.PushSolverParameters;
			}
		}

		private void InitializeRagdoll(int playerId, PlayerRagdollEntity playerRagdollEntity)
		{
			if (base.Object.StateAuthority.PlayerId == playerId)
			{
				_playerRagdollEntity = playerRagdollEntity;
				_playerRagdollEntity.OnSimulationStarted += EnableArmControlledByPhysics;
				_playerRagdollEntity.OnSimulationStopped += DisableArmControlledByPhysics;
				_playerRagdollEntity.OnSimulateHandsRagdollChanged += OnHandsRagdollChanged;
				ReconcileArmControlledByPhysics();
			}
		}

		private void LateUpdate()
		{
			if (_armEndSet && _armStartSet && !(_armStart == null) && !(_armEnd == null) && !(_armStartForRope == null) && !(_armEndForRope == null))
			{
				AdjustLength();
				ReconcileArmControlledByPhysics();
				InterpolateLastArmEndPosition();
				RestampRopeEndpointsToRenderPose();
			}
		}

		private Vector3 GetAxis(Transform t, Axis axis)
		{
			return axis switch
			{
				Axis.Forward => t.forward, 
				Axis.Back => -t.forward, 
				Axis.Right => t.right, 
				Axis.Left => -t.right, 
				Axis.Up => t.up, 
				Axis.Down => -t.up, 
				_ => t.forward, 
			};
		}

		private void Update()
		{
			ApplyObiArmResolutionConfiguration();
			if (_armStartSet && !(_shoulderTransform == null))
			{
				if (_particleStartSecondAttachment != null && _startArmSecondSnapAttachment != null)
				{
					_startArmSecondSnapAttachment.Snap(_shoulderTransform.position);
				}
				if (_armEndSet && !(_armEndSecondForRope == null) && _secondEndArmSnapAttachment != null)
				{
					_secondEndArmSnapAttachment.Snap(_armEndSecondForRope.position);
				}
			}
		}

		private void ApplyObiArmResolutionConfiguration()
		{
			if (ReduceSolverUpdateEnabled)
			{
				ObiArmResolutionAuthorityConfiguration authorityResolutionConfiguration = GetAuthorityResolutionConfiguration();
				ObiArmResolutionTierConfiguration obiArmResolutionTierConfiguration = (IsResolvingHighTier() ? authorityResolutionConfiguration.HighTier : authorityResolutionConfiguration.LowTier);
				_obiSolver.substeps = obiArmResolutionTierConfiguration.Substeps;
			}
		}

		private ObiArmResolutionAuthorityConfiguration GetAuthorityResolutionConfiguration()
		{
			if (!base.HasStateAuthority)
			{
				return _obiArmResolutionConfiguration.NonAuthority;
			}
			return _obiArmResolutionConfiguration.Authority;
		}

		private bool IsResolvingHighTier()
		{
			if (_armEndGrabbable != null && _armEndGrabbable.GrabbedByPlayers.Count > 0)
			{
				return true;
			}
			if (_lineArmController != null && ArmOrientation == Arm.Right && _lineArmController.CurrentGrabbables.Count > 0)
			{
				return true;
			}
			return false;
		}

		private void InterpolateLastArmEndPosition()
		{
			if (base.Object == null)
			{
				return;
			}
			bool flag = _playerStateService.GetPlayerState(base.Object.InputAuthority.PlayerId) == PlayerState.Store && ArmOrientation == Arm.Right;
			if (TargetOwnerNetworkID != default(NetworkId) && (TargetOwnerNetworkID != _settedTargetOwnerNetworkID || TargetHandleIndex != _settedTargetHandleIndex) && !base.Object.HasStateAuthority && !TrySetTargetTransform(TargetOwnerNetworkID, TargetHandleIndex))
			{
				return;
			}
			if (flag)
			{
				if (!_lineArmsModel.TryGetLineArmForPlayer(base.Object.InputAuthority.PlayerId, out var lineArm))
				{
					return;
				}
				bool flag2 = false;
				foreach (IPointGrabable currentGrabbable in lineArm.CurrentGrabbables)
				{
					if (currentGrabbable.GrabObject != null && currentGrabbable.GrabObject.Grabbers.Count > 0)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (lineArm.PhysGrabber != null)
					{
						_targetTransform = lineArm.PhysGrabber.transform;
					}
					_cachedTargetTransform = null;
					_currentHandle = null;
				}
				else
				{
					_targetTransform = _cachedTargetTransform;
				}
				if (ProcessRagdollReturn(flag))
				{
					return;
				}
				if (!flag2)
				{
					HandleStoreInterpolation();
					return;
				}
			}
			if (!ProcessRagdollReturn(flag))
			{
				HandleDefaultInterpolation();
			}
		}

		private bool ProcessRagdollReturn(bool isStoreArm)
		{
			if (!_isReturningFromRagdoll)
			{
				return false;
			}
			if (_targetTransform == null || _armEnd == null || _armEndController == null)
			{
				return false;
			}
			if (_armEndController.IsGrabbedBySomeone || _isArmControlledByPhysics)
			{
				_isReturningFromRagdoll = false;
				return false;
			}
			Vector3 b = (isStoreArm ? ClampPositionToStoreArmDonut(_targetTransform.position) : _targetTransform.position);
			_ragdollReturnElapsed += Time.deltaTime;
			float num = Mathf.Clamp01(_ragdollReturnElapsed / Mathf.Max(0.0001f, _ragdollReturnDuration));
			float t = Mathf.SmoothStep(0f, 1f, num);
			_armEnd.position = Vector3.Lerp(_ragdollReturnStartPosition, b, t);
			if (num >= 1f)
			{
				_isReturningFromRagdoll = false;
			}
			return true;
		}

		private void HandleStoreInterpolation()
		{
			if (!(_targetTransform == null) && !(_armEnd == null) && !(_armEndController == null) && !_armEndController.IsGrabbedBySomeone && !_isArmControlledByPhysics)
			{
				Vector3 vector = ClampPositionToStoreArmDonut(_targetTransform.position);
				float b = 0f;
				if (_lineArmController != null)
				{
					b = _lineArmController.ArmEndMoveSpeed;
				}
				float t = Time.fixedDeltaTime / Mathf.Max(0.0001f, b);
				Vector3 position = _armEnd.position;
				float num = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(vector.x, vector.z));
				_armEnd.position = ((num > 10f) ? vector : new Vector3(Mathf.Lerp(position.x, vector.x, t), Mathf.Lerp(position.y, vector.y, t), Mathf.Lerp(position.z, vector.z, t)));
			}
		}

		private bool TryGetStoreDonutCenter(out Vector3 center)
		{
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Object.InputAuthority, out var value) && value != null)
			{
				center = value.transform.position;
				return true;
			}
			if (_playerCharacterMovableBase != null)
			{
				center = _playerCharacterMovableBase.transform.position;
				return true;
			}
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Object.StateAuthority, out value) && value != null)
			{
				center = value.transform.position;
				return true;
			}
			center = default(Vector3);
			return false;
		}

		private Vector3 ClampPositionToStoreArmDonut(Vector3 worldPosition)
		{
			if (!TryGetStoreDonutCenter(out var center))
			{
				return worldPosition;
			}
			float num = Mathf.Min(_storeArmMinRangeNoGrabbing, _storeArmRangeNoGrabbing);
			float num2 = Mathf.Max(_storeArmMinRangeNoGrabbing, _storeArmRangeNoGrabbing);
			if (num2 <= 0f)
			{
				return worldPosition;
			}
			Vector3 vector = new Vector3(worldPosition.x - center.x, 0f, worldPosition.z - center.z);
			float magnitude = vector.magnitude;
			if (magnitude < Mathf.Epsilon)
			{
				Vector3 vector2 = new Vector3(_armEnd.position.x - center.x, 0f, _armEnd.position.z - center.z);
				if (vector2.sqrMagnitude < Mathf.Epsilon)
				{
					vector2 = Vector3.forward;
				}
				vector = vector2.normalized * num;
			}
			else if (magnitude < num || magnitude > num2)
			{
				vector = vector / magnitude * Mathf.Clamp(magnitude, num, num2);
			}
			return new Vector3(center.x + vector.x, worldPosition.y, center.z + vector.z);
		}

		private void HandleDefaultInterpolation()
		{
			if (!(_targetTransform != null) || !(_armEnd != null) || !(_armEndController != null) || _armEndController.IsGrabbedBySomeone || _isArmControlledByPhysics)
			{
				return;
			}
			if (!InIdlePose)
			{
				_armEnd.position = ResolveGrabbedHandleRenderPosition();
				return;
			}
			float b = 0f;
			if (_lineArmController != null)
			{
				b = _lineArmController.ArmEndMoveSpeed;
			}
			float t = Time.fixedDeltaTime / Mathf.Max(0.0001f, b);
			float num = Vector3.Distance(_armEnd.position, _targetTransform.position);
			_armEnd.position = ((num > 10f) ? _targetTransform.position : Vector3.Lerp(_armEnd.position, _targetTransform.position, t));
		}

		private Vector3 ResolveGrabbedHandleRenderPosition()
		{
			PhysicsInfluenceVolume physicsInfluenceVolume = ResolveTargetCarrierVolume();
			if (physicsInfluenceVolume == null)
			{
				return _targetTransform.position;
			}
			Transform transform = physicsInfluenceVolume.transform;
			physicsInfluenceVolume.GetCarrierRenderPose(out var position, out var rotation);
			Vector3 point = transform.worldToLocalMatrix.MultiplyPoint3x4(_targetTransform.position);
			return Matrix4x4.TRS(position, rotation, transform.lossyScale).MultiplyPoint3x4(point);
		}

		private PhysicsInfluenceVolume ResolveTargetCarrierVolume()
		{
			if ((object)_targetTransform != _carrierVolumeTargetRef)
			{
				_carrierVolumeTargetRef = _targetTransform;
				_targetCarrierVolume = ((_targetTransform != null) ? _targetTransform.GetComponentInParent<PhysicsInfluenceVolume>() : null);
			}
			return _targetCarrierVolume;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 773565831u)]
		private void SetTargetTransform_RPC([RpcPayload(4)] NetworkId ownerID, [RpcPayload(4)] int handleIndex)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(773565831u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.ArmVisualsController::SetTargetTransform_RPC(Fusion.NetworkId,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(ownerID, 4);
						writer.Write(handleIndex, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			TargetOwnerNetworkID = ownerID;
			TargetHandleIndex = handleIndex;
			TrySetTargetTransform(ownerID, handleIndex);
		}

		private bool TrySetTargetTransform(NetworkId ownerID, int handleIndex)
		{
			NetworkObject networkObject = base.Runner.FindObject(ownerID);
			if (networkObject == null)
			{
				return false;
			}
			Transform transform;
			if (handleIndex < 0)
			{
				transform = networkObject.transform;
			}
			else
			{
				if (!networkObject.TryGetComponent<IPointGrabable>(out var component) || component.Handles == null || handleIndex >= component.Handles.Count || component.Handles[handleIndex] == null)
				{
					return false;
				}
				transform = component.Handles[handleIndex];
			}
			_settedTargetOwnerNetworkID = ownerID;
			_settedTargetHandleIndex = handleIndex;
			_targetTransform = transform;
			_cachedTargetTransform = transform;
			return true;
		}

		private void AdjustLength()
		{
			if (!(_armStart == null) && !(_armEnd == null) && !(_armEndController == null) && !(_ropeLengthController == null))
			{
				float value = Vector3.Distance(_armStart.position, _armEnd.position);
				value = ((!InIdlePose) ? Mathf.Clamp(value, 0f, _maxRopeLength) : Mathf.Clamp(value, 0f, _maxRopeLengthIdle));
				if (!InIdlePose || _armEndController.IsGrabbedBySomeone)
				{
					value += _additionalLengthWehDraggingObjects;
				}
				if (InIdlePose && _armEndController.IsGrabbedBySomeone)
				{
					value += _additionalLengthWehGrabbedByPlayers;
				}
				float num = _distanceErrorOnRemovingLength;
				if (InIdlePose)
				{
					num = _distanceErrorOnRemovingLengthIdle;
				}
				if (_ropeLengthController.CurrentLength < value - _stretchLength)
				{
					_ropeLengthController.AddLengthWithCustomDelta(base.Runner.DeltaTime);
				}
				else if (_ropeLengthController.CurrentLength > value - (_stretchLength - num))
				{
					_ropeLengthController.RemoveLengthWithCustomDelta(base.Runner.DeltaTime);
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2074382894u)]
		private void SnapArmStart_RPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2074382894u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.ArmVisualsController::SnapArmStart_RPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!(base.Object == null))
			{
				if (!_armStartsModel.IsContainsStart(base.Object.StateAuthority, ArmOrientation))
				{
					ArmStartsModel armStartsModel = _armStartsModel;
					armStartsModel.OnArmStartAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Combine(armStartsModel.OnArmStartAdded, new Action<PlayerRef, Transform, Arm>(ArmStartAdded));
				}
				else if (!_armStartSet)
				{
					SetArmStart();
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2726215737u)]
		private void SnapArmEnd_RPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2726215737u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.ArmVisualsController::SnapArmEnd_RPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Object == null)
			{
				return;
			}
			if (!_armStartsModel.IsContainsEnd(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmStartAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Combine(armStartsModel.OnArmStartAdded, new Action<PlayerRef, Transform, Arm>(ArmEndAdded));
				return;
			}
			if (!_armEndIdlePoseSet && !_armStartsModel.IsContainsIdlePose(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel2 = _armStartsModel;
				armStartsModel2.OnArmIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Combine(armStartsModel2.OnArmIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmIdlePoseAdded));
			}
			if (!_armEndSet)
			{
				SetArmEnd();
			}
		}

		private void ArmStartAdded(PlayerRef playerRef, Transform armStart, Arm arm)
		{
			if (!(base.Object == null) && base.Object.IsValid && !(playerRef != base.Object.StateAuthority) && arm == ArmOrientation && _armStartsModel.IsContainsStart(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmStartAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Remove(armStartsModel.OnArmStartAdded, new Action<PlayerRef, Transform, Arm>(ArmStartAdded));
				if (!_armStartSet)
				{
					SetArmStart();
				}
			}
		}

		private void ArmEndAdded(PlayerRef playerRef, Transform armStart, Arm arm)
		{
			if (!(base.Object == null) && base.Object.IsValid && !(playerRef != base.Object.StateAuthority) && arm == ArmOrientation && _armStartsModel.IsContainsEnd(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmStartAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Remove(armStartsModel.OnArmStartAdded, new Action<PlayerRef, Transform, Arm>(ArmEndAdded));
				if (!_armEndSet)
				{
					SetArmEnd();
				}
			}
		}

		private void OnArmIdlePoseAdded(PlayerRef playerRef, Transform armStart, Arm arm)
		{
			if (!(base.Object == null) && base.Object.IsValid && !(playerRef != base.Object.StateAuthority) && arm == ArmOrientation && _armStartsModel.IsContainsIdlePose(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Remove(armStartsModel.OnArmIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmIdlePoseAdded));
				if (!_armEndIdlePoseSet)
				{
					SetEndIdlePose();
				}
			}
		}

		private void OnArmStoreIdlePoseAdded(PlayerRef playerRef, Transform armStart, Arm arm)
		{
			if (base.Object == null || !base.Object.IsValid || playerRef != base.Object.StateAuthority || arm != ArmOrientation || !_armStartsModel.IsContainsIdleStorePose(base.Object.StateAuthority, ArmOrientation))
			{
				return;
			}
			ArmStartsModel armStartsModel = _armStartsModel;
			armStartsModel.OnArmStoreIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Remove(armStartsModel.OnArmStoreIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmStoreIdlePoseAdded));
			if (!_armEndStoreIdlePoseSet)
			{
				SetEndStoreIdlePose();
				if (_playerStateService.GetPlayerState(base.Object.InputAuthority.PlayerId) == PlayerState.Store)
				{
					SetTargetToIdlePose(_armEndStoreIdlePose);
				}
			}
		}

		private void SetArmStart()
		{
			if (!_armStartsModel.IsContainsStart(base.Object.StateAuthority, ArmOrientation))
			{
				return;
			}
			_armStart = _armStartsModel.GetStart(base.Object.StateAuthority, ArmOrientation);
			if (_particleStartAttachment != null && _armStartForRope != null)
			{
				_particleStartAttachment.target = _armStartForRope;
				_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			}
			if (_particleStartSecondAttachment != null && _armStartForRope != null)
			{
				_particleStartSecondAttachment.target = _armStartForRope;
				_particleStartSecondAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
				if (_startArmSecondSnapAttachment != null && _shoulderTransform != null)
				{
					_startArmSecondSnapAttachment.Snap(_shoulderTransform.position);
				}
			}
			if (_startArmSnapAttachment != null)
			{
				_startArmSnapAttachment.Snap();
			}
			_armStartSet = true;
		}

		private void SetArmEnd()
		{
			if (!_armStartsModel.IsContainsEnd(base.Object.StateAuthority, ArmOrientation))
			{
				return;
			}
			_armEnd = _armStartsModel.GetEnd(base.Object.StateAuthority, ArmOrientation).Transform;
			if (_armEnd != null)
			{
				_armEndController = _armEnd.GetComponent<ArmEndController>();
			}
			if (!_armStartsModel.IsContainsIdlePose(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Combine(armStartsModel.OnArmIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmIdlePoseAdded));
			}
			else
			{
				SetEndIdlePose();
			}
			if (!_armStartsModel.IsContainsIdleStorePose(base.Object.StateAuthority, ArmOrientation))
			{
				ArmStartsModel armStartsModel2 = _armStartsModel;
				armStartsModel2.OnArmStoreIdlePoseAdded = (Action<PlayerRef, Transform, Arm>)Delegate.Combine(armStartsModel2.OnArmStoreIdlePoseAdded, new Action<PlayerRef, Transform, Arm>(OnArmStoreIdlePoseAdded));
			}
			else
			{
				SetEndStoreIdlePose();
			}
			if (_particleEndAttachment != null && _armEndForRope != null)
			{
				_particleEndAttachment.target = _armEndForRope;
				_particleEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			}
			if (_endArmSnapAttachment != null)
			{
				_endArmSnapAttachment.Snap();
			}
			if (_particleSecondEndAttachment != null && _armEndForRope != null)
			{
				_particleSecondEndAttachment.target = _armEndForRope;
				_particleSecondEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
				if (_secondEndArmSnapAttachment != null && _armEndSecondForRope != null)
				{
					_secondEndArmSnapAttachment.Snap(_armEndSecondForRope.position);
				}
			}
			_armEndSet = true;
		}

		private void SetEndIdlePose()
		{
			_armEndIdlePose = _armStartsModel.GetIdlePose(base.Object.StateAuthority, ArmOrientation);
			if (_armEnd != null && _armEndIdlePose != null)
			{
				_armEnd.position = _armEndIdlePose.position;
			}
			_armEndIdlePoseSet = true;
		}

		private void SetEndStoreIdlePose()
		{
			_armEndStoreIdlePose = _armStartsModel.GetIdleStorePose(base.Object.StateAuthority, ArmOrientation);
			_armEndStoreIdlePoseSet = true;
		}

		public void SnapToStoreIdlePose()
		{
			if (_armEndStoreIdlePose == null)
			{
				SetEndStoreIdlePose();
			}
			if (!(_armEndStoreIdlePose == null) && !(_armEnd == null))
			{
				_isReturningFromRagdoll = false;
				SetTargetToIdlePose(_armEndStoreIdlePose);
				_armEnd.position = _armEndStoreIdlePose.position;
			}
		}

		public void SetArmOrientation(Arm armOrientation)
		{
			ArmOrientation = armOrientation;
			_armStartsModel.AddArm(base.Object.StateAuthority, base.transform, armOrientation);
			if (_registeredArmOrientation != Arm.None && _registeredArmOrientation != armOrientation)
			{
				_armVisualsControllerModel.UnregisterArmVisualsController(_registeredPlayerId, _registeredArmOrientation, this);
				RegisterArmVisuals();
			}
		}

		public void SetLineArmController(LineArmType lineArmType)
		{
			LineArmType = lineArmType;
		}

		private void EnableArmControlledByPhysics(IRagdollEntity ragdollEntity)
		{
			if (!(_playerRagdollEntity == null) && _playerRagdollEntity.IsHandsPhysicsActive)
			{
				_isReturningFromRagdoll = false;
				_isArmControlledByPhysics = true;
				if (_ropeLengthController != null)
				{
					_ropeLengthController.BlockLengthChange();
				}
			}
		}

		private void DisableArmControlledByPhysics(IRagdollEntity ragdollEntity)
		{
			if (!(_playerRagdollEntity != null) || !_playerRagdollEntity.IsHandsPhysicsActive)
			{
				_isArmControlledByPhysics = false;
				StartRagdollReturn();
				if (_ropeLengthController != null)
				{
					_ropeLengthController.UnBlockLengthChange();
				}
			}
		}

		private void StartRagdollReturn()
		{
			if (!(_armEnd == null))
			{
				_isReturningFromRagdoll = true;
				_ragdollReturnElapsed = 0f;
				_ragdollReturnStartPosition = _armEnd.position;
			}
		}

		private void ReconcileArmControlledByPhysics()
		{
			if (_playerRagdollEntity == null)
			{
				return;
			}
			bool isHandsPhysicsActive = _playerRagdollEntity.IsHandsPhysicsActive;
			if (isHandsPhysicsActive != _isArmControlledByPhysics)
			{
				if (isHandsPhysicsActive)
				{
					EnableArmControlledByPhysics(_playerRagdollEntity);
				}
				else
				{
					DisableArmControlledByPhysics(_playerRagdollEntity);
				}
			}
		}

		private void ProcessStoreIdlePoseEnter(PlayerStateData playerStateData)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerStateData.PlayerId == base.Object.InputAuthority.PlayerId && ArmOrientation == Arm.Left && playerStateData.PlayerState == PlayerState.Store)
			{
				SetTargetToIdlePose(_armEndStoreIdlePose);
			}
		}

		private void ProcessStoreIdlePoseExit(PlayerStateData playerStateData)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerStateData.PlayerId == base.Object.InputAuthority.PlayerId && ArmOrientation == Arm.Left && playerStateData.PlayerState == PlayerState.Store)
			{
				SetTargetToIdlePose(_armEndIdlePose);
			}
		}

		private void SetTargetToIdlePose(Transform idlePose)
		{
			if (!(idlePose == null) && idlePose.TryGetComponent<NetworkObject>(out var component))
			{
				SetTargetTransform_RPC(component.Id, -1);
				InIdlePose = true;
			}
		}

		private void OnHandsRagdollChanged(bool simulate)
		{
			ReconcileArmControlledByPhysics();
		}

		public void SetArmEndGrabbable(IPointGrabable grabbable)
		{
			ArmEndGrabbableNetworkId = grabbable.NetworkObject.Id;
		}

		private void SetLineArmInternal()
		{
			if (_lineArmsModel.TryGetLineArmForPlayer(base.Object.InputAuthority.PlayerId, LineArmType, out var lineArm))
			{
				_lineArmController = lineArm;
			}
		}

		private void SetArmEndGrabbableInternal()
		{
			if (base.Runner.TryFindObject(ArmEndGrabbableNetworkId, out var networkObject))
			{
				_armEndGrabbable = networkObject.GetComponent<IPointGrabable>();
			}
		}

		private void SwitchPlayerArmsMaterial()
		{
			_ropeExtrudedRenderer.material = (base.HasStateAuthority ? _authorativeArmsMaterial : _nonAuthorativeArmsMaterial);
			_ropeExtrudedRenderer.OnValidate();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ArmOrientation = _ArmOrientation;
			TargetOwnerNetworkID = _TargetOwnerNetworkID;
			TargetHandleIndex = _TargetHandleIndex;
			InIdlePose = _InIdlePose;
			LineArmType = _LineArmType;
			ArmEndGrabbableNetworkId = _ArmEndGrabbableNetworkId;
			ReduceSolverUpdateEnabled = _ReduceSolverUpdateEnabled;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ArmOrientation = ArmOrientation;
			_TargetOwnerNetworkID = TargetOwnerNetworkID;
			_TargetHandleIndex = TargetHandleIndex;
			_InIdlePose = InIdlePose;
			_LineArmType = LineArmType;
			_ArmEndGrabbableNetworkId = ArmEndGrabbableNetworkId;
			_ReduceSolverUpdateEnabled = ReduceSolverUpdateEnabled;
		}

		[NetworkRpcWeavedInvoker(773565831u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetTargetTransform_RPC_0040Invoker773565831([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out NetworkId value, 4);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ArmVisualsController)context.TargetBehaviour).SetTargetTransform_RPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2074382894u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SnapArmStart_RPC_0040Invoker2074382894([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ArmVisualsController)context.TargetBehaviour).SnapArmStart_RPC();
		}

		[NetworkRpcWeavedInvoker(2726215737u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SnapArmEnd_RPC_0040Invoker2726215737([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ArmVisualsController)context.TargetBehaviour).SnapArmEnd_RPC();
		}
	}
}
