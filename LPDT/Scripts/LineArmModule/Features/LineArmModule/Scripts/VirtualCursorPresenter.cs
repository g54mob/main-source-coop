using Features.LineArmModule.Scripts.Data;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	[PublicAPI]
	public class VirtualCursorPresenter : PresenterBehaviour<VirtualCursorViewBase>
	{
		private readonly VirtualCursorModel _virtualCursorModel;

		public VirtualCursorPresenter(VirtualCursorModel virtualCursorModel)
		{
			_virtualCursorModel = virtualCursorModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_virtualCursorModel.OnStateChanged += OnVirtualCursorStateChanged;
			OnVirtualCursorStateChanged(_virtualCursorModel.IsVirtualCursorActive, _virtualCursorModel.ScreenPosition);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_virtualCursorModel.OnStateChanged -= OnVirtualCursorStateChanged;
		}

		private void OnVirtualCursorStateChanged(bool isVisible, Vector2 screenPosition)
		{
			if (isVisible)
			{
				base.View.SetVirtualCursorScreenPosition(screenPosition);
			}
			base.View.SetVirtualCursorVisibility(isVisible);
		}
	}
}
