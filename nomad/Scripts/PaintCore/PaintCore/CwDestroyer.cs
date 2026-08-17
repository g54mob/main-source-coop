using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwDestroyer")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Destroyer")]
	public class CwDestroyer : MonoBehaviour, IHitPoint, IHit, IHitLine, IHitQuad
	{
		[SerializeField]
		private GameObject target;

		public GameObject Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		[ContextMenu("Destroy Now")]
		public void DestroyNow()
		{
			Object.Destroy(base.gameObject);
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			DestroyNow();
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Quaternion rotation, bool clip)
		{
			DestroyNow();
		}

		public void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Vector3 positionD, Quaternion rotation, bool clip)
		{
			DestroyNow();
		}
	}
}
