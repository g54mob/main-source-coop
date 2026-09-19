using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG.Muffin.InputSubmodule.InputModule.Core.Scripts
{
	public class InputVector2Actions : InputDefaultActions
	{
		public Action<Vector2> VectorChangedStarted;

		public Action<Vector2> VectorChangedPerformed;

		public Action<Vector2> VectorChangedCanceled;

		public Action<InputDevice, Vector2> VectorChangedWithDeviceCallbackStarted;

		public Action<InputDevice, Vector2> VectorChangedWithDeviceCallbackPerformed;

		public Action<InputDevice, Vector2> VectorChangedWithDeviceCallbackCanceled;
	}
}
