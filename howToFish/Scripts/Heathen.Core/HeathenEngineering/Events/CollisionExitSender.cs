using UnityEngine;

namespace HeathenEngineering.Events
{
	[AddComponentMenu("System Core/Events/Collision Exit Sender")]
	public class CollisionExitSender : MonoBehaviour
	{
		[Header("Game Event")]
		public CollisionGameEvent PhysicsEvent;

		[Header("Direct Event")]
		public UnityCollisionEvent ColliderExited;

		private void OnCollisionExit(Collision collision)
		{
			if (PhysicsEvent != null)
			{
				PhysicsEvent.Raise(collision);
			}
			ColliderExited.Invoke(collision);
		}
	}
}
