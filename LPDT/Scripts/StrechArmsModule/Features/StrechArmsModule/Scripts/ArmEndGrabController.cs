using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.LineArmModule.Scripts;
using Features.PhysicsUtilsModule.Scripts;
using Features.PhysicsVolumeModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ArmEndGrabController : NetworkBehaviour
	{
		[SerializeField]
		private CopyRotationFromObject _copyRotationFromObject;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private ArmEndController _armEndController;

		[SerializeField]
		private float _tearDistance = 2f;

		[SerializeField]
		private List<Collider> _armEndColliders;

		[SerializeField]
		private float _linearDampingOnPhysics = 1f;

		[SerializeField]
		private LayerMask _groundLayerMask;

		private const float DECK_BREACH_MARGIN = 0.25f;

		private const float RECOVERED_ARM_REACH = 0.45f;

		private const float BODY_ON_DECK_MAX_HEIGHT = 3f;

		private bool _isStateRequested;

		private bool _authorityTransferred;

		private LineArmsModel _lineArmsModel;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerRagdollEntity _playerRagdollEntity;

		private IPhysicsOverlapService _physicsOverlapService;

		private bool _isPhysicsEnabled;

		private float _initialLinearDampingOnPhysics;

		[Inject]
		private void InjectDependencies(LineArmsModel lineArmsModel, PlayersRagdollModel playersRagdollModel, IPhysicsOverlapService physicsOverlapService)
		{
			_lineArmsModel = lineArmsModel;
			_playersRagdollModel = playersRagdollModel;
			_physicsOverlapService = physicsOverlapService;
		}

		public override void Spawned()
		{
			if (_playersRagdollModel.PlayersRagdoll.TryGetValue(base.Object.StateAuthority.PlayerId, out var value))
			{
				InitializeRagdoll(base.Object.StateAuthority.PlayerId, value);
			}
			else
			{
				_playersRagdollModel.OnPlayerRagdollAdded += InitializeRagdoll;
			}
			_simplePointGrabable.LocalOnGrab += OnGrabbed;
			_simplePointGrabable.LocalOnUnGrab += OnUnGrabbed;
			if (!(base.Object.StateAuthority != base.Runner.LocalPlayer) && _armEndController.ArmOrientation == Arm.Right)
			{
				_simplePointGrabable.LocalGrabBlocked = true;
				_initialLinearDampingOnPhysics = _rigidbody.linearDamping;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_simplePointGrabable != null)
			{
				_simplePointGrabable.LocalOnGrab -= OnGrabbed;
				_simplePointGrabable.LocalOnUnGrab -= OnUnGrabbed;
			}
			if (_playersRagdollModel != null)
			{
				_playersRagdollModel.OnPlayerRagdollAdded -= InitializeRagdoll;
				if (_playerRagdollEntity != null)
				{
					_playerRagdollEntity.OnSimulationStarted -= EnableArmEndPhysics;
					_playerRagdollEntity.OnSimulationStopped -= DisableArmEndPhysics;
					_playerRagdollEntity.OnTeleported -= TeleportArmEnd;
					_playerRagdollEntity.OnSimulateHandsRagdollChanged -= OnHandsRagdollChanged;
				}
			}
		}

		private void OnGrabbed(int i)
		{
			_armEndController.AddSynchronizationReason(ArmEndSyncReason.Grabbed);
		}

		private void OnUnGrabbed()
		{
			_armEndController.RemoveSynchronizationReason(ArmEndSyncReason.Grabbed);
		}

		private void InitializeRagdoll(int playerId, PlayerRagdollEntity playerRagdollEntity)
		{
			if (base.Object.StateAuthority.PlayerId == playerId)
			{
				_playerRagdollEntity = playerRagdollEntity;
				_playerRagdollEntity.OnSimulationStarted += EnableArmEndPhysics;
				_playerRagdollEntity.OnSimulationStopped += DisableArmEndPhysics;
				_playerRagdollEntity.OnTeleported += TeleportArmEnd;
				_playerRagdollEntity.OnSimulateHandsRagdollChanged += OnHandsRagdollChanged;
				ReconcileArmEndPhysics();
			}
		}

		private void Update()
		{
			if (base.Object == null)
			{
				return;
			}
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(_rigidbody.position, Vector3.down, out hitInfo, 10f, _groundLayerMask);
			foreach (Collider armEndCollider in _armEndColliders)
			{
				if (armEndCollider != null)
				{
					armEndCollider.isTrigger = !flag || !_isPhysicsEnabled;
				}
			}
			KeepArmEndAboveCarrierDeck();
			if (_simplePointGrabable == null)
			{
				return;
			}
			bool flag2 = _simplePointGrabable.GrabbedByPlayers.Count > 0;
			bool flag3 = base.Object.StateAuthority == base.Object.InputAuthority;
			if (_copyRotationFromObject.Object != null && _copyRotationFromObject.HasStateAuthority)
			{
				_copyRotationFromObject.DisableRotation = flag2;
			}
			if (_isPhysicsEnabled)
			{
				_rigidbody.isKinematic = false;
			}
			else
			{
				_rigidbody.isKinematic = !flag2;
			}
			ProcessBlockGrab(flag2);
			if (flag2)
			{
				foreach (PhysGrabber grabber in _simplePointGrabable.GrabObject.Grabbers)
				{
					if (!(Vector3.Distance(grabber.GrabberTransform.position, base.transform.position) <= _tearDistance) && TryGetLineArm(grabber.PlayerId, out var lineArm) && lineArm.CurrentGrabbables.Count > 0)
					{
						lineArm.UnJoinAll(throwItem: false);
						break;
					}
				}
			}
			if (flag2 && !flag3)
			{
				if (TryGetLineArm(base.Object.InputAuthority.PlayerId, out var lineArm2) && lineArm2.CurrentGrabbables.Count > 0 && _armEndController.ArmOrientation == Arm.Right)
				{
					lineArm2.UnJoinAll(throwItem: false);
				}
				_authorityTransferred = true;
				return;
			}
			if (flag2 && flag3 && !_isStateRequested)
			{
				List<PhysGrabber> grabbers = _simplePointGrabable.GrabObject.Grabbers;
				if (grabbers.Count > 0)
				{
					PhysGrabber physGrabber = grabbers[0];
					if (physGrabber.PlayerId != base.Object.InputAuthority.PlayerId)
					{
						_isStateRequested = true;
						_simplePointGrabable.RequestStateAuthorityRPC(physGrabber.PlayerId);
						return;
					}
				}
			}
			if (!flag2 && !flag3 && _authorityTransferred && !_isStateRequested)
			{
				_isStateRequested = true;
				_simplePointGrabable.RequestStateAuthorityRPC(base.Object.InputAuthority.PlayerId);
			}
			if (flag3)
			{
				_authorityTransferred = false;
				_isStateRequested = false;
			}
		}

		private bool TryGetLineArm(int playerId, out LineArmControllerBase lineArm)
		{
			return _lineArmsModel.TryGetLineArmForPlayer(playerId, out lineArm);
		}

		private void ProcessBlockGrab(bool isGrabbed)
		{
			if (_lineArmsModel.GetAllLineArmsForPlayer(base.Object.InputAuthority.PlayerId) != null && _lineArmsModel.GetAllLineArmsForPlayer(base.Object.InputAuthority.PlayerId).ContainsKey(LineArmType.RightArmDefault))
			{
				LineArmControllerBase lineArmControllerBase = _lineArmsModel.GetAllLineArmsForPlayer(base.Object.InputAuthority.PlayerId)[LineArmType.RightArmDefault];
				if (lineArmControllerBase != null)
				{
					lineArmControllerBase.EnableGrabbing(!isGrabbed);
				}
			}
		}

		private void KeepArmEndAboveCarrierDeck()
		{
			if (!base.HasStateAuthority || _rigidbody == null || _playerRagdollEntity == null)
			{
				return;
			}
			Rigidbody rigidBody = _playerRagdollEntity.RootPhysData.RigidBody;
			if (rigidBody == null)
			{
				return;
			}
			Vector3 position = rigidBody.position;
			PhysicsInfluenceVolume physicsInfluenceVolume = PhysicsInfluenceVolume.FindDeclaredFloorUnder(position, 0.25f, 3f);
			if (physicsInfluenceVolume == null)
			{
				return;
			}
			Vector3 position2 = _rigidbody.position;
			if (physicsInfluenceVolume.IsOffDeclaredFloor(position2, 0.25f))
			{
				Vector3 vector = position2 - position;
				vector -= Vector3.Project(vector, Vector3.up);
				vector = ((vector.sqrMagnitude > 0.0001f) ? vector.normalized : Vector3.forward);
				if (physicsInfluenceVolume.TryGetDeclaredFloorPoint(position + vector * 0.45f, out var floorPoint))
				{
					_rigidbody.position = floorPoint;
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
					_rigidbody.PublishTransform();
				}
			}
		}

		private void EnableArmEndPhysics(IRagdollEntity ragdollEntity)
		{
			if (_playerRagdollEntity.IsHandsPhysicsActive && !_isPhysicsEnabled)
			{
				_isPhysicsEnabled = true;
				_rigidbody.linearDamping = _linearDampingOnPhysics;
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = Vector3.zero;
				}
				_physicsOverlapService.ResolveOverlap(_armEndColliders[0], _groundLayerMask);
				_armEndController.AddSynchronizationReason(ArmEndSyncReason.Ragdoll);
			}
		}

		private void DisableArmEndPhysics(IRagdollEntity ragdollEntity)
		{
			if (!_playerRagdollEntity.IsHandsPhysicsActive && _isPhysicsEnabled)
			{
				_isPhysicsEnabled = false;
				_rigidbody.linearDamping = _initialLinearDampingOnPhysics;
				_armEndController.RemoveSynchronizationReason(ArmEndSyncReason.Ragdoll);
			}
		}

		private void TeleportArmEnd(Vector3 position, Vector3 rootPosition)
		{
			Vector3 vector = _rigidbody.position - rootPosition;
			_rigidbody.position = position + vector;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.PublishTransform();
		}

		private void OnHandsRagdollChanged(bool simulate)
		{
			ReconcileArmEndPhysics();
		}

		private void ReconcileArmEndPhysics()
		{
			if (_playerRagdollEntity.IsHandsPhysicsActive)
			{
				EnableArmEndPhysics(_playerRagdollEntity);
			}
			else
			{
				DisableArmEndPhysics(_playerRagdollEntity);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
