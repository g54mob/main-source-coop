using Features.QuotaModule.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.BellFeature.Scripts.Views
{
	[PublicAPI]
	public class BellPresenter : PresenterBehaviour<BellViewBase>
	{
		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		public BellPresenter(QuotaCompletionModel quotaCompletionModel, QuotaSynchronizedModel quotaSynchronizedModel)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_quotaSynchronizedModel = quotaSynchronizedModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_quotaCompletionModel.OnQuotaCompleted += OnQuotaCompleted;
			_quotaCompletionModel.OnBellActivated += OnBellActivated;
			_quotaSynchronizedModel.OnQuotaChanged += OnQuotaChanged;
			UpdateBellState();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_quotaCompletionModel.OnQuotaCompleted -= OnQuotaCompleted;
			_quotaCompletionModel.OnBellActivated -= OnBellActivated;
			_quotaSynchronizedModel.OnQuotaChanged -= OnQuotaChanged;
		}

		private void OnQuotaCompleted(bool _)
		{
			UpdateBellState();
		}

		private void OnBellActivated(bool _)
		{
			UpdateBellState();
		}

		private void OnQuotaChanged(float _, float __)
		{
			UpdateBellState();
		}

		private void UpdateBellState()
		{
			bool isActive = QuotaBellReadiness.IsQuotaMet(_quotaCompletionModel, _quotaSynchronizedModel) && !_quotaCompletionModel.IsBellActivated.Value;
			base.View.SetIsActive(isActive);
		}
	}
}
