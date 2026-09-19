using System.Collections.Generic;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts.Rewards
{
	[CreateAssetMenu(fileName = "SlotMachineRewardConfig", menuName = "Configurations/SlotMachine/Reward Config")]
	public class SlotMachineRewardConfig : ScriptableObject
	{
		[Tooltip("Paid out for any three-of-a-kind that has no override below.")]
		[SerializeField]
		private SlotMachineRewardBase _defaultReward;

		[Tooltip("Per-symbol payouts. An entry here replaces the default reward for that combination.")]
		[SerializeField]
		private List<SlotMachineSymbolReward> _symbolOverrides = new List<SlotMachineSymbolReward>();

		[Tooltip("Total number of items one machine may ever hand out across the whole session.")]
		[Min(0f)]
		[SerializeField]
		private int _maxTotalItemCount = 40;

		public int MaxTotalItemCount => _maxTotalItemCount;

		public SlotMachineRewardBase GetReward(SlotSymbol symbol)
		{
			for (int i = 0; i < _symbolOverrides.Count; i++)
			{
				SlotMachineSymbolReward slotMachineSymbolReward = _symbolOverrides[i];
				if (slotMachineSymbolReward.Symbol == symbol && slotMachineSymbolReward.Reward != null)
				{
					return slotMachineSymbolReward.Reward;
				}
			}
			return _defaultReward;
		}
	}
}
