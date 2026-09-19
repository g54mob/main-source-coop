using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts.Rewards
{
	public abstract class SlotMachineRewardBase : ScriptableObject
	{
		public abstract UniTask Grant(SlotMachineRewardContext context);
	}
}
