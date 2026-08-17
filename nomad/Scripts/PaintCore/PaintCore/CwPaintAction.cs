using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintAction")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Action")]
	public class CwPaintAction : MonoBehaviour, IHitPoint, IHit, IHitLine, IHitTriangle, IHitQuad
	{
		[SerializeField]
		public UnityEvent action;

		public UnityEvent Action
		{
			get
			{
				if (action == null)
				{
					action = new UnityEvent();
				}
				return action;
			}
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			if (action != null)
			{
				action.Invoke();
			}
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			if (action != null)
			{
				action.Invoke();
			}
		}

		public void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			if (action != null)
			{
				action.Invoke();
			}
		}

		public void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			if (action != null)
			{
				action.Invoke();
			}
		}
	}
}
