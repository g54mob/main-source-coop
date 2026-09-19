using System;
using UnityEngine;

namespace Features.CameraModelModule
{
	public interface ICameraGrabFocusService
	{
		bool IsActive { get; }

		Vector3 LookDirection { get; }

		void Activate(Func<Vector3> focusPoint, bool pinAim = false);

		void Deactivate();

		Vector2 ConsumeLookDelta();
	}
}
