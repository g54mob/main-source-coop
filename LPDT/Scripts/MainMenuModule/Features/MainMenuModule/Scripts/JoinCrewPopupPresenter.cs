using Features.SteamInviteModule.Scripts;
using Features.UIAnimationsModule.Scripts;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class JoinCrewPopupPresenter : PresenterBehaviour<JoinCrewPopupViewBase>, IBackButtonProcessor
	{
		private readonly JoinCrewPopupModel _model;

		private readonly ISteamInviteService _steamInviteService;

		private readonly SteamModel _steamModel;

		private readonly INavigationService _navigationService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly IUIAnimationService _uiAnimationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public JoinCrewPopupPresenter(JoinCrewPopupModel model, ISteamInviteService steamInviteService, SteamModel steamModel, INavigationService navigationService, IUIBackButtonRegistrationService backButtonRegistrationService, IUIAnimationService uiAnimationService)
		{
			_model = model;
			_steamInviteService = steamInviteService;
			_steamModel = steamModel;
			_navigationService = navigationService;
			_backButtonRegistrationService = backButtonRegistrationService;
			_uiAnimationService = uiAnimationService;
		}

		public bool CanHandleBack()
		{
			return _model.IsOpen;
		}

		public void OnBack()
		{
			if (_model.IsOpen)
			{
				ClosePopup();
			}
		}

		protected override void OnViewSet()
		{
			base.View.SetVisible(_model.IsOpen);
			base.View.SetJoinViaCodeInteractable(_model.JoinViaCodeInteractable);
			UpdateSteamJoinVisibility();
			base.View.OnCloseClicked += ClosePopup;
			base.View.OnJoinViaCodeClicked += RequestJoinViaCode;
			base.View.OnJoinViaSteamClicked += JoinViaSteam;
			base.View.OnRoomIdentifierChanged += OnRoomIdentifierChanged;
			_model.OnOpenChanged += OnOpenChanged;
			_model.OnJoinViaCodeInteractableChanged += OnJoinViaCodeInteractableChanged;
			_model.OnRoomIdentifierDisplayChanged += OnRoomIdentifierDisplayChanged;
			_steamInviteService.OnSteamInitialized += UpdateSteamJoinVisibility;
			if (_model.IsOpen)
			{
				_backButtonRegistrationService.Register(this);
			}
		}

		protected override void OnDisposed()
		{
			base.View.OnCloseClicked -= ClosePopup;
			base.View.OnJoinViaCodeClicked -= RequestJoinViaCode;
			base.View.OnJoinViaSteamClicked -= JoinViaSteam;
			base.View.OnRoomIdentifierChanged -= OnRoomIdentifierChanged;
			_model.OnOpenChanged -= OnOpenChanged;
			_model.OnJoinViaCodeInteractableChanged -= OnJoinViaCodeInteractableChanged;
			_model.OnRoomIdentifierDisplayChanged -= OnRoomIdentifierDisplayChanged;
			_steamInviteService.OnSteamInitialized -= UpdateSteamJoinVisibility;
			_backButtonRegistrationService.Unregister(this);
		}

		private void OnJoinViaCodeInteractableChanged(bool interactable)
		{
			base.View.SetJoinViaCodeInteractable(interactable);
		}

		private void OnRoomIdentifierDisplayChanged(string roomIdentifier)
		{
			base.View.SetRoomIdentifier(roomIdentifier);
		}

		private void RequestJoinViaCode()
		{
			_model.RequestJoinViaCode();
		}

		private void OnRoomIdentifierChanged(string roomIdentifier)
		{
			_model.NotifyRoomIdentifierChanged(roomIdentifier);
		}

		private void OnOpenChanged(bool isOpen)
		{
			base.View.SetVisible(isOpen);
			if (isOpen)
			{
				_backButtonRegistrationService.Register(this);
				PlayOpenAnimation();
				if (base.View.FirstButtonToSelect != null)
				{
					_navigationService.SetNavigationToObject(base.View.FirstButtonToSelect);
				}
			}
			else
			{
				_backButtonRegistrationService.Unregister(this);
				if (base.View.ButtonToSelectOnExit != null)
				{
					_navigationService.SetNavigationToObject(base.View.ButtonToSelectOnExit);
				}
			}
		}

		private void ClosePopup()
		{
			_model.SetOpen(isOpen: false);
			base.View.ClearInputField();
		}

		private void PlayOpenAnimation()
		{
			_uiAnimationService.Play(base.View.AnimationTarget, UIAnimationKey.ScaleUp);
		}

		private void JoinViaSteam()
		{
			_steamInviteService.OpenFriendsOverlay();
		}

		private void UpdateSteamJoinVisibility()
		{
			base.View.SetSteamJoinVisible(_steamModel.IsSteamInitialized);
		}
	}
}
