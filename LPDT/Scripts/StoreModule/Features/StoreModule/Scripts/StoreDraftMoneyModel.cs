using System;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class StoreDraftMoneyModel : JsonSynchronizableBase<StoreDraftMoneyModel>
	{
		private float _localDraftPending;

		[field: SerializeField]
		public float DraftMoney { get; private set; }

		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<float> OnDraftMoneyChanged;

		public event Action OnDraftFailure;

		protected override void SetNewValues(StoreDraftMoneyModel synchronizable, bool isSynchronizedOnStart)
		{
			SetDraftMoney(synchronizable.DraftMoney, isNeedSynchronize: false);
			_localDraftPending = 0f;
		}

		public void AddDraftMoneyLocal(float value)
		{
			DraftMoney += value;
			_localDraftPending += value;
			this.OnDraftMoneyChanged?.Invoke(DraftMoney);
		}

		public void RemoveDraftMoneyLocal(float value)
		{
			DraftMoney -= value;
			_localDraftPending = Mathf.Max(0f, _localDraftPending - value);
			this.OnDraftMoneyChanged?.Invoke(DraftMoney);
		}

		public void SetDraftMoney(float draftMoney, bool isNeedSynchronize = true)
		{
			DraftMoney = draftMoney;
			_localDraftPending = 0f;
			this.OnDraftMoneyChanged?.Invoke(DraftMoney);
			if (isNeedSynchronize)
			{
				Synchronize();
			}
		}

		public void AddDraftMoney(float value)
		{
			DraftMoney += value;
			this.OnDraftMoneyChanged?.Invoke(DraftMoney);
			Synchronize();
		}

		public void RemoveDraftMoney(float value)
		{
			DraftMoney -= value;
			this.OnDraftMoneyChanged?.Invoke(DraftMoney);
			Synchronize();
		}

		public void OnDraftFailureInvoke()
		{
			this.OnDraftFailure?.Invoke();
		}

		public void SynchronizeDraftMoney()
		{
			Synchronize();
		}
	}
}
