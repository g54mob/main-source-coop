using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.CurrencyModule.Scripts.Views
{
	public class CurrencyPresenter : PresenterBehaviour<CurrencyViewBase>
	{
		private readonly CurrencyModel _model;

		public CurrencyPresenter(CurrencyModel model)
		{
			_model = model;
		}

		protected override void OnViewSet()
		{
			_model.OnCurrencyChanged += UpdateCurrencyCount;
			SetCurrencyCount();
		}

		protected override void OnDisposed()
		{
			_model.OnCurrencyChanged -= UpdateCurrencyCount;
		}

		private void SetCurrencyCount()
		{
			UpdateCurrencyCount(_model.CurrentCurrency);
		}

		private void UpdateCurrencyCount(int currency)
		{
			base.View.SetCurrentCurrency(currency);
		}
	}
}
