using System;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.QuotaModule.Scripts
{
	[Serializable]
	public class CurrentWalletSynchronizedModel : JsonSynchronizableBaseWithCustomData<CurrentWalletSynchronizedModel, QuotaSynchronizeData>, ISessionCleanup
	{
		[field: SerializeField]
		public float CurrentSessionMoney { get; private set; }

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<float> OnCurrentSessionMoneyChanged;

		protected override void SetNewValues(CurrentWalletSynchronizedModel model, bool isSynchronizedOnStart)
		{
			CurrentSessionMoney = model.CurrentSessionMoney;
			this.OnCurrentSessionMoneyChanged?.Invoke(CurrentSessionMoney);
		}

		protected override void SetNewCustomValues(QuotaSynchronizeData data)
		{
			switch (data.Operation)
			{
			case QuotaOperation.Add:
				CurrentSessionMoney += data.QuotaToChange;
				break;
			case QuotaOperation.Remove:
				CurrentSessionMoney -= data.QuotaToChange;
				if (CurrentSessionMoney < 0f)
				{
					CurrentSessionMoney = 0f;
				}
				break;
			case QuotaOperation.Replace:
				CurrentSessionMoney = data.QuotaToChange;
				if (CurrentSessionMoney < 0f)
				{
					CurrentSessionMoney = 0f;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.OnCurrentSessionMoneyChanged?.Invoke(CurrentSessionMoney);
		}

		public void AddQuota(float moneyToAdd)
		{
			base.Data1 = new QuotaSynchronizeData
			{
				QuotaToChange = moneyToAdd,
				Operation = QuotaOperation.Add
			};
			CustomSynchronize();
		}

		public void SetQuota(float moneyToSet)
		{
			base.Data1 = new QuotaSynchronizeData
			{
				QuotaToChange = moneyToSet,
				Operation = QuotaOperation.Replace
			};
			CustomSynchronize();
		}

		public void RemoveQuota(float moneyToRemove)
		{
			base.Data1 = new QuotaSynchronizeData
			{
				QuotaToChange = moneyToRemove,
				Operation = QuotaOperation.Remove
			};
			CustomSynchronize();
		}

		public void Cleanup()
		{
			CurrentSessionMoney = 0f;
		}
	}
}
