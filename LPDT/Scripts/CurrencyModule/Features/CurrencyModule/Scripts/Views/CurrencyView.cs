using TMPro;
using UnityEngine;

namespace Features.CurrencyModule.Scripts.Views
{
	public class CurrencyView : CurrencyViewBase
	{
		[SerializeField]
		private TMP_Text _currencyCount;

		public override void SetCurrentCurrency(int currency)
		{
			_currencyCount.text = currency.ToString();
		}
	}
}
