using Features.MultiplayerSessionServices.Scripts;
using Features.StrechArmsModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class VisibilityDebugPresenter : PresenterBehaviour<VisibilityDebugViewBase>
	{
		private readonly SessionWindow _sessionWindow;

		private readonly ArmStartsModel _armsStartsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private bool _isUiActive = true;

		private bool _isArmsActive = true;

		public VisibilityDebugPresenter(SessionWindow sessionWindow, ArmStartsModel armsStartsModel, MultiplayerModel multiplayerModel)
		{
			_sessionWindow = sessionWindow;
			_armsStartsModel = armsStartsModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnArmsActiveSwitched += SwitchActiveArms;
			base.View.OnUIActiveSwitched += SwitchUi;
			base.View.RefreshArmsToggle(_isArmsActive);
			base.View.RefreshUiToggle(_isUiActive);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnArmsActiveSwitched -= SwitchActiveArms;
			base.View.OnUIActiveSwitched -= SwitchUi;
		}

		private void SwitchActiveArms(bool isActive)
		{
			Transform transform = _armsStartsModel.GetEnd(_multiplayerModel.NetworkRunner.LocalPlayer, Arm.Left).Transform;
			Transform transform2 = _armsStartsModel.GetEnd(_multiplayerModel.NetworkRunner.LocalPlayer, Arm.Right).Transform;
			Transform arm = _armsStartsModel.GetArm(_multiplayerModel.NetworkRunner.LocalPlayer, Arm.Left);
			Transform arm2 = _armsStartsModel.GetArm(_multiplayerModel.NetworkRunner.LocalPlayer, Arm.Right);
			transform.gameObject.SetActive(isActive);
			transform2.gameObject.SetActive(isActive);
			arm.gameObject.SetActive(isActive);
			arm2.gameObject.SetActive(isActive);
			_isArmsActive = isActive;
		}

		private void SwitchUi(bool isActive)
		{
			if (isActive)
			{
				_sessionWindow.Open();
			}
			else
			{
				_sessionWindow.Close();
			}
			_isUiActive = isActive;
		}
	}
}
