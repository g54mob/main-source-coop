using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface IGuideLineEntity
	{
		void SetPoints(IReadOnlyList<Vector3> points);

		void Enable();

		void Disable();

		void SetDissolve(float amount);

		void AnimateDissolve(float from, float to, float duration, Action onComplete);

		void StopDissolveAnimation();
	}
}
