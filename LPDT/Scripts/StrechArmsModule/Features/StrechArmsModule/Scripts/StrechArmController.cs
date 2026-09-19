using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkInputModule.Scripts;
using Fusion;
using Fusion.Addons.Physics;
using Obi;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class StrechArmController : NetworkBehaviour, IArmController
	{
		[SerializeField]
		private LayerMask _raycastLayerMask;

		private const float HOLD_TIME = 0.2f;

		[SerializeField]
		private SnapAttachment _startArmSnapAttachment;

		[SerializeField]
		private SnapAttachment _endArmSnapAttachment;

		[SerializeField]
		private ObiRope _rope;

		[SerializeField]
		private ObiSolver _obiSolver;

		[SerializeField]
		private RopeLengthController _ropeLengthController;

		[SerializeField]
		private Rigidbody _endPositionRigidbody;

		[SerializeField]
		private NetworkRigidbody _endPositionNetworkRigidbody;

		[SerializeField]
		private ObiParticleAttachment _particleStartAttachment;

		[SerializeField]
		private ObiParticleAttachment _particleEndAttachment;

		[SerializeField]
		private Transform _idlePoseTransform;

		[Header("Throw Settings")]
		[SerializeField]
		private float _throwDistance = 10f;

		[SerializeField]
		private float _throwDuration = 0.6f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ArmOrientation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Arm _ArmOrientation;

		private GrabArmController _grabArmController;

		private ArmEndController _armEndController;

		private bool _isArmThrowing;

		private Coroutine _throwCoroutine;

		private ArmStartsModel _armStartsModel;

		private bool _armStartSetted;

		private bool _armEndSetted;

		private Coroutine _lengthCoroutine;

		private Coroutine _ButtonHoldingCoroutine;

		private Coroutine _moveToIdlingCoroutine;

		private Coroutine _enemyGrabbedCoroutine;

		private bool inIdlePose;

		private bool inEnemyGrabbedPose;

		private MultiplayerModel _multiplayerModel;

		private CameraModel _cameraModel;

		private ArmControllersModel _armControllersModel;

		private ArmStateModel _armStateModel;

		private bool _isRegistered;

		private Vector3 _hitPoint;

		private IInputService _inputService;

		private ArmControllerData _armControllerData;

		private bool _isTornOff;

		private bool _isRightArmHolding;

		private bool _isRightArmRemovingLength;

		private PlayerMovableModel _playerMovableModel;

		private Transform _startTransform;

		private Transform _playerTransform;

		private IdlePoseForArmModel _idlePoseForArmModel;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe Arm ArmOrientation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StrechArmController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Arm*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StrechArmController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Arm*)((byte*)Ptr + 0) = value;
			}
		}

		public bool IsTornOff => _isTornOff;

		public event Action<Arm> OnArmEnable;

		public event Action<Arm> OnArmDisable;

		[Inject]
		private void InjectDependencies(ArmStartsModel armStartsModel, MultiplayerModel multiplayerModel, CameraModel cameraModel, ArmStateModel armStateModel, IInputService inputService, PlayerMovableModel playerMovableModel, ArmControllersModel armControllersModel, IdlePoseForArmModel idlePoseForArmModel)
		{
			_idlePoseForArmModel = idlePoseForArmModel;
			_inputService = inputService;
			_armStartsModel = armStartsModel;
			_multiplayerModel = multiplayerModel;
			_cameraModel = cameraModel;
			_armStateModel = armStateModel;
			_armControllersModel = armControllersModel;
			_playerMovableModel = playerMovableModel;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_armControllersModel.UnregisterStretchArmController(base.Object.StateAuthority, _armControllerData);
		}

		public void SetArmOrientation(Arm armOrientation)
		{
			ArmOrientation = armOrientation;
			TryRegisterController();
		}

		public void Enable()
		{
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_playerTransform = _playerMovableModel.AllCharacterMovables[base.Object.InputAuthority].transform;
			base.gameObject.SetActive(value: true);
			this.OnArmEnable?.Invoke(ArmOrientation);
		}

		public void Disable()
		{
			base.gameObject.SetActive(value: false);
			this.OnArmDisable?.Invoke(ArmOrientation);
		}

		private void TryRegisterController()
		{
			if (base.Object != null && ArmOrientation != Arm.None && !_isRegistered && _armStateModel != null)
			{
				_armStateModel.RegisterArmController(ArmOrientation, this);
				_isRegistered = true;
				RegisterStretchArmControllerRpc();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 418659427u)]
		private void RegisterStretchArmControllerRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(418659427u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::RegisterStretchArmControllerRpc()", invokeInfo, PlayerRef.None);
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
			_armControllerData = new ArmControllerData
			{
				Arm = ArmOrientation,
				StretchArmController = this
			};
			_armControllersModel.RegisterStretchArmController(base.Object.StateAuthority, _armControllerData);
		}

		private void SetArmStart(Transform transform)
		{
			_particleStartAttachment.target = transform;
		}

		private void SetArmEnd(Transform transform)
		{
			_particleEndAttachment.target = transform;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_grabArmController.OnHited -= StopThrow;
		}

		private void Update()
		{
			if (_isTornOff)
			{
				return;
			}
			if (_grabArmController != null)
			{
				bool flag = _ropeLengthController.CurrentLength > 0.1f;
				_grabArmController.SetIsThrow(flag);
				if (_grabArmController.EnemyGrabbed && !inEnemyGrabbedPose)
				{
					SetToEnemyGrabbed();
				}
				else if (!flag && !inIdlePose && _grabArmController.GrabController.CurrentGrabState != GrabState.Grabbed && _grabArmController.GrabController.CurrentGrabState != GrabState.GrabStatic && !_grabArmController.EnemyGrabbed)
				{
					SetToIdlePose();
				}
				else if (flag && inIdlePose && !_grabArmController.EnemyGrabbed)
				{
					ResetIdlePose();
				}
			}
			SnapArmStart();
			SnapArmEnd();
			UpdateArmState();
			if (_startTransform != null)
			{
				SetArmStart(_startTransform);
				_startArmSnapAttachment.Snap(_startTransform.position);
			}
			if (_endPositionRigidbody != null)
			{
				_endArmSnapAttachment.Snap(_endPositionRigidbody.position);
			}
		}

		public void SetToEnemyGrabbed()
		{
			if (_moveToIdlingCoroutine != null)
			{
				StopCoroutine(_moveToIdlingCoroutine);
			}
			if (base.Object.HasStateAuthority)
			{
				_endPositionRigidbody.isKinematic = true;
			}
			inIdlePose = false;
			inEnemyGrabbedPose = true;
			_obiSolver.maxStepsPerFrame = 1;
			base.transform.SetParent(null, worldPositionStays: true);
			_grabArmController.VisualController.SetTargetPositionForInterpolation(null);
			_endPositionNetworkRigidbody.InterpolationTarget = _grabArmController.VisualController.VisualsRoot;
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_particleEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_enemyGrabbedCoroutine = StartCoroutine(MoveToStaticGrababble());
		}

		private void SetToIdlePose()
		{
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_particleEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			if (base.Object.HasStateAuthority)
			{
				_endPositionRigidbody.isKinematic = true;
			}
			inIdlePose = true;
			inEnemyGrabbedPose = false;
			_moveToIdlingCoroutine = StartCoroutine(MoveToIdlePose());
			if (_enemyGrabbedCoroutine != null)
			{
				StopCoroutine(_enemyGrabbedCoroutine);
			}
			if (_armEndController != null)
			{
				_armEndController.RemoveSynchronizationReason(ArmEndSyncReason.Throwing);
			}
		}

		public void ResetIdlePose()
		{
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_particleEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
			inIdlePose = false;
			inEnemyGrabbedPose = false;
			_obiSolver.maxStepsPerFrame = 1;
			base.transform.SetParent(null, worldPositionStays: true);
			_grabArmController.VisualController.SetTargetPositionForInterpolation(null);
			_endPositionNetworkRigidbody.InterpolationTarget = _grabArmController.VisualController.VisualsRoot;
			_endPositionRigidbody.transform.SetParent(null, worldPositionStays: true);
			if (_moveToIdlingCoroutine != null)
			{
				StopCoroutine(_moveToIdlingCoroutine);
			}
			if (_enemyGrabbedCoroutine != null)
			{
				StopCoroutine(_enemyGrabbedCoroutine);
			}
		}

		private IEnumerator MoveToStaticGrababble()
		{
			if (_endPositionRigidbody == null || _idlePoseForArmModel == null)
			{
				yield break;
			}
			bool wasNearTheParent = false;
			_ = _endPositionRigidbody.position;
			float elapsed = 0f;
			while (_grabArmController.EnemyGrabbed && !_isTornOff)
			{
				if (_endPositionRigidbody == null || _idlePoseForArmModel == null)
				{
					yield break;
				}
				yield return null;
				_ = _endPositionRigidbody.position;
				Vector3 position = _grabArmController.EnemyTransform.position;
				_ropeLengthController.AddLength(40f);
				if (wasNearTheParent)
				{
					_endPositionNetworkRigidbody.transform.position = position;
					_endPositionNetworkRigidbody.InterpolationTarget = _grabArmController.VisualController.VisualsRoot;
					continue;
				}
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / 0.9f);
				Vector3 position2 = Vector3.Lerp(_endPositionRigidbody.position, position, t);
				_endPositionRigidbody.MovePosition(position2);
				if (elapsed >= 0.9f)
				{
					_grabArmController.VisualController.SetTargetPositionForInterpolation(_grabArmController.EnemyTransform);
					wasNearTheParent = true;
				}
			}
			if (base.Object.HasStateAuthority)
			{
				_endPositionRigidbody.isKinematic = false;
			}
			inEnemyGrabbedPose = false;
			_obiSolver.maxStepsPerFrame = 1;
			base.transform.SetParent(null, worldPositionStays: true);
			_grabArmController.VisualController.SetTargetPositionForInterpolation(null);
			_endPositionNetworkRigidbody.InterpolationTarget = _grabArmController.VisualController.VisualsRoot;
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			_particleEndAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
		}

		private IEnumerator MoveToIdlePose()
		{
			if (_endPositionRigidbody == null || _idlePoseForArmModel == null)
			{
				yield break;
			}
			bool wasNearTheParent = false;
			_ = _idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority).position;
			_ = _endPositionRigidbody.position;
			float elapsed = 0f;
			while (inIdlePose && !(_endPositionRigidbody == null) && _idlePoseForArmModel != null)
			{
				yield return null;
				_ = _endPositionRigidbody.position;
				Vector3 position = _idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority).position;
				if (wasNearTheParent)
				{
					_endPositionRigidbody.MovePosition(position);
					continue;
				}
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / 0.9f);
				Vector3 position2 = Vector3.Lerp(_endPositionRigidbody.position, position, t);
				_endPositionRigidbody.MovePosition(position2);
				if (IsNearTarget(_endPositionRigidbody.position, position))
				{
					base.transform.SetParent(_idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority), worldPositionStays: true);
					_grabArmController.VisualController.SetTargetPositionForInterpolation(_idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority));
					_endPositionNetworkRigidbody.InterpolationTarget = _endPositionNetworkRigidbody.transform;
					_endPositionRigidbody.transform.SetParent(_idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority), worldPositionStays: true);
					wasNearTheParent = true;
					yield return new WaitForFixedUpdate();
					_obiSolver.maxStepsPerFrame = 0;
				}
			}
		}

		private bool IsNearTarget(Vector3 srcPosition, Vector3 targetPosition)
		{
			return Mathf.Approximately(Vector3.Distance(srcPosition, targetPosition), 0f);
		}

		private void UpdateArmState()
		{
			if (!(base.Object == null) && _isRegistered && _armStateModel != null && !(_ropeLengthController == null))
			{
				float progress = Mathf.Clamp01(_ropeLengthController.CurrentLength / _throwDistance);
				_armStateModel.SetArmProgress(ArmOrientation, progress);
				if (_isArmThrowing && _armStateModel.GetArmState(ArmOrientation) != ArmState.Grabbing)
				{
					_armStateModel.SetArmState(ArmOrientation, ArmState.Flying);
				}
				else if (!_isArmThrowing && _ropeLengthController.CurrentLength < 0.1f && _armStateModel.GetArmState(ArmOrientation) != ArmState.Grabbing)
				{
					_armStateModel.SetArmState(ArmOrientation, ArmState.ReadyToThrow);
				}
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (_isTornOff || base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer || !GetInput<NetworkInputActions>(out var input))
			{
				return;
			}
			if (ArmOrientation == Arm.Left)
			{
				if (input.DropAllFromArmsPhase.IsSet(InputActionPhase.Started) && !_isArmThrowing && (double)_ropeLengthController.CurrentLength < 0.1)
				{
					ThrowArmRpc();
					return;
				}
				if (input.GrabItemPhase.IsSet(InputActionPhase.Started) && !_isArmThrowing)
				{
					StartCheckingForRightArmHold();
				}
				if (input.GrabItemPhase.IsSet(InputActionPhase.Canceled) && _isRightArmRemovingLength)
				{
					StopRemoveLengthRpc();
				}
			}
			if (ArmOrientation != Arm.Right)
			{
				return;
			}
			if (input.DropAllFromArmsPhase.IsSet(InputActionPhase.Started) && !_isArmThrowing && (double)_ropeLengthController.CurrentLength < 0.1)
			{
				ThrowArmRpc();
				return;
			}
			if (input.GrabItemPhase.IsSet(InputActionPhase.Started) && !_isArmThrowing)
			{
				StartCheckingForRightArmHold();
			}
			if (input.GrabItemPhase.IsSet(InputActionPhase.Canceled) && _isRightArmRemovingLength)
			{
				StopRemoveLengthRpc();
			}
		}

		public void StartCheckingForRightArmHold()
		{
			_isRightArmHolding = true;
			if (ArmOrientation == Arm.Right)
			{
				InputDefaultActions grabItem = _inputService.GrabItem;
				grabItem.Canceled = (Action)Delegate.Combine(grabItem.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem2 = _inputService.GrabItem;
				grabItem2.Performed = (Action)Delegate.Combine(grabItem2.Performed, new Action(OnRightArmHoldCanceled));
			}
			else if (ArmOrientation == Arm.Left)
			{
				InputDefaultActions grabItem3 = _inputService.GrabItem;
				grabItem3.Canceled = (Action)Delegate.Combine(grabItem3.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem4 = _inputService.GrabItem;
				grabItem4.Performed = (Action)Delegate.Combine(grabItem4.Performed, new Action(OnRightArmHoldCanceled));
			}
			_ButtonHoldingCoroutine = StartCoroutine(WaitForRightButtonHold());
		}

		private void OnRightArmHoldCanceled()
		{
			if (ArmOrientation == Arm.Right)
			{
				InputDefaultActions grabItem = _inputService.GrabItem;
				grabItem.Canceled = (Action)Delegate.Remove(grabItem.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem2 = _inputService.GrabItem;
				grabItem2.Performed = (Action)Delegate.Remove(grabItem2.Performed, new Action(OnRightArmHoldCanceled));
			}
			else if (ArmOrientation == Arm.Left)
			{
				InputDefaultActions grabItem3 = _inputService.GrabItem;
				grabItem3.Canceled = (Action)Delegate.Remove(grabItem3.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem4 = _inputService.GrabItem;
				grabItem4.Performed = (Action)Delegate.Remove(grabItem4.Performed, new Action(OnRightArmHoldCanceled));
			}
			_isRightArmHolding = false;
		}

		public IEnumerator WaitForRightButtonHold()
		{
			float elapsedTime = 0f;
			while (elapsedTime < 0.2f)
			{
				if (!_isRightArmHolding)
				{
					yield break;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			StartRemoveLengthRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 979024373u)]
		private void StartRemoveLengthRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(979024373u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::StartRemoveLengthRpc()", invokeInfo, PlayerRef.None);
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
			_lengthCoroutine = StartCoroutine(RemoveLengthCoroutine());
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 503031437u)]
		private void StopRemoveLengthRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(503031437u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::StopRemoveLengthRpc()", invokeInfo, PlayerRef.None);
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
			if (_lengthCoroutine != null)
			{
				StopCoroutine(_lengthCoroutine);
				_lengthCoroutine = null;
			}
		}

		private IEnumerator RemoveLengthCoroutine()
		{
			_isRightArmRemovingLength = true;
			if (_grabArmController.GrabController.CurrentGrabState == GrabState.Thrown)
			{
				_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			}
			while (true)
			{
				_ropeLengthController.RemoveLength();
				yield return new WaitForFixedUpdate();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2044271651u)]
		private void ThrowArmRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2044271651u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::ThrowArmRpc()", invokeInfo, PlayerRef.None);
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
			_throwCoroutine = StartCoroutine(ThrowArmCoroutine());
			if (_armEndController != null)
			{
				_armEndController.AddSynchronizationReason(ArmEndSyncReason.Throwing);
			}
		}

		private void SnapArmStart()
		{
			if (!(base.Object == null))
			{
				_startTransform = _armStartsModel.GetStart(base.Object.InputAuthority, ArmOrientation);
				SetArmStart(_armStartsModel.GetStart(base.Object.InputAuthority, ArmOrientation));
				_startArmSnapAttachment.Snap();
				_armStartSetted = true;
			}
		}

		private void SnapArmEnd()
		{
			if (!(base.Object == null))
			{
				_armEndSetted = true;
				_endPositionNetworkRigidbody = _endPositionRigidbody.GetComponent<NetworkRigidbody>();
				_grabArmController = _endPositionRigidbody.GetComponent<GrabArmController>();
				_grabArmController.OnHited += StopThrow;
				_armEndController = _endPositionRigidbody.GetComponent<ArmEndController>();
				SetArmEnd(_endPositionRigidbody.transform);
				_endArmSnapAttachment.Snap();
			}
		}

		private void StopThrow()
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				StopThrow_RPC();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1490015940u)]
		private void StopThrow_RPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1490015940u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::StopThrow_RPC()", invokeInfo, PlayerRef.None);
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
			if (_throwCoroutine != null)
			{
				StopCoroutine(_throwCoroutine);
				_endPositionRigidbody.linearVelocity = Vector3.zero;
				_endPositionRigidbody.useGravity = true;
				_grabArmController._isThrowing = false;
				if (_grabArmController.GrabController.CurrentGrabState == GrabState.GrabStatic)
				{
					_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
				}
				else if (base.Object.HasStateAuthority)
				{
					_endPositionRigidbody.isKinematic = false;
				}
				_isArmThrowing = false;
				_throwCoroutine = null;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(_hitPoint, 0.2f);
		}

		private IEnumerator ThrowArmCoroutine()
		{
			if (inIdlePose)
			{
				ResetIdlePose();
			}
			yield return null;
			_grabArmController._isThrowing = true;
			_isArmThrowing = true;
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
			Vector3 startPos = _idlePoseForArmModel.GetIdlePose(ArmOrientation, base.Object.InputAuthority).position;
			Vector3 targetPoint;
			if (Physics.Raycast(new Ray(_cameraModel.AimTransform.position, _cameraModel.AimTransform.forward), out var hitInfo, 1000f, _raycastLayerMask))
			{
				_hitPoint = hitInfo.point;
				targetPoint = hitInfo.point;
				Debug.LogError("Hitted " + hitInfo.transform.gameObject.name, hitInfo.transform.gameObject);
			}
			else
			{
				targetPoint = _cameraModel.AimTransform.position + _cameraModel.AimTransform.forward * _throwDistance;
			}
			if (base.Object.HasStateAuthority)
			{
				_endPositionRigidbody.isKinematic = true;
			}
			_endPositionRigidbody.useGravity = false;
			_endPositionRigidbody.transform.position = startPos;
			yield return null;
			_endPositionRigidbody.transform.position = startPos;
			float elapsed = 0f;
			float num = _throwDistance / _throwDuration;
			float timeToTarget = Vector3.Distance(startPos, targetPoint) / num;
			while (elapsed < timeToTarget)
			{
				elapsed += Time.fixedDeltaTime;
				float t = Mathf.Clamp01(elapsed / timeToTarget);
				Vector3 position = Vector3.Lerp(startPos, targetPoint, t);
				_endPositionRigidbody.MovePosition(position);
				_ropeLengthController.AddLength();
				yield return new WaitForFixedUpdate();
			}
			_endPositionRigidbody.MovePosition(targetPoint);
			if (base.Object.HasStateAuthority)
			{
				_endPositionRigidbody.isKinematic = false;
			}
			_endPositionRigidbody.useGravity = true;
			if (_armStateModel.GetArmState(ArmOrientation) != ArmState.Grabbing)
			{
				_grabArmController.TryGrab();
			}
			if (_grabArmController.GrabController.CurrentGrabState == GrabState.GrabStatic)
			{
				_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
			}
			_grabArmController._isThrowing = false;
			_isArmThrowing = false;
			_throwCoroutine = null;
		}

		public void TearOffArm()
		{
			TearOffArmRpc();
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 373913288u)]
		private void TearOffArmRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(373913288u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.StrechArmController::TearOffArmRpc()", invokeInfo, PlayerRef.None);
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
			_particleStartAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
			_particleStartAttachment.breakThreshold = 0f;
			_isTornOff = true;
			_grabArmController.UngrabAll();
			IArmEndEntity end = _armStartsModel.GetEnd(base.Object.StateAuthority, ArmOrientation);
			if (end != null && end.Transform.TryGetComponent<ArmEndController>(out var component))
			{
				component.AddSynchronizationReason(ArmEndSyncReason.TornOff);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ArmOrientation = _ArmOrientation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ArmOrientation = ArmOrientation;
		}

		[NetworkRpcWeavedInvoker(418659427u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RegisterStretchArmControllerRpc_0040Invoker418659427([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).RegisterStretchArmControllerRpc();
		}

		[NetworkRpcWeavedInvoker(979024373u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StartRemoveLengthRpc_0040Invoker979024373([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).StartRemoveLengthRpc();
		}

		[NetworkRpcWeavedInvoker(503031437u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StopRemoveLengthRpc_0040Invoker503031437([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).StopRemoveLengthRpc();
		}

		[NetworkRpcWeavedInvoker(2044271651u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ThrowArmRpc_0040Invoker2044271651([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).ThrowArmRpc();
		}

		[NetworkRpcWeavedInvoker(1490015940u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StopThrow_RPC_0040Invoker1490015940([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).StopThrow_RPC();
		}

		[NetworkRpcWeavedInvoker(373913288u)]
		[Preserve]
		[WeaverGenerated]
		protected static void TearOffArmRpc_0040Invoker373913288([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StrechArmController)context.TargetBehaviour).TearOffArmRpc();
		}
	}
}
