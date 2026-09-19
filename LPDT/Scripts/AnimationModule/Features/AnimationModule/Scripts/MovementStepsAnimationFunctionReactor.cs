using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class MovementStepsAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnStep;

		public event Action OnRunStep;

		private void InvokeStep()
		{
			this.OnStep?.Invoke();
		}

		private void InvokeRunStep()
		{
			this.OnRunStep?.Invoke();
		}
	}
}
