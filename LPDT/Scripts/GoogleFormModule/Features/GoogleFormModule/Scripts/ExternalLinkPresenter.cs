using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.GoogleFormModule.Scripts
{
	public class ExternalLinkPresenter : PresenterBehaviour<ExternalLinkViewBase>
	{
		private readonly GoogleFormConfiguration _googleFormConfiguration;

		private readonly GameAnalyticsEventSendService _analyticsEventSendService;

		public ExternalLinkPresenter(GoogleFormConfiguration googleFormConfiguration, GameAnalyticsEventSendService analyticsEventSendService)
		{
			_googleFormConfiguration = googleFormConfiguration;
			_analyticsEventSendService = analyticsEventSendService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnFeedbackButtonClick += RedirectToExternalLink;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnFeedbackButtonClick -= RedirectToExternalLink;
		}

		private void RedirectToExternalLink(ExternalLinkType externalLinkType)
		{
			_analyticsEventSendService.NewDesignEvent($"{base.View.ExternalLinkSource}:Click:{externalLinkType}");
			Application.OpenURL(_googleFormConfiguration.ExternalLinks[externalLinkType]);
		}
	}
}
