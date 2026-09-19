using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.QuotaModule.Scripts.Views
{
	[PublicAPI]
	public class QuotaDebugPresenter : PresenterBehaviour<QuotaDebugViewBase>
	{
		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		public QuotaDebugPresenter(QuotaSynchronizedModel quotaSynchronizedModel)
		{
			_quotaSynchronizedModel = quotaSynchronizedModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.AddQuotaButton.onClick.AddListener(AddQuota);
			base.View.RemoveQuotaButton.onClick.AddListener(RemoveQuota);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.AddQuotaButton.onClick.RemoveListener(AddQuota);
			base.View.RemoveQuotaButton.onClick.RemoveListener(RemoveQuota);
		}

		private void AddQuota()
		{
			_quotaSynchronizedModel.CurrentQuota.Value = Mathf.Max(0f, _quotaSynchronizedModel.CurrentQuota.Value + 100f);
		}

		private void RemoveQuota()
		{
			_quotaSynchronizedModel.CurrentQuota.Value = Mathf.Max(0f, _quotaSynchronizedModel.CurrentQuota.Value - 100f);
		}
	}
}
