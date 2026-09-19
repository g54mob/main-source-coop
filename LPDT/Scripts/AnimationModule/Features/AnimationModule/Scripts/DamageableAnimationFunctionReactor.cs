using System;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class DamageableAnimationFunctionReactor : MonoBehaviour
	{
		public event Action TryDealBaseAttackDamage;

		public event Action TryDealLowAttackDamage;

		public event Action TryAttackAnimationFinished;

		public void InvokeTryDealBaseAttackDamage()
		{
			this.TryDealBaseAttackDamage?.Invoke();
		}

		public void InvokeTryDealLowAttackDamage()
		{
			this.TryDealLowAttackDamage?.Invoke();
		}

		public void InvokeTryAttackAnimationFinished()
		{
			this.TryAttackAnimationFinished?.Invoke();
		}
	}
}
