using UnityEngine;

namespace Features.RumModule.Scripts
{
	public class RumHitData
	{
		public GameObject GameObject;

		public Vector3 RelativeVelocity;

		public RumHitData(GameObject gameObject, Vector3 relativeVelocity)
		{
			GameObject = gameObject;
			RelativeVelocity = relativeVelocity;
		}
	}
}
