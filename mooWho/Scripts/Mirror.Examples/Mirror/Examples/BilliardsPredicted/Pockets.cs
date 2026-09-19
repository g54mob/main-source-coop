using UnityEngine;

namespace Mirror.Examples.BilliardsPredicted
{
	public class Pockets : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (NetworkServer.active && PredictedRigidbody.IsPredicted(other, out var predictedRigidbody))
			{
				if (predictedRigidbody.TryGetComponent<WhiteBallPredicted>(out var component))
				{
					Rigidbody predictedRigidbody2 = predictedRigidbody.predictedRigidbody;
					predictedRigidbody2.position = component.startPosition;
					predictedRigidbody2.linearVelocity = Vector3.zero;
				}
				if ((bool)predictedRigidbody.GetComponent<RedBallPredicted>())
				{
					NetworkServer.Destroy(predictedRigidbody.gameObject);
				}
			}
		}
	}
}
