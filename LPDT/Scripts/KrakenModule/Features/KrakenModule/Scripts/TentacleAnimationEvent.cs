using System;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public class TentacleAnimationEvent : MonoBehaviour
	{
		public event Action OnCatch;

		public event Action OnStartThrow;

		public event Action OnThrow;

		public void InvokeOnCatch()
		{
			this.OnCatch?.Invoke();
		}

		public void InvokeOnStartThrow()
		{
			this.OnStartThrow?.Invoke();
		}

		public void InvokeOnThrow()
		{
			this.OnThrow?.Invoke();
		}
	}
}
