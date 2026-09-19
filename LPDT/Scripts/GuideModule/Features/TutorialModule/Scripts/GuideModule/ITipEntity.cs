using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface ITipEntity
	{
		Transform Transform { get; }

		bool IsInitialized { get; set; }

		event Action<bool> OnInitialized;

		void Enable();

		void Disable();

		void SetDissolve(float amount);

		void AnimateDissolve(float from, float to, float duration, Action onComplete);

		void StopDissolveAnimation();
	}
}
