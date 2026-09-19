using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class AgentDoorPushModifier : MonoBehaviour
	{
		[SerializeField]
		private float _openSpeed = 90f;

		[SerializeField]
		private float _openForce = 25f;

		[SerializeField]
		private float _openDuration = 2f;

		public float OpenSpeed => _openSpeed;

		public float OpenForce => _openForce;

		public float OpenDuration => _openDuration;
	}
}
