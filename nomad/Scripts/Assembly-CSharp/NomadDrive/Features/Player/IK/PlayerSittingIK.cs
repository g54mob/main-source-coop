using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using EvilCore.Networking;
using EvilCore.Networking.Parenting;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Vehicle;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Features.Vehicle.Parts.SteeringWheel;
using RootMotion.FinalIK;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.IK
{
	[DefaultExecutionOrder(-10)]
	public class PlayerSittingIK : NetworkBehaviour, IPlayerComponent
	{
		[SerializeField]
		private FullBodyBipedIK fbbik;

		[SerializeField]
		private InteractionSystem interactionSystem;

		[SerializeField]
		private HandPoser leftHandPoser;

		[SerializeField]
		private HandPoser rightHandPoser;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private INetworkObjectSpawnWatcher _spawnWatcher;

		[SyncVar(hook = "OnSeatKeyChanged")]
		private ulong _seatKey;

		private Player _player;

		private bool _isSubscribed;

		private bool _isSitting;

		private bool _isDriverSeat;

		private bool _isLocalPlayer;

		private bool _isLateJoinCompleted;

		private VehicleSteeringWheelModule _subscribedSteeringModule;

		private Transform _leftHandTarget;

		private Transform _rightHandTarget;

		private Transform _leftFootTarget;

		private Transform _rightFootTarget;

		private Transform _leftElbowBendGoal;

		private Transform _rightElbowBendGoal;

		private Transform _leftKneeBendGoal;

		private Transform _rightKneeBendGoal;

		private Transform _leftHandPoseRoot;

		private Transform _rightHandPoseRoot;

		public Action<ulong, ulong> _Mirror_SyncVarHookDelegate__seatKey;

		public int SetupPriority => 26;

		public bool RequiresRemoteSolvers
		{
			get
			{
				if (_isSitting)
				{
					return _isDriverSeat;
				}
				return false;
			}
		}

		public ulong Network_seatKey
		{
			get
			{
				return _seatKey;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _seatKey, 1uL, _Mirror_SyncVarHookDelegate__seatKey);
			}
		}

		private static ulong PackSeat(uint netId, byte index)
		{
			return ((ulong)netId << 8) | index;
		}

		private static uint UnpackNetId(ulong key)
		{
			return (uint)(key >> 8);
		}

		private static byte UnpackIndex(ulong key)
		{
			return (byte)(key & 0xFF);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_isLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				SetForLateJoinerAsync().Forget();
			}
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			_player = GetComponent<Player>();
			if (_isLocalPlayer && _player != null && fbbik != null)
			{
				_player.OnPlayerSit.AddListener(HandleSit);
				_player.OnPlayerStand.AddListener(HandleStand);
				_isSubscribed = true;
			}
			base.enabled = true;
		}

		private void OnDestroy()
		{
			_spawnWatcher?.UnregisterPendingLookup(UnpackNetId(_seatKey), OnSeatSpawnedLate);
			UnsubscribeSteeringWheelEvents();
			if (!_isLocalPlayer)
			{
				SetRemoteSolversEnabled(isEnabled: false);
			}
			if (_isSubscribed && !(_player == null))
			{
				_player.OnPlayerSit.RemoveListener(HandleSit);
				_player.OnPlayerStand.RemoveListener(HandleStand);
			}
		}

		private async UniTaskVoid SetForLateJoinerAsync()
		{
			if (_seatKey == 0L)
			{
				_isLateJoinCompleted = true;
				return;
			}
			if (_networkManager == null)
			{
				_isLateJoinCompleted = true;
				return;
			}
			uint targetSeatNetId = UnpackNetId(_seatKey);
			(bool, GameObject) obj = await _networkManager.TryGetNetworkObjectByIdWithBackoffAsync(targetSeatNetId, 30, 100, 2000, 1.5f, this.GetCancellationTokenOnDestroy());
			_isLateJoinCompleted = true;
			if (obj.Item1)
			{
				ApplyFromSeatKey(_seatKey);
			}
			else if (_spawnWatcher != null)
			{
				_spawnWatcher.RegisterPendingLookup(targetSeatNetId, OnSeatSpawnedLate, "PlayerSittingIK.LateJoinerSeat");
			}
		}

		private void OnSeatSpawnedLate(GameObject seatObject)
		{
			if (!(seatObject == null) && !(this == null) && _seatKey != 0L)
			{
				ApplyFromSeatKey(_seatKey);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetSeat(ulong seatKey)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarULong(seatKey);
			SendCommandInternal("System.Void NomadDrive.Features.Player.IK.PlayerSittingIK::CmdSetSeat(System.UInt64)", -360374123, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void HandleSit(ISittable seat)
		{
			if (seat == null)
			{
				CmdSetSeat(0uL);
				return;
			}
			byte index = (byte)((seat.NetworkedTransform != null) ? seat.NetworkedTransform.NetworkedTransformIndex : 0);
			CmdSetSeat(PackSeat(seat.NetId, index));
		}

		private void HandleStand()
		{
			CmdSetSeat(0uL);
		}

		public void ReleaseSeatedIK()
		{
			ApplyFromSeatKey(0uL);
		}

		public void ClearSeatedIKNetworked()
		{
			CmdSetSeat(0uL);
		}

		private void OnSeatKeyChanged(ulong oldKey, ulong newKey)
		{
			if (_isLateJoinCompleted)
			{
				ApplyFromSeatKey(newKey);
			}
		}

		private void ApplyFromSeatKey(ulong seatKey)
		{
			uint num = UnpackNetId(seatKey);
			byte seatIndex = UnpackIndex(seatKey);
			UnsubscribeSteeringWheelEvents();
			ResetIK();
			ClearActiveTargets();
			_isSitting = false;
			_isDriverSeat = false;
			if (!_isLocalPlayer)
			{
				SetRemoteSolversEnabled(isEnabled: false);
			}
			if (num == 0 || fbbik == null || _networkManager == null)
			{
				return;
			}
			if (!_networkManager.TryGetNetworkObjectById(num, out var networkObject) || networkObject == null)
			{
				if (_spawnWatcher != null)
				{
					_spawnWatcher.RegisterPendingLookup(num, OnSeatSpawnedLate, "PlayerSittingIK.SeatHookPending");
				}
				return;
			}
			ISittable sittable = ResolveSittable(networkObject, seatIndex);
			if (sittable == null)
			{
				return;
			}
			_leftKneeBendGoal = sittable.LeftKneeBendGoal;
			_rightKneeBendGoal = sittable.RightKneeBendGoal;
			_leftElbowBendGoal = sittable.LeftElbowBendGoal;
			_rightElbowBendGoal = sittable.RightElbowBendGoal;
			if (sittable.IsDriverSeat && sittable.VehicleSeatSlot != null)
			{
				_isDriverSeat = true;
				BuildDriverSeatTargets(sittable);
				SubscribeSteeringWheelEvents(sittable);
				if (!_isLocalPlayer)
				{
					SetRemoteSolversEnabled(isEnabled: true);
				}
			}
			else
			{
				BuildPassengerSeatTargets(sittable);
			}
			_isSitting = true;
			CancelLingeringInteractions();
			ApplyAllTargets();
		}

		private static ISittable ResolveSittable(GameObject root, byte seatIndex)
		{
			ISittable[] componentsInChildren = root.GetComponentsInChildren<ISittable>(includeInactive: true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return null;
			}
			ISittable[] array = componentsInChildren;
			foreach (ISittable sittable in array)
			{
				if (sittable != null)
				{
					NetworkedTransform networkedTransform = sittable.NetworkedTransform;
					if (networkedTransform != null && networkedTransform.NetworkedTransformIndex == seatIndex)
					{
						return sittable;
					}
				}
			}
			if (componentsInChildren.Length != 1)
			{
				return null;
			}
			return componentsInChildren[0];
		}

		private void SetRemoteSolversEnabled(bool isEnabled)
		{
			if (fbbik != null)
			{
				fbbik.enabled = isEnabled;
			}
			if (interactionSystem != null)
			{
				interactionSystem.enabled = isEnabled;
			}
			if (leftHandPoser != null)
			{
				leftHandPoser.enabled = isEnabled;
			}
			if (rightHandPoser != null)
			{
				rightHandPoser.enabled = isEnabled;
			}
		}

		private void SubscribeSteeringWheelEvents(ISittable seat)
		{
			if (seat.VehicleSeatSlot == null)
			{
				return;
			}
			VehicleManager vehicleManager = seat.VehicleSeatSlot.VehicleManager;
			if (!(vehicleManager == null))
			{
				VehicleSteeringWheelModule module = vehicleManager.GetModule<VehicleSteeringWheelModule>();
				if (!(module == null) && !(module.SteeringWheelSlotRef == null))
				{
					_subscribedSteeringModule = module;
					module.SteeringWheelSlotRef.OnSteeringWheelAttached.AddListener(OnSteeringWheelAttached);
					module.SteeringWheelSlotRef.OnSteeringWheelDetached.AddListener(OnSteeringWheelDetached);
				}
			}
		}

		private void UnsubscribeSteeringWheelEvents()
		{
			if (!(_subscribedSteeringModule == null))
			{
				if (_subscribedSteeringModule.SteeringWheelSlotRef != null)
				{
					_subscribedSteeringModule.SteeringWheelSlotRef.OnSteeringWheelAttached.RemoveListener(OnSteeringWheelAttached);
					_subscribedSteeringModule.SteeringWheelSlotRef.OnSteeringWheelDetached.RemoveListener(OnSteeringWheelDetached);
				}
				_subscribedSteeringModule = null;
			}
		}

		private void OnSteeringWheelAttached(SteeringWheel sw)
		{
			if (_isSitting && !(sw == null) && !(fbbik == null))
			{
				_leftHandTarget = sw.LeftHandRig;
				_rightHandTarget = sw.RightHandRig;
				_leftHandPoseRoot = sw.LeftHandRig;
				_rightHandPoseRoot = sw.RightHandRig;
				ApplyAllTargets();
			}
		}

		private void OnSteeringWheelDetached(SteeringWheel sw)
		{
			if (_isSitting)
			{
				_leftHandTarget = null;
				_rightHandTarget = null;
				_leftHandPoseRoot = null;
				_rightHandPoseRoot = null;
				if (!(fbbik == null))
				{
					IKSolverFullBodyBiped solver = fbbik.solver;
					ResetEffector(solver.leftHandEffector);
					ResetEffector(solver.rightHandEffector);
					ResetHandPoser(leftHandPoser);
					ResetHandPoser(rightHandPoser);
				}
			}
		}

		private void BuildPassengerSeatTargets(ISittable seat)
		{
			_leftHandTarget = seat.LeftHandTarget;
			_rightHandTarget = seat.RightHandTarget;
			_leftFootTarget = seat.LeftFootTarget;
			_rightFootTarget = seat.RightFootTarget;
			_leftHandPoseRoot = seat.LeftHandTarget;
			_rightHandPoseRoot = seat.RightHandTarget;
		}

		private void BuildDriverSeatTargets(ISittable seat)
		{
			VehicleManager vehicleManager = ((seat.VehicleSeatSlot != null) ? seat.VehicleSeatSlot.VehicleManager : null);
			if (!(vehicleManager == null))
			{
				VehiclePedalModule module = vehicleManager.GetModule<VehiclePedalModule>();
				if (module != null)
				{
					_leftFootTarget = module.LeftFootTarget;
					_rightFootTarget = module.RightFootTarget;
				}
				if (_leftFootTarget == null)
				{
					_leftFootTarget = seat.LeftFootTarget;
				}
				VehicleSteeringWheelModule module2 = vehicleManager.GetModule<VehicleSteeringWheelModule>();
				if (module2 != null && module2.InstalledSteeringWheel != null)
				{
					SteeringWheel installedSteeringWheel = module2.InstalledSteeringWheel;
					_leftHandTarget = installedSteeringWheel.LeftHandRig;
					_rightHandTarget = installedSteeringWheel.RightHandRig;
					_leftHandPoseRoot = installedSteeringWheel.LeftHandRig;
					_rightHandPoseRoot = installedSteeringWheel.RightHandRig;
				}
			}
		}

		private void CancelLingeringInteractions()
		{
			if (!(interactionSystem == null))
			{
				interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
				interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
				interactionSystem.StopInteraction(FullBodyBipedEffector.LeftFoot);
				interactionSystem.StopInteraction(FullBodyBipedEffector.RightFoot);
			}
		}

		private void LateUpdate()
		{
			if (_isSitting && !(fbbik == null))
			{
				ApplyAllTargets();
			}
		}

		private void ApplyAllTargets()
		{
			IKSolverFullBodyBiped solver = fbbik.solver;
			ApplyEffectorIfFree(solver.leftHandEffector, FullBodyBipedEffector.LeftHand, _leftHandTarget);
			ApplyEffectorIfFree(solver.rightHandEffector, FullBodyBipedEffector.RightHand, _rightHandTarget);
			ApplyEffectorIfFree(solver.leftFootEffector, FullBodyBipedEffector.LeftFoot, _leftFootTarget);
			ApplyEffectorIfFree(solver.rightFootEffector, FullBodyBipedEffector.RightFoot, _rightFootTarget);
			ApplyBendGoal(solver, FullBodyBipedChain.LeftArm, _leftElbowBendGoal);
			ApplyBendGoal(solver, FullBodyBipedChain.RightArm, _rightElbowBendGoal);
			ApplyBendGoal(solver, FullBodyBipedChain.LeftLeg, _leftKneeBendGoal);
			ApplyBendGoal(solver, FullBodyBipedChain.RightLeg, _rightKneeBendGoal);
			ApplyHandPoserIfFree(leftHandPoser, FullBodyBipedEffector.LeftHand, _leftHandPoseRoot);
			ApplyHandPoserIfFree(rightHandPoser, FullBodyBipedEffector.RightHand, _rightHandPoseRoot);
		}

		private void ApplyEffectorIfFree(IKEffector effector, FullBodyBipedEffector type, Transform target)
		{
			if (!(target == null) && (!(interactionSystem != null) || !interactionSystem.IsInInteraction(type)))
			{
				effector.target = target;
				effector.positionWeight = 1f;
				effector.rotationWeight = 1f;
			}
		}

		private void ApplyHandPoserIfFree(HandPoser poser, FullBodyBipedEffector type, Transform poseRoot)
		{
			if (!(poser == null) && !(poseRoot == null) && (!(interactionSystem != null) || !interactionSystem.IsInInteraction(type)))
			{
				poser.poseRoot = poseRoot;
				poser.weight = 1f;
			}
		}

		private void ResetIK()
		{
			if (!(fbbik == null))
			{
				IKSolverFullBodyBiped solver = fbbik.solver;
				ResetEffector(solver.leftHandEffector);
				ResetEffector(solver.rightHandEffector);
				ResetEffector(solver.leftFootEffector);
				ResetEffector(solver.rightFootEffector);
				ResetBendGoal(solver, FullBodyBipedChain.LeftArm);
				ResetBendGoal(solver, FullBodyBipedChain.RightArm);
				ResetBendGoal(solver, FullBodyBipedChain.LeftLeg);
				ResetBendGoal(solver, FullBodyBipedChain.RightLeg);
				ResetHandPoser(leftHandPoser);
				ResetHandPoser(rightHandPoser);
			}
		}

		private void ClearActiveTargets()
		{
			_leftHandTarget = null;
			_rightHandTarget = null;
			_leftFootTarget = null;
			_rightFootTarget = null;
			_leftElbowBendGoal = null;
			_rightElbowBendGoal = null;
			_leftKneeBendGoal = null;
			_rightKneeBendGoal = null;
			_leftHandPoseRoot = null;
			_rightHandPoseRoot = null;
		}

		private static void ResetEffector(IKEffector effector)
		{
			effector.target = null;
			effector.positionWeight = 0f;
			effector.rotationWeight = 0f;
		}

		private static void ApplyBendGoal(IKSolverFullBodyBiped solver, FullBodyBipedChain chain, Transform target)
		{
			if (!(target == null))
			{
				IKConstraintBend bendConstraint = solver.GetBendConstraint(chain);
				bendConstraint.bendGoal = target;
				bendConstraint.weight = 1f;
			}
		}

		private static void ResetBendGoal(IKSolverFullBodyBiped solver, FullBodyBipedChain chain)
		{
			IKConstraintBend bendConstraint = solver.GetBendConstraint(chain);
			bendConstraint.bendGoal = null;
			bendConstraint.weight = 0f;
		}

		private static void ResetHandPoser(HandPoser poser)
		{
			if (!(poser == null))
			{
				poser.poseRoot = null;
				poser.weight = 0f;
			}
		}

		public PlayerSittingIK()
		{
			_Mirror_SyncVarHookDelegate__seatKey = OnSeatKeyChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetSeat__UInt64(ulong seatKey)
		{
			Network_seatKey = seatKey;
		}

		protected static void InvokeUserCode_CmdSetSeat__UInt64(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetSeat called on client.");
			}
			else
			{
				((PlayerSittingIK)obj).UserCode_CmdSetSeat__UInt64(reader.ReadVarULong());
			}
		}

		static PlayerSittingIK()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerSittingIK), "System.Void NomadDrive.Features.Player.IK.PlayerSittingIK::CmdSetSeat(System.UInt64)", InvokeUserCode_CmdSetSeat__UInt64, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarULong(_seatKey);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarULong(_seatKey);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _seatKey, _Mirror_SyncVarHookDelegate__seatKey, reader.ReadVarULong());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _seatKey, _Mirror_SyncVarHookDelegate__seatKey, reader.ReadVarULong());
			}
		}
	}
}
