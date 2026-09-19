using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public class KrakenItemThrowService : IKrakenItemThrowService
	{
		public Vector3 CalculateThrowVelocity(Vector3 startPosition, Vector3 targetPosition, float minimumDuration, float horizontalSpeed, out float duration)
		{
			Vector3 vector = targetPosition - startPosition;
			float num = new Vector3(vector.x, 0f, vector.z).magnitude / Mathf.Max(0.01f, horizontalSpeed);
			duration = Mathf.Max(0.01f, minimumDuration, num);
			Vector3 result = new Vector3(vector.x / duration, 0f, vector.z / duration);
			result.y = (vector.y - 0.5f * Physics.gravity.y * duration * duration) / duration;
			return result;
		}

		public Vector3 EvaluateBallisticPosition(Vector3 startPosition, Vector3 throwVelocity, float elapsed)
		{
			return startPosition + throwVelocity * elapsed + 0.5f * Physics.gravity * elapsed * elapsed;
		}

		public void ThrowRigidbody(Rigidbody rigidbody, Vector3 targetPosition, float minimumDuration, float horizontalSpeed)
		{
			if (!(rigidbody == null))
			{
				float duration;
				Vector3 vector = CalculateThrowVelocity(rigidbody.position, targetPosition, minimumDuration, horizontalSpeed, out duration);
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.WakeUp();
				rigidbody.AddForce(vector * rigidbody.mass, ForceMode.Impulse);
			}
		}
	}
}
