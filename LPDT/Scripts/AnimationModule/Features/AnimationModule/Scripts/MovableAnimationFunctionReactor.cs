using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class MovableAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnFootstepPerformed;

		public void InvokeOnFootstepPerformed()
		{
			this.OnFootstepPerformed?.Invoke();
		}
	}
}
