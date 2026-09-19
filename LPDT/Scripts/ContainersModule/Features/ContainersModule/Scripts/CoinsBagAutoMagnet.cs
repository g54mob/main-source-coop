using System.Collections.Generic;
using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	public class CoinsBagAutoMagnet : MonoBehaviour
	{
		[Header("Configuration")]
		[SerializeField]
		private Transform _magnetPoint;

		[SerializeField]
		private float _moveSpeed = 5f;

		[SerializeField]
		private float _minDistance = 0.1f;

		[SerializeField]
		private float _finalImpulse = 2f;

		private readonly HashSet<Rigidbody> _coinsInTrigger = new HashSet<Rigidbody>();

		private void OnTriggerEnter(Collider other)
		{
			IGrabableBase grabableBase = null;
			Rigidbody rigidbody = null;
			if (other.TryGetComponent<CoinMarker>(out var component))
			{
				grabableBase = component.Grabbable;
				if (component.Grabbable.TryGetComponent<Rigidbody>(out var component2))
				{
					rigidbody = component2;
				}
			}
			if (other.TryGetComponent<IGrabableBase>(out var component3))
			{
				grabableBase = component3;
				if (other.TryGetComponent<Rigidbody>(out var component4))
				{
					rigidbody = component4;
				}
			}
			if (grabableBase != null && !(rigidbody == null) && !grabableBase.IsEnableToGrab)
			{
				UnjoinCoin(rigidbody.gameObject, grabableBase);
				_coinsInTrigger.Add(rigidbody);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<Rigidbody>(out var component))
			{
				_coinsInTrigger.Remove(component);
			}
		}

		private void FixedUpdate()
		{
			if (_magnetPoint == null)
			{
				return;
			}
			Vector3 position = _magnetPoint.position;
			float fixedDeltaTime = Time.fixedDeltaTime;
			List<Rigidbody> list = new List<Rigidbody>();
			foreach (Rigidbody item in _coinsInTrigger)
			{
				if (item == null)
				{
					list.Add(item);
					continue;
				}
				Vector3 position2 = item.position;
				Vector3 vector = position - position2;
				if (vector.magnitude <= _minDistance)
				{
					Vector3 normalized = (vector.normalized + Random.insideUnitSphere * 0.3f).normalized;
					item.AddForce(normalized * _finalImpulse, ForceMode.Impulse);
					list.Add(item);
				}
				else
				{
					Vector3 position3 = Vector3.MoveTowards(position2, position, _moveSpeed * fixedDeltaTime);
					item.MovePosition(position3);
					item.linearVelocity = Vector3.zero;
				}
			}
			foreach (Rigidbody item2 in list)
			{
				_coinsInTrigger.Remove(item2);
			}
		}

		private void UnjoinCoin(GameObject coinObject, IGrabableBase grabable)
		{
			SpringJoint component = coinObject.GetComponent<SpringJoint>();
			if (component != null && component.connectedBody != null)
			{
				grabable.Unjoin(component.connectedBody);
			}
			else
			{
				grabable.UnjoinAll();
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (!(_magnetPoint == null))
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(_magnetPoint.position, _minDistance);
			}
		}
	}
}
