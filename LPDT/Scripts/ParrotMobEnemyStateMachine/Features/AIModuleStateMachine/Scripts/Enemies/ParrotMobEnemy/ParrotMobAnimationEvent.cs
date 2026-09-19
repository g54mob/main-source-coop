using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	public class ParrotMobAnimationEvent : MonoBehaviour
	{
		public event Action OnScreamStart;

		public void InvokeOnScreamStart()
		{
			this.OnScreamStart?.Invoke();
		}
	}
}
