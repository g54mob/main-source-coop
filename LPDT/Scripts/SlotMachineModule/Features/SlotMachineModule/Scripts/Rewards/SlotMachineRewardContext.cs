using System;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts.Rewards
{
	public readonly struct SlotMachineRewardContext
	{
		public readonly SlotSymbol Symbol;

		public readonly Vector3 PayoutPosition;

		public readonly Func<Vector3> ResolvePayoutDirection;

		public readonly float PayoutForce;

		public readonly IItemSpawnService ItemSpawnService;

		public readonly ISlotMachinePayoutBudget PayoutBudget;

		public readonly int BetMultiplier;

		public readonly Action<NetworkObject> RegisterRewardItem;

		public SlotMachineRewardContext(SlotSymbol symbol, Vector3 payoutPosition, Func<Vector3> resolvePayoutDirection, float payoutForce, IItemSpawnService itemSpawnService, ISlotMachinePayoutBudget payoutBudget, Action<NetworkObject> registerRewardItem, int betMultiplier)
		{
			RegisterRewardItem = registerRewardItem;
			BetMultiplier = betMultiplier;
			Symbol = symbol;
			PayoutPosition = payoutPosition;
			ResolvePayoutDirection = resolvePayoutDirection;
			PayoutForce = payoutForce;
			ItemSpawnService = itemSpawnService;
			PayoutBudget = payoutBudget;
		}
	}
}
