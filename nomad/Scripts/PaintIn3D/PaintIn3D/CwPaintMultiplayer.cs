using System.Collections;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwPaintMultiplayer")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Multiplayer")]
	public class CwPaintMultiplayer : MonoBehaviour, IHitPoint, IHit, IHitLine
	{
		[SerializeField]
		private float delay = 0.5f;

		public float Delay
		{
			get
			{
				return delay;
			}
			set
			{
				delay = value;
			}
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			if (position.x < 0f)
			{
				position.x += 100f;
			}
			else
			{
				position.x -= 100f;
			}
			StartCoroutine(SimulateNetworkTransmission(preview, priority, pressure, seed, position, rotation));
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			if (position.x < 0f)
			{
				position.x += 100f;
				endPosition.x += 100f;
			}
			else
			{
				position.x -= 100f;
				endPosition.x -= 100f;
			}
			StartCoroutine(SimulateNetworkTransmission(preview, priority, pressure, seed, position, endPosition, rotation, clip));
		}

		private IEnumerator SimulateNetworkTransmission(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			yield return new WaitForSecondsRealtime(delay);
			IHitPoint[] componentsInChildren = GetComponentsInChildren<IHitPoint>();
			foreach (IHitPoint hitPoint in componentsInChildren)
			{
				if ((Object)hitPoint != this)
				{
					hitPoint.HandleHitPoint(preview, priority, pressure, seed, position, rotation);
				}
			}
		}

		private IEnumerator SimulateNetworkTransmission(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			yield return new WaitForSecondsRealtime(delay);
			IHitLine[] componentsInChildren = GetComponentsInChildren<IHitLine>();
			foreach (IHitLine hitLine in componentsInChildren)
			{
				if ((Object)hitLine != this)
				{
					hitLine.HandleHitLine(preview, priority, pressure, seed, position, endPosition, rotation, clip);
				}
			}
		}
	}
}
