using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class CartPushController : NetworkBehaviour
	{
		[SerializeField]
		private PlayerInCartTrigger _playerInCartTrigger;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private float _targetSpeed = 2f;

		[SerializeField]
		private float _pushStrength = 10f;

		[SerializeField]
		private float _damping = 5f;

		[SerializeField]
		private float _maxSpeed = 2.5f;

		[SerializeField]
		private float _smoothLerpSpeed = 10f;

		[SerializeField]
		private float _minPlayerPushSpeed = 0.1f;

		private Vector3 _smoothDirection;

		private void OnCollisionStay(Collision other)
		{
			if (_grabable.GrabbedByPlayers.Count > 0 || (_layerMask.value & (1 << other.gameObject.layer)) == 0 || (_playerInCartTrigger != null && (!other.gameObject.TryGetComponent<NetworkObject>(out var component) || _playerInCartTrigger.PlayersInCase.Contains(component.InputAuthority.PlayerId))))
			{
				return;
			}
			Rigidbody rigidbody = _grabable.Rigidbody;
			Rigidbody rigidbody2 = other.rigidbody;
			if (rigidbody == null || rigidbody2 == null || rigidbody.isKinematic)
			{
				return;
			}
			Vector3 vector = rigidbody.worldCenterOfMass - rigidbody2.worldCenterOfMass;
			vector.y = 0f;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 0.0001f)
			{
				return;
			}
			vector /= Mathf.Sqrt(sqrMagnitude);
			Vector3 linearVelocity = rigidbody2.linearVelocity;
			linearVelocity.y = 0f;
			if (!(linearVelocity.sqrMagnitude < _minPlayerPushSpeed * _minPlayerPushSpeed) && !(Vector3.Dot(linearVelocity.normalized, vector) <= 0.1f))
			{
				_smoothDirection = Vector3.Lerp(_smoothDirection, vector, _smoothLerpSpeed * Time.fixedDeltaTime).normalized;
				Vector3 linearVelocity2 = rigidbody.linearVelocity;
				linearVelocity2.y = 0f;
				Vector3 vector2 = _smoothDirection * _targetSpeed - linearVelocity2;
				rigidbody.AddForce(vector2 * _pushStrength, ForceMode.Acceleration);
				Vector3 force = -linearVelocity2 * _damping;
				rigidbody.AddForce(force, ForceMode.Acceleration);
				Vector3 linearVelocity3 = rigidbody.linearVelocity;
				Vector3 vector3 = new Vector3(linearVelocity3.x, 0f, linearVelocity3.z);
				if (vector3.magnitude > _maxSpeed)
				{
					vector3 = vector3.normalized * _maxSpeed;
					rigidbody.linearVelocity = new Vector3(vector3.x, linearVelocity3.y, vector3.z);
				}
				Debug.DrawLine(rigidbody.worldCenterOfMass, rigidbody.worldCenterOfMass + _smoothDirection, Color.red);
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
