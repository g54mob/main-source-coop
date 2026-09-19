using System;
using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	public class CustomizationAnimationReactor : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		public event Action OnCrouchInStart;

		public event Action OnCrouchInEnd;

		public event Action OnCrouchOutStart;

		public event Action OnCrouchOutEnd;

		public event Action OnCustomizationReset;

		public event Action OnCrouchLoopStart;

		public event Action OnCrouchLoopEnd;

		public void ResetCustomizationAnimationState()
		{
			this.OnCustomizationReset?.Invoke();
		}

		public void InvokeOnCrouchInStart()
		{
			this.OnCrouchInStart?.Invoke();
		}

		public void InvokeOnCrouchInEnd()
		{
			this.OnCrouchInEnd?.Invoke();
		}

		public void InvokeOnCrouchOutStart()
		{
			this.OnCrouchOutStart?.Invoke();
		}

		public void InvokeOnCrouchOutEnd()
		{
			this.OnCrouchOutEnd?.Invoke();
		}

		public void InvokeOnCrouchLoopEnd()
		{
			this.OnCrouchLoopEnd?.Invoke();
		}

		public void InvokeOnCrouchLoopStart()
		{
			this.OnCrouchLoopStart?.Invoke();
		}
	}
}
