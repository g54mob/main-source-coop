using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class MonkeyPorterCartFollower : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _followDistance = 0.8f;

		[SerializeField]
		private float _heightOffset;

		[Tooltip("Point on this cart where a carried item rests (position + rotation). Author it in the cart hierarchy.")]
		[SerializeField]
		private Transform _itemHoldPoint;

		private const float PROXY_ITEM_GLUE_DISTANCE = 0.6f;

		private MonkeyPorterEnemy _ownerMonkey;

		private NavMeshAgent _ownerAgent;

		private Transform _proxyCarriedItem;

		private NetworkId _proxyCarriedItemId;

		private Collider[] _solidColliders;

		private bool _areSolidCollidersEnabled;

		public float FollowDistance => _followDistance;

		public override void Spawned()
		{
			base.Spawned();
			SetupRigidbody();
			CacheSolidColliders();
		}

		public void StateAuthorityChanged()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				SetupRigidbody();
			}
		}

		private void CacheSolidColliders()
		{
			List<Collider> list = new List<Collider>();
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (!collider.isTrigger)
				{
					list.Add(collider);
				}
			}
			_solidColliders = list.ToArray();
		}

		private void SetSolidCollidersEnabled(bool isEnabled)
		{
			if (_solidColliders == null || _areSolidCollidersEnabled == isEnabled)
			{
				return;
			}
			_areSolidCollidersEnabled = isEnabled;
			Collider[] solidColliders = _solidColliders;
			foreach (Collider collider in solidColliders)
			{
				if (collider != null)
				{
					collider.enabled = isEnabled;
				}
			}
		}

		private void SetupRigidbody()
		{
			_rigidbody.isKinematic = true;
			_rigidbody.useGravity = false;
			_rigidbody.interpolation = RigidbodyInterpolation.None;
		}

		private void LateUpdate()
		{
			if (base.Object == null || !base.Object.IsValid)
			{
				return;
			}
			ResolveOwner();
			if (_ownerMonkey == null)
			{
				return;
			}
			SetSolidCollidersEnabled(_ownerMonkey.IsCartFollowSuspended);
			if (!base.HasStateAuthority)
			{
				RideCarriedItemOnProxy();
				return;
			}
			if (!_ownerMonkey.IsCartFollowSuspended)
			{
				PositionCart();
			}
			RideCarriedItem();
		}

		private void PositionCart()
		{
			Transform transform = _ownerMonkey.transform;
			Vector3 forward = transform.forward;
			forward.y = 0f;
			forward = ((forward.sqrMagnitude > 0.0001f) ? forward.normalized : Vector3.forward);
			float num = ((_ownerAgent != null) ? _ownerAgent.baseOffset : 0f);
			Vector3 position = transform.position + forward * _followDistance;
			position.y = transform.position.y - num + _heightOffset;
			Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
			_rigidbody.transform.SetPositionAndRotation(position, rotation);
		}

		private void RideCarriedItem()
		{
			Rigidbody carriedItemRigidbody = _ownerMonkey.CarriedItemRigidbody;
			if (!(carriedItemRigidbody == null))
			{
				if (!carriedItemRigidbody.isKinematic)
				{
					carriedItemRigidbody.isKinematic = true;
				}
				Transform transform = ((_itemHoldPoint != null) ? _itemHoldPoint : _rigidbody.transform);
				Transform carryHandSocket = _ownerMonkey.CarryHandSocket;
				if (carryHandSocket == null)
				{
					carriedItemRigidbody.transform.SetPositionAndRotation(transform.position, transform.rotation);
					return;
				}
				float takeProgress = _ownerMonkey.TakeProgress01;
				Vector3 position = Vector3.Lerp(carryHandSocket.position, transform.position, takeProgress);
				Quaternion rotation = Quaternion.Slerp(carryHandSocket.rotation, transform.rotation, takeProgress);
				carriedItemRigidbody.transform.SetPositionAndRotation(position, rotation);
			}
		}

		private void RideCarriedItemOnProxy()
		{
			Transform transform = ResolveProxyCarriedItem();
			if (!(transform == null))
			{
				Transform transform2 = ((_itemHoldPoint != null) ? _itemHoldPoint : _rigidbody.transform);
				if (!((transform.position - transform2.position).sqrMagnitude > 0.36f))
				{
					transform.SetPositionAndRotation(transform2.position, transform2.rotation);
				}
			}
		}

		private Transform ResolveProxyCarriedItem()
		{
			NetworkId carriedItemId = _ownerMonkey.CarriedItemId;
			if (carriedItemId == default(NetworkId) || base.Runner == null)
			{
				_proxyCarriedItem = null;
				_proxyCarriedItemId = default(NetworkId);
				return null;
			}
			if (_proxyCarriedItem != null && _proxyCarriedItemId == carriedItemId)
			{
				return _proxyCarriedItem;
			}
			NetworkObject networkObject = base.Runner.FindObject(carriedItemId);
			_proxyCarriedItem = ((networkObject != null) ? networkObject.transform : null);
			_proxyCarriedItemId = carriedItemId;
			return _proxyCarriedItem;
		}

		private void ResolveOwner()
		{
			if (_ownerMonkey != null && _ownerMonkey.Object != null && _ownerMonkey.Object.IsValid)
			{
				if (_ownerAgent == null)
				{
					_ownerMonkey.TryGetComponent<NavMeshAgent>(out _ownerAgent);
				}
			}
			else
			{
				if (base.Runner == null || !base.Object.IsValid)
				{
					return;
				}
				foreach (NetworkObject allNetworkObject in base.Runner.GetAllNetworkObjects())
				{
					if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.TryGetComponent<MonkeyPorterEnemy>(out var component) && !(component.CartId != base.Object.Id))
					{
						_ownerMonkey = component;
						_ownerMonkey.TryGetComponent<NavMeshAgent>(out _ownerAgent);
						break;
					}
				}
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
