using Features.DisconnectHandlerModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class WantToLeaveLobbyPopupPresenter : PresenterBehaviour<WantToLeaveLobbyPopupViewBase>
	{
		private readonly WantToLeaveLobbyPopupModel _model;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		public WantToLeaveLobbyPopupPresenter(WantToLeaveLobbyPopupModel model, DisconnectRequestEventClass disconnectRequestEventClass)
		{
			_model = model;
			_disconnectRequestEventClass = disconnectRequestEventClass;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			base.View.SetVisible(_model.IsOpen);
			base.View.OnYesClicked += ConfirmLeave;
			base.View.OnNoClicked += CancelLeave;
			_model.OnOpenChanged += OnModelOpenChanged;
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			base.View.OnYesClicked -= ConfirmLeave;
			base.View.OnNoClicked -= CancelLeave;
			_model.OnOpenChanged -= OnModelOpenChanged;
		}

		private void OnModelOpenChanged(bool isOpen)
		{
			base.View.SetVisible(isOpen);
		}

		private void ConfirmLeave()
		{
			_model.SetOpen(isOpen: false);
			_disconnectRequestEventClass.Publish(DisconnectRequestReason.LobbyLeave);
		}

		private void CancelLeave()
		{
			_model.SetOpen(isOpen: false);
		}
	}
}
