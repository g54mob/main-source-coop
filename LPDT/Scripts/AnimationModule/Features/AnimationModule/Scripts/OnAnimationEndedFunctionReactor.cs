using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class OnAnimationEndedFunctionReactor : MonoBehaviour
	{
		public event Action OnAnimationEnded;

		public void InvokeAnimationEnded()
		{
			this.OnAnimationEnded?.Invoke();
		}
	}
}
