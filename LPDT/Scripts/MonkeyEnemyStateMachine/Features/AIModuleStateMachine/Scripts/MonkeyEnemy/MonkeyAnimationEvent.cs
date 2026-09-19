using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyAnimationEvent : MonoBehaviour
	{
		public event Action OnAttack;

		public event Action OnFakeAttack;

		public event Action OnGrab;

		public event Action OnSpawnGiftItem;

		public event Action OnStep;

		public void InvokeOnAttack()
		{
			this.OnAttack?.Invoke();
		}

		public void InvokeOnFakeAttack()
		{
			this.OnFakeAttack?.Invoke();
		}

		public void InvokeOnGrab()
		{
			this.OnGrab?.Invoke();
		}

		public void InvokeOnSpawnGiftItem()
		{
			this.OnSpawnGiftItem?.Invoke();
		}

		public void InvokeOnStep()
		{
			this.OnStep?.Invoke();
		}
	}
}
