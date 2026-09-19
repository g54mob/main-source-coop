using System;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.CurrencyModule.Scripts
{
	[Serializable]
	public class CurrencyModel : JsonSynchronizableBase<CurrencyModel>, ISessionCleanup
	{
		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		[field: SerializeField]
		public int CurrentCurrency { get; private set; }

		public event Action<int> OnCurrencyChanged;

		public void SetCurrency(int currency)
		{
			CurrentCurrency = currency;
			this.OnCurrencyChanged?.Invoke(CurrentCurrency);
		}

		protected override void SetNewValues(CurrencyModel synchronizable, bool isSynchronizedOnStart)
		{
			SetCurrency(synchronizable.CurrentCurrency);
		}

		public void Cleanup()
		{
			CurrentCurrency = 0;
			this.OnCurrencyChanged = null;
		}
	}
}
