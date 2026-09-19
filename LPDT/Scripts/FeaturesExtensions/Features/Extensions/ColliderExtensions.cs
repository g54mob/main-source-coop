using UnityEngine;

namespace Features.Extensions
{
	public static class ColliderExtensions
	{
		public static Vector3 CapsuleTop(this CapsuleCollider capsuleCollider)
		{
			Vector3 vector = capsuleCollider.transform.TransformPoint(capsuleCollider.center);
			Vector3 up = capsuleCollider.transform.up;
			return vector + up * (capsuleCollider.height / 2f - capsuleCollider.radius);
		}

		public static Vector3 CapsuleBottom(this CapsuleCollider capsuleCollider)
		{
			Vector3 vector = capsuleCollider.transform.TransformPoint(capsuleCollider.center);
			Vector3 up = capsuleCollider.transform.up;
			return vector - up * (capsuleCollider.height / 2f - capsuleCollider.radius);
		}
	}
}
