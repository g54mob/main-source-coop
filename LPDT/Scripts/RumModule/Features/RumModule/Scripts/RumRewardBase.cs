using UnityEngine;

namespace Features.RumModule.Scripts
{
	public abstract class RumRewardBase : MonoBehaviour
	{
		public abstract void ApplyReward(RumType rumType);
	}
}
