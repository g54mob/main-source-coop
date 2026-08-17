using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitScreenLine")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Screen Line")]
	public class CwHitScreenLine : CwHitScreenBase
	{
		public enum FrequencyType
		{
			StartAndEnd = 0,
			PixelInterval = 1,
			ScaledPixelInterval = 2,
			StretchedPixelInterval = 3,
			StretchedScaledPixelInterval = 4,
			Once = 5
		}

		[SerializeField]
		private FrequencyType frequency = FrequencyType.PixelInterval;

		[SerializeField]
		private float interval = 10f;

		[SerializeField]
		[Range(0f, 1f)]
		private float position;

		[SerializeField]
		private float pixelOffset;

		[SerializeField]
		private CwPointConnector connector;

		public FrequencyType Frequency
		{
			get
			{
				return frequency;
			}
			set
			{
				frequency = value;
			}
		}

		public float Interval
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
			}
		}

		public float Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public float PixelOffset
		{
			get
			{
				return pixelOffset;
			}
			set
			{
				pixelOffset = value;
			}
		}

		public CwPointConnector Connector
		{
			get
			{
				if (connector == null)
				{
					connector = new CwPointConnector();
				}
				return connector;
			}
		}

		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			Connector.ClearHitCache();
		}

		[ContextMenu("Reset Connections")]
		public void ResetConnections()
		{
			connector.ResetConnections();
		}

		protected virtual void OnEnable()
		{
			Connector.ResetConnections();
		}

		protected virtual void Update()
		{
			connector.Update();
		}

		public override void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
			if (finger.Index >= 0)
			{
				if (up && storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				switch (frequency)
				{
				case FrequencyType.StartAndEnd:
					PaintStartEnd(finger, up);
					break;
				case FrequencyType.PixelInterval:
					PaintStartInterval(finger, up, interval, stretch: false);
					break;
				case FrequencyType.ScaledPixelInterval:
					PaintStartInterval(finger, up, interval / CwInputManager.ScaleFactor, stretch: false);
					break;
				case FrequencyType.StretchedPixelInterval:
					PaintStartInterval(finger, up, interval, stretch: true);
					break;
				case FrequencyType.StretchedScaledPixelInterval:
					PaintStartInterval(finger, up, interval / CwInputManager.ScaleFactor, stretch: true);
					break;
				case FrequencyType.Once:
					PaintOne(finger, up, position, pixelOffset);
					break;
				}
				connector.BreakHits(finger);
			}
		}

		private void PaintStartEnd(CwInputManager.Finger finger, bool up)
		{
			bool preview = !up;
			Vector2 startScreenPosition = finger.StartScreenPosition;
			Vector2 screenPosition = finger.ScreenPosition;
			Vector2 vector = screenPosition - startScreenPosition;
			PaintAt(connector, connector.HitCache, startScreenPosition, startScreenPosition - vector, preview, finger.Pressure, finger);
			PaintAt(connector, connector.HitCache, screenPosition, screenPosition - vector, preview, finger.Pressure, finger);
		}

		private void PaintStartInterval(CwInputManager.Finger finger, bool up, float pixelSpacing, bool stretch)
		{
			bool preview = !up;
			Vector2 vector = finger.StartScreenPosition;
			Vector2 screenPosition = finger.ScreenPosition;
			Vector2 vector2 = screenPosition - vector;
			float magnitude = vector2.magnitude;
			int num = 0;
			if (pixelSpacing > 0f)
			{
				num = Mathf.FloorToInt(magnitude / pixelSpacing);
				if (stretch && num > 0)
				{
					pixelSpacing = magnitude / (float)num;
				}
			}
			for (int i = 0; i <= num; i++)
			{
				PaintAt(connector, connector.HitCache, vector, vector - vector2, preview, finger.Pressure, finger);
				vector = Vector2.MoveTowards(vector, screenPosition, pixelSpacing);
			}
		}

		private void PaintOne(CwInputManager.Finger finger, bool up, float frac, float pixelOff)
		{
			bool preview = !up;
			Vector2 startScreenPosition = finger.StartScreenPosition;
			Vector2 vector = finger.ScreenPosition - startScreenPosition;
			Vector2 normalized = vector.normalized;
			startScreenPosition += vector * frac + normalized * pixelOff;
			PaintAt(connector, connector.HitCache, startScreenPosition, startScreenPosition - vector, preview, finger.Pressure, finger);
		}
	}
}
