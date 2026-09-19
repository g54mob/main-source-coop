using System;
using UnityEngine;

namespace Features.GuillotineModule.Scripts
{
	public class GuillotineAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnExecuted;

		public event Action OnDown;

		public event Action OnUp;

		public void InvokeOnExecuted()
		{
			this.OnExecuted?.Invoke();
		}

		public void InvokeOnDown()
		{
			this.OnDown?.Invoke();
		}

		public void InvokeOnUp()
		{
			this.OnUp?.Invoke();
		}
	}
}
