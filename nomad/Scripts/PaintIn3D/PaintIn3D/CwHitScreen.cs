using System;
using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitScreen")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Screen")]
	public class CwHitScreen : CwHitScreenBase
	{
		protected class Link : CwInputManager.Link
		{
			public float Age;

			public bool Down;

			public int State;

			public float Distance;

			public Vector2 ScreenDelta;

			public Vector2 ScreenOld;

			public List<Vector2> History = new List<Vector2>();

			public void Move(Vector2 screenNew)
			{
				if (State == 0)
				{
					ScreenOld = screenNew;
					State = 1;
				}
				else if (TryMove(screenNew) || State == 2)
				{
					State++;
				}
			}

			private bool TryMove(Vector2 screenNew)
			{
				float num = 2f;
				float num2 = Vector2.Distance(ScreenOld, screenNew);
				if (num2 >= num)
				{
					ScreenOld = Vector2.MoveTowards(ScreenOld, screenNew, num2 - num * 0.5f);
					return true;
				}
				return false;
			}

			public override void Clear()
			{
				Age = 0f;
				Down = false;
				State = 0;
				Distance = 0f;
				ScreenDelta = Vector2.zero;
				ScreenOld = Vector2.zero;
				History.Clear();
			}
		}

		public enum FrequencyType
		{
			PixelInterval = 0,
			ScaledPixelInterval = 1,
			TimeInterval = 2,
			OnceOnRelease = 3,
			OnceOnPress = 4,
			OnceEveryFrame = 5
		}

		[SerializeField]
		private FrequencyType frequency = FrequencyType.OnceEveryFrame;

		[SerializeField]
		private float interval = 10f;

		[SerializeField]
		private CwPointConnector connector;

		[NonSerialized]
		private List<Link> links = new List<Link>();

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
			foreach (Link link in links)
			{
				link.Clear();
			}
			Connector.ResetConnections();
			if (ShouldUpgradePointers())
			{
				Debug.LogWarning("Upgrading CwHitScreen Controls - To remove this warning you can manually click the \"Upgrade\" button in the inspector of this component while outside of play mode.", base.gameObject);
				TryUpgradePointers();
			}
		}

		protected virtual void Update()
		{
			connector.Update();
		}

		public override void BreakFinger(CwInputManager.Finger finger)
		{
			Link link = CwInputManager.Link.Find(links, finger);
			if (link != null)
			{
				connector.BreakHits(link);
			}
		}

		public override void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
			Link link = CwInputManager.Link.Find(links, finger);
			bool flag = true;
			if (finger.Index < 0)
			{
				if (CwInputManager.PointOverGui(finger.ScreenPosition, base.GuiLayers))
				{
					connector.BreakHits(link);
					return;
				}
			}
			else if (down)
			{
				if (CwInputManager.PointOverGui(finger.ScreenPosition, base.GuiLayers))
				{
					connector.BreakHits(link);
					return;
				}
			}
			else if (link == null)
			{
				return;
			}
			if (link == null)
			{
				link = CwInputManager.Link.Create(ref links, finger);
			}
			link.Move(finger.ScreenPosition);
			if (finger.Index < 0)
			{
				RecordAndPaintAt(link, finger.ScreenPosition, link.ScreenOld, preview: true, 0f, link);
				return;
			}
			if (base.NeedsDrawAngle)
			{
				down = link.State == 2;
				flag = link.State >= 2;
			}
			if (flag)
			{
				switch (frequency)
				{
				case FrequencyType.PixelInterval:
					PaintSmooth(link, down, interval);
					break;
				case FrequencyType.ScaledPixelInterval:
					PaintSmooth(link, down, interval / CwInputManager.ScaleFactor);
					break;
				case FrequencyType.TimeInterval:
					PaintInterval(link, down);
					break;
				case FrequencyType.OnceOnRelease:
					PaintRelease(link, up);
					break;
				case FrequencyType.OnceOnPress:
					PaintPress(link, down);
					break;
				case FrequencyType.OnceEveryFrame:
					PaintEvery(link, down);
					break;
				}
			}
			base.HandleFingerUpdate(finger, down, up);
		}

		protected override void HandleFingerUp(CwInputManager.Finger finger)
		{
			Link link = CwInputManager.Link.Find(links, finger);
			if (link != null)
			{
				connector.BreakHits(link);
				OnFingerUp(link);
				link.Clear();
			}
		}

		private void PaintSmooth(Link link, bool down, float pixelSpacing)
		{
			Vector2 vector = link.Finger.GetSmoothScreenPosition(0f);
			if (down || link.History.Count == 0)
			{
				if (storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview: false, link.Finger.Pressure, link);
			}
			if (!(pixelSpacing > 0f))
			{
				return;
			}
			int num = Mathf.Max(1, Mathf.FloorToInt(link.Finger.SmoothScreenPositionDelta));
			float num2 = CwHelper.Reciprocal(num);
			for (int i = 0; i <= num; i++)
			{
				Vector2 smoothScreenPosition = link.Finger.GetSmoothScreenPosition(Mathf.Clamp01((float)i * num2));
				float num3 = Vector2.Distance(vector, smoothScreenPosition);
				int num4 = Mathf.FloorToInt((link.Distance + num3) / pixelSpacing);
				for (int j = 0; j < num4; j++)
				{
					float num5 = pixelSpacing - link.Distance;
					vector = Vector2.MoveTowards(vector, smoothScreenPosition, num5);
					RecordAndPaintAt(link, vector, link.History[link.History.Count - 1], preview: false, link.Finger.Pressure, link);
					num3 -= num5;
					link.Distance = 0f;
				}
				link.Distance += num3;
				vector = smoothScreenPosition;
			}
		}

		protected virtual void OnFingerUp(Link link)
		{
		}

		private void PaintInterval(Link link, bool down)
		{
			if (down)
			{
				if (storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				link.Age = interval;
			}
			link.Age += Time.deltaTime;
			if (link.Age >= interval)
			{
				if (interval > 0f)
				{
					link.Age %= interval;
				}
				else
				{
					link.Age = 0f;
				}
				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview: false, link.Finger.Pressure, link);
			}
		}

		private void PaintRelease(Link link, bool up)
		{
			bool preview = true;
			if (up)
			{
				preview = false;
				if (storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
			}
			RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview, link.Finger.Pressure, link);
		}

		private void PaintPress(Link link, bool down)
		{
			if (down)
			{
				if (storeStates)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview: false, link.Finger.Pressure, link);
			}
		}

		private void PaintEvery(Link link, bool down)
		{
			if (down && storeStates)
			{
				CwStateManager.PotentiallyStoreAllStates();
			}
			RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview: false, link.Finger.Pressure, link);
		}

		private void RecordAndPaintAt(Link link, Vector2 screenNew, Vector2 screenOld, bool preview, float pressure, object owner)
		{
			link.History.Add(screenNew);
			PaintAt(connector, connector.HitCache, screenNew, screenOld, preview, pressure, owner);
		}
	}
}
