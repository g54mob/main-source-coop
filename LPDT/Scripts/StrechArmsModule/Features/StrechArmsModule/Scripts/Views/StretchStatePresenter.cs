using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.StrechArmsModule.Scripts.Views
{
	public class StretchStatePresenter : PresenterBehaviour<StretchStateViewBase>
	{
		private readonly ArmStateModel _armStateModel;

		private readonly PlayersArmsModel _playersArmsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly Dictionary<Arm, StrechArmController> _armControllers = new Dictionary<Arm, StrechArmController>();

		private bool _leftArmEnabled;

		private bool _rightArmEnabled;

		public StretchStatePresenter(ArmStateModel armStateModel, PlayersArmsModel playersArmsModel, MultiplayerModel multiplayerModel)
		{
			_armStateModel = armStateModel;
			_playersArmsModel = playersArmsModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			_armStateModel.OnArmStateChanged += OnArmStateChanged;
			_armStateModel.OnArmProgressChanged += OnArmProgressChanged;
			_armStateModel.OnArmControllerRegistered += OnArmControllerRegistered;
			_armStateModel.OnEnemyGrabbedChanged += OnEnemyGrabbed;
			_playersArmsModel.OnPlayerArmsUpdated += OnPlayerArmsUpdated;
			StrechArmController armController = _armStateModel.GetArmController(Arm.Left);
			if (armController != null)
			{
				OnArmControllerRegistered(Arm.Left, armController);
			}
			StrechArmController armController2 = _armStateModel.GetArmController(Arm.Right);
			if (armController2 != null)
			{
				OnArmControllerRegistered(Arm.Right, armController2);
			}
			UpdateArmState(Arm.Left);
			UpdateArmState(Arm.Right);
			UpdateArmsVisibility();
		}

		private void OnArmControllerRegistered(Arm armOrientation, StrechArmController controller)
		{
			if (_armControllers.ContainsKey(armOrientation))
			{
				StrechArmController strechArmController = _armControllers[armOrientation];
				strechArmController.OnArmEnable -= OnArmEnable;
				strechArmController.OnArmDisable -= OnArmDisable;
			}
			_armControllers[armOrientation] = controller;
			controller.OnArmEnable += OnArmEnable;
			controller.OnArmDisable += OnArmDisable;
			if (controller.gameObject.activeSelf)
			{
				OnArmEnable(armOrientation);
			}
			else
			{
				OnArmDisable(armOrientation);
			}
		}

		private void OnArmEnable(Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				_leftArmEnabled = true;
				break;
			case Arm.Right:
				_rightArmEnabled = true;
				break;
			}
			UpdateArmsVisibility();
		}

		private void OnArmDisable(Arm armOrientation)
		{
			switch (armOrientation)
			{
			case Arm.Left:
				_leftArmEnabled = false;
				break;
			case Arm.Right:
				_rightArmEnabled = false;
				break;
			}
			UpdateArmsVisibility();
		}

		private void OnPlayerArmsUpdated(int playerId, List<Arm> availableArms)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				UpdateArmsVisibility();
			}
		}

		private void UpdateArmsVisibility()
		{
			if (!(base.View == null))
			{
				int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				List<Arm> value;
				bool flag = _playersArmsModel.AvailableArms.TryGetValue(playerId, out value) && value.Contains(Arm.Left);
				bool flag2 = value?.Contains(Arm.Right) ?? false;
				base.View.SetActiveLeftArm(flag && _leftArmEnabled);
				base.View.SetActiveRightArm(flag2 && _rightArmEnabled);
			}
		}

		protected override void OnDisposed()
		{
			base.OnViewDisabled();
			_armStateModel.OnArmStateChanged -= OnArmStateChanged;
			_armStateModel.OnArmProgressChanged -= OnArmProgressChanged;
			_armStateModel.OnArmControllerRegistered -= OnArmControllerRegistered;
			_playersArmsModel.OnPlayerArmsUpdated -= OnPlayerArmsUpdated;
			_armStateModel.OnEnemyGrabbedChanged -= OnEnemyGrabbed;
			foreach (KeyValuePair<Arm, StrechArmController> armController in _armControllers)
			{
				if (armController.Value != null)
				{
					armController.Value.OnArmEnable -= OnArmEnable;
					armController.Value.OnArmDisable -= OnArmDisable;
				}
			}
			_armControllers.Clear();
		}

		private void OnArmStateChanged(Arm armOrientation, ArmState state)
		{
			if (IsArmAvailable(armOrientation))
			{
				float armProgress = _armStateModel.GetArmProgress(armOrientation);
				base.View.UpdateArmState(armOrientation, state, armProgress);
			}
		}

		private void OnArmProgressChanged(Arm armOrientation, float progress)
		{
			if (IsArmAvailable(armOrientation))
			{
				ArmState armState = _armStateModel.GetArmState(armOrientation);
				base.View.UpdateArmState(armOrientation, armState, progress);
			}
		}

		private void UpdateArmState(Arm armOrientation)
		{
			if (IsArmAvailable(armOrientation))
			{
				ArmState armState = _armStateModel.GetArmState(armOrientation);
				float armProgress = _armStateModel.GetArmProgress(armOrientation);
				base.View.UpdateArmState(armOrientation, armState, armProgress);
			}
		}

		private bool IsArmAvailable(Arm armOrientation)
		{
			MultiplayerModel multiplayerModel = _multiplayerModel;
			if (multiplayerModel == null || !(multiplayerModel.NetworkRunner?.LocalPlayer).HasValue)
			{
				return false;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_playersArmsModel.AvailableArms.TryGetValue(playerId, out var value))
			{
				return false;
			}
			return value.Contains(armOrientation);
		}

		private void OnEnemyGrabbed(Arm arm, bool isGrabbed)
		{
			base.View.SetIsGrabbed(arm, isGrabbed);
		}
	}
}
