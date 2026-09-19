using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts.ChestGrab;
using Features.Movement.Scripts;
using Features.PhysicsInteractionModule.Scripts;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	public class ChestLidStandOpenController : MonoBehaviour, IStandUpBlockingResponder
	{
		[Header("References")]
		[SerializeField]
		private Rigidbody _lidRigidbody;

		[SerializeField]
		private SimplePointGrabable _lidGrabable;

		[SerializeField]
		private LidItemsAuthorityFollower _lidItemsAuthorityFollower;

		[Header("Impulse")]
		[SerializeField]
		private float _impulsePerMass = 2f;

		[SerializeField]
		private float _forwardBias = 0.35f;

		[Header("Open Guard")]
		[SerializeField]
		private Vector3 _topCheckHalfExtents = new Vector3(0.35f, 0.2f, 0.35f);

		[SerializeField]
		private float _topCheckUpOffset = 0.2f;

		[SerializeField]
		private LayerMask _playerCheckMask = -1;

		private readonly HashSet<int> _impulseSentForPlayerId = new HashSet<int>();

		private readonly Collider[] _topCheckHits = new Collider[16];

		public LidItemsAuthorityFollower LidItemsAuthorityFollower => _lidItemsAuthorityFollower;

		public void OnLocalPlayerStandUpBlocked(int inputAuthorityPlayerId, Vector3 playerWorldPosition, Collider standUpBlockingCollider)
		{
			if ((!TryGetComponent<IChestScreamerLidGate>(out var component) || !component.ShouldSuppressStandUpOpenImpulse) && OwnsStandUpBlocker(standUpBlockingCollider) && _lidGrabable.Runner.LocalPlayer.PlayerId == inputAuthorityPlayerId && !_impulseSentForPlayerId.Contains(inputAuthorityPlayerId))
			{
				_impulseSentForPlayerId.Add(inputAuthorityPlayerId);
				ApplyStandOpenImpulse(inputAuthorityPlayerId, playerWorldPosition);
			}
		}

		public void OnLocalPlayerStandUpNoLongerBlocked(int inputAuthorityPlayerId, Collider standUpBlockingCollider)
		{
			if (OwnsStandUpBlocker(standUpBlockingCollider))
			{
				_impulseSentForPlayerId.Remove(inputAuthorityPlayerId);
			}
		}

		private bool OwnsStandUpBlocker(Collider standUpBlockingCollider)
		{
			if (_lidGrabable == null || standUpBlockingCollider == null)
			{
				return false;
			}
			return standUpBlockingCollider.GetComponentInParent<SimplePointGrabable>() == _lidGrabable;
		}

		private void ApplyStandOpenImpulse(int requesterPlayerId, Vector3 playerPos)
		{
			if (!IsOtherPlayerOnLidTop(requesterPlayerId))
			{
				Transform nearestHandle = _lidGrabable.GetNearestHandle(playerPos);
				if (nearestHandle == null)
				{
					nearestHandle = _lidRigidbody.transform;
				}
				float num = Mathf.Max(0.0001f, _lidRigidbody.mass);
				Vector3 vector = Vector3.ProjectOnPlane(playerPos - _lidRigidbody.worldCenterOfMass, Vector3.up);
				if (vector.sqrMagnitude < 0.0001f)
				{
					vector = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
				}
				vector.Normalize();
				Vector3 normalized = (Vector3.up + vector * _forwardBias).normalized;
				float num2 = num * _impulsePerMass;
				Vector3 force = normalized * num2;
				_lidRigidbody.AddForceAtPosition(force, nearestHandle.position, ForceMode.Impulse);
			}
		}

		private bool IsOtherPlayerOnLidTop(int requesterPlayerId)
		{
			int num = Physics.OverlapBoxNonAlloc(_lidRigidbody.worldCenterOfMass + Vector3.up * _topCheckUpOffset, _topCheckHalfExtents, _topCheckHits, _lidRigidbody.rotation, _playerCheckMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _topCheckHits[i];
				if (!(collider == null))
				{
					PlayerCharacterMovableBase playerCharacterMovableBase = collider.GetComponent<PlayerCharacterMovableBase>() ?? collider.GetComponentInParent<PlayerCharacterMovableBase>();
					if (!(playerCharacterMovableBase == null) && !(playerCharacterMovableBase.Object == null) && playerCharacterMovableBase.Object.IsValid && playerCharacterMovableBase.Object.InputAuthority.PlayerId != requesterPlayerId)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
