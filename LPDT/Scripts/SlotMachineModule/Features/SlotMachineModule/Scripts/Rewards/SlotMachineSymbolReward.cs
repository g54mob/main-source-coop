using System;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts.Rewards
{
	[Serializable]
	public class SlotMachineSymbolReward
	{
		[SerializeField]
		private SlotSymbol _symbol = SlotSymbol.Symbol1;

		[SerializeField]
		private SlotMachineRewardBase _reward;

		public SlotSymbol Symbol => _symbol;

		public SlotMachineRewardBase Reward => _reward;
	}
}
