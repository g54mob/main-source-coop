using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class KillEndedAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnKillEnded;

		private void InvokeOnKillEnded()
		{
			this.OnKillEnded?.Invoke();
		}
	}
}
