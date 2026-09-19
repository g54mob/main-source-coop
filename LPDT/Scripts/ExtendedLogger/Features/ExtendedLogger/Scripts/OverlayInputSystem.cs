using System;
using Features.InputModule.Scripts.Generated;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.ExtendedLogger.Scripts
{
	public class OverlayInputSystem : IInitializable, IDisposable
	{
		private readonly IInputService _inputService;

		private readonly IOverlayService _overlayService;

		private readonly OverlayGlobalWindow _overlayWindow;

		public OverlayInputSystem(IInputService inputService, IOverlayService overlayService, OverlayGlobalWindow overlayWindow)
		{
			_inputService = inputService;
			_overlayService = overlayService;
			_overlayWindow = overlayWindow;
		}

		public void Initialize()
		{
			InputDefaultActions turnOnOffOverlay = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay.Performed = (Action)Delegate.Combine(turnOnOffOverlay.Performed, new Action(SwitchOverlayActive));
		}

		public void Dispose()
		{
			InputDefaultActions turnOnOffOverlay = _inputService.TurnOnOffOverlay;
			turnOnOffOverlay.Performed = (Action)Delegate.Remove(turnOnOffOverlay.Performed, new Action(SwitchOverlayActive));
		}

		private void SwitchOverlayActive()
		{
			if (Debug.isDebugBuild)
			{
				_overlayService.SetOverlayActive(_overlayWindow.WindowStatus == WindowStatus.Closed);
			}
		}
	}
}
