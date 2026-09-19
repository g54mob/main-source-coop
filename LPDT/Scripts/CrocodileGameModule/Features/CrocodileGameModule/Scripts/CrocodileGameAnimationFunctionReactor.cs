using System;
using UnityEngine;

namespace Features.CrocodileGameModule.Scripts
{
	public class CrocodileGameAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnBiteDamage;

		public event Action OnBiteFinished;

		public void InvokeOnBiteDamage()
		{
			this.OnBiteDamage?.Invoke();
		}

		public void InvokeOnBiteFinished()
		{
			this.OnBiteFinished?.Invoke();
		}
	}
}
