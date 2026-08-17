using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[RequireComponent(typeof(CwHitPointers))]
	public abstract class CwPointer : MonoBehaviour
	{
		public class VirtualFinger
		{
			public Vector2 Position;
		}

		[NonSerialized]
		protected CwHitPointers cachedHitPointers;

		[NonSerialized]
		private List<CwInputManager.Finger> fingers = new List<CwInputManager.Finger>();

		public int GetFingerCount(bool ignoreStartedOverGui)
		{
			int num = 0;
			for (int i = 0; i < fingers.Count; i++)
			{
				CwInputManager.Finger finger = fingers[i];
				if (!ignoreStartedOverGui || !finger.StartedOverGui)
				{
					num++;
				}
			}
			return num;
		}

		public bool GetFinger(int index, Vector2 position, float pressure, bool set, out CwInputManager.Finger finger)
		{
			for (int i = 0; i < fingers.Count; i++)
			{
				finger = fingers[i];
				if (finger.Index == index)
				{
					StepFinger(finger, position, pressure, set);
					return false;
				}
			}
			finger = new CwInputManager.Finger();
			fingers.Add(finger);
			InitFinger(finger, index, position, pressure, set, cachedHitPointers.GuiLayers);
			return true;
		}

		public bool TryNullFinger(int index)
		{
			for (int i = 0; i < fingers.Count; i++)
			{
				CwInputManager.Finger finger = fingers[i];
				if (finger.Index == index)
				{
					cachedHitPointers.BreakFinger(finger);
					fingers.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		protected virtual void OnEnable()
		{
			cachedHitPointers = GetComponent<CwHitPointers>();
		}

		private void InitFinger(CwInputManager.Finger finger, int index, Vector2 screenPosition, float pressure, bool set, int guiLayers)
		{
			finger.Index = index;
			finger.Down = true;
			finger.Age = 0f;
			finger.StartedOverGui = CwInputManager.PointOverGui(screenPosition, guiLayers);
			finger.StartScreenPosition = screenPosition;
			finger.ScreenPositionOld = screenPosition;
			finger.ScreenPositionOldOld = screenPosition;
			finger.ScreenPositionOldOldOld = screenPosition;
			finger.Pressure = pressure;
			finger.ScreenPosition = screenPosition;
			finger.Up = !set;
		}

		private void StepFinger(CwInputManager.Finger finger, Vector2 screenPosition, float pressure, bool set)
		{
			finger.Down = false;
			finger.Age += Time.deltaTime;
			finger.ScreenPositionOldOldOld = finger.ScreenPositionOldOld;
			finger.ScreenPositionOldOld = finger.ScreenPositionOld;
			finger.ScreenPositionOld = finger.ScreenPosition;
			finger.Pressure = pressure;
			finger.ScreenPosition = screenPosition;
			finger.Up = !set;
		}
	}
}
