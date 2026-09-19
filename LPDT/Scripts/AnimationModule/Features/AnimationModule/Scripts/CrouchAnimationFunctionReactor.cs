using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class CrouchAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnCrouch;

		public event Action OnCrouchEnd;

		private void InvokeCrouch()
		{
			this.OnCrouch?.Invoke();
		}

		private void InvokeCrouchEnd()
		{
			this.OnCrouchEnd?.Invoke();
		}
	}
}
