using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public abstract class MatchmakingPreviewPopupViewBase : ViewBehaviour
	{
		[SerializeField]
		private LocalizationKey _noGamesFoundLocalizationKey;

		[field: SerializeField]
		public Selectable FirstButtonToSelectFound { get; private set; }

		[field: SerializeField]
		public Selectable FirstButtonToSelectNotFound { get; private set; }

		[field: SerializeField]
		public Selectable ButtonToSelectOnExit { get; private set; }

		[field: SerializeField]
		public RectTransform AnimationTarget { get; private set; }

		[field: SerializeField]
		public LocalizationKey NoGamesFoundDescriptionLocalizationKey { get; private set; }

		[field: SerializeField]
		public LocalizationKey GameFoundLocalizationKey { get; private set; }

		public LocalizationKey NoGamesFoundLocalizationKey => _noGamesFoundLocalizationKey;

		public event Action OnJoinClicked;

		public event Action OnSearchAgainClicked;

		public event Action OnCloseClicked;

		public abstract void SetVisible(bool isVisible);

		public abstract void ShowSessionInfo(string header, string hostName, string playersText, string region, string ping);

		public abstract void ShowNoGamesFound(string header, string description);

		protected void InvokeJoinClicked()
		{
			this.OnJoinClicked?.Invoke();
		}

		protected void InvokeSearchAgainClicked()
		{
			this.OnSearchAgainClicked?.Invoke();
		}

		protected void InvokeCloseClicked()
		{
			this.OnCloseClicked?.Invoke();
		}
	}
}
