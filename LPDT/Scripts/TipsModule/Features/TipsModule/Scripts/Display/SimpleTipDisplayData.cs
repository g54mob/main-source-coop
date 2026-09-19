using System;
using Features.TipsModule.Scripts.Views;
using Global.Modules.LocalizationModule.Scripts.Generated;
using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public class SimpleTipDisplayData : TipDisplayDataBase
	{
		public string TipIconText;

		public InputActionReference InputAction;

		public LocalizationKey TipLocalizationKey;

		public override void Activate(ITipViewHost host, TipBindContext ctx)
		{
			TipViewBase tipViewBase = host.RentSimple();
			ApplyTo(tipViewBase, ctx);
			tipViewBase.ShowView();
		}

		public override TipDisplayDataBase Clone()
		{
			return new SimpleTipDisplayData
			{
				ShowOnInputDevices = ShowOnInputDevices,
				HideOnInputDevices = HideOnInputDevices,
				IconCustomizationType = IconCustomizationType,
				TipIconText = TipIconText,
				InputAction = InputAction,
				TipLocalizationKey = TipLocalizationKey
			};
		}

		public void ApplyTo(TipViewBase view, TipBindContext ctx)
		{
			TipIconBinder.Apply(view.TipIcon, view.TipIconText, InputAction, TipIconText, ctx.InputDeviceService, IconCustomizationType);
			view.TipText.SetText(ctx.LocalizationService.GetLocalizedString(TipLocalizationKey));
		}
	}
}
