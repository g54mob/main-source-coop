using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public interface IKrakenItemThrowService
	{
		Vector3 CalculateThrowVelocity(Vector3 startPosition, Vector3 targetPosition, float minimumDuration, float horizontalSpeed, out float duration);

		Vector3 EvaluateBallisticPosition(Vector3 startPosition, Vector3 throwVelocity, float elapsed);

		void ThrowRigidbody(Rigidbody rigidbody, Vector3 targetPosition, float minimumDuration, float horizontalSpeed);
	}
}
