using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Animation
{
	public class RageAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnRoarSound;

		public event Action OnRoarEnded;

		public event Action OnRageInteractedEnded;

		public event Action OnRageInteractedStart;

		public event Action OnBite;

		public void InvokeRoarSound()
		{
			this.OnRoarSound?.Invoke();
		}

		public void InvokeRoarEnded()
		{
			this.OnRoarEnded?.Invoke();
		}

		public void InvokeRageInteractedEnded()
		{
			this.OnRageInteractedEnded?.Invoke();
		}

		public void InvokeRageInteractedStart()
		{
			this.OnRageInteractedStart?.Invoke();
		}

		public void InvokeBite()
		{
			this.OnBite?.Invoke();
		}
	}
}
