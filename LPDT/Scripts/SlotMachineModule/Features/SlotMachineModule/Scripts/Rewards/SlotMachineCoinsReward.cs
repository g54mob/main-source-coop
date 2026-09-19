using Cysharp.Threading.Tasks;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts.Rewards
{
	[CreateAssetMenu(fileName = "SlotMachineCoinsReward", menuName = "Configurations/SlotMachine/Rewards/Coins")]
	public class SlotMachineCoinsReward : SlotMachineRewardBase
	{
		[SerializeField]
		private MonoItem _coinPrefab;

		[Tooltip("Coins paid out for a single bet coin. The whole amount is multiplied by the bet.")]
		[Min(1f)]
		[SerializeField]
		private int _minCoinsPerBetCoin = 2;

		[Min(1f)]
		[SerializeField]
		private int _maxCoinsPerBetCoin = 3;

		[Tooltip("Currency value of one paid out coin.")]
		[Min(1f)]
		[SerializeField]
		private int _minCoinValue = 10;

		[Min(1f)]
		[SerializeField]
		private int _maxCoinValue = 20;

		[Min(0f)]
		[SerializeField]
		private float _spawnRandomRadius = 0.06f;

		[Tooltip("Delay between coins so they leave the box one by one instead of overlapping.")]
		[Min(0f)]
		[SerializeField]
		private float _spawnInterval = 0.08f;

		public override UniTask Grant(SlotMachineRewardContext context)
		{
			if (_coinPrefab == null)
			{
				return UniTask.CompletedTask;
			}
			int requestedCount = Random.Range(_minCoinsPerBetCoin, Mathf.Max(_minCoinsPerBetCoin, _maxCoinsPerBetCoin) + 1) * Mathf.Max(1, context.BetMultiplier);
			int num = context.PayoutBudget.ConsumeItemCount(requestedCount);
			if (num <= 0)
			{
				return UniTask.CompletedTask;
			}
			return SpawnCoins(context, num);
		}

		private async UniTask SpawnCoins(SlotMachineRewardContext context, int coinCount)
		{
			for (int i = 0; i < coinCount; i++)
			{
				int num = Random.Range(_minCoinValue, Mathf.Max(_minCoinValue, _maxCoinValue) + 1);
				Vector3 position = context.PayoutPosition + Random.insideUnitSphere * _spawnRandomRadius;
				ItemData itemData = new ItemData(_coinPrefab.DefaultConfig)
				{
					CurrencyValue = num,
					MaxCurrencyValue = num
				};
				NetworkBehaviour networkBehaviour = await context.ItemSpawnService.SpawnItem(_coinPrefab, position, Quaternion.identity, itemData);
				if (!(networkBehaviour == null))
				{
					context.RegisterRewardItem(networkBehaviour.Object);
					if (networkBehaviour.TryGetComponent<MonoItem>(out var component))
					{
						component.AddForce(context.PayoutForce, context.ResolvePayoutDirection(), ForceMode.Impulse);
					}
					if (_spawnInterval > 0f && i < coinCount - 1)
					{
						await UniTask.Delay((int)(_spawnInterval * 1000f));
					}
				}
			}
		}
	}
}
