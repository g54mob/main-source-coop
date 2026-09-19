using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	public interface IFearHoleAbsorbVisualController
	{
		void ResetVisual();

		void Play(float duration, float endScale, int ease);

		void Play(float duration, float endScale, int ease, Vector3 worldTarget);

		void Kill(bool resetScale);
	}
}
