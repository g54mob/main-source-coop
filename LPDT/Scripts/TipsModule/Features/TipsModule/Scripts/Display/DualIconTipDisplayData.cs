using System;
using Features.TipsModule.Scripts.Views;
using Global.Modules.LocalizationModule.Scripts.Generated;
using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public abstract class DualIconTipDisplayData : TipDisplayDataBase
	{
		public string PrimaryTipIconText;

		public InputActionReference PrimaryInputAction;

		public string SecondaryTipIconText;

		public InputActionReference SecondaryInputAction;

		public LocalizationKey TipLocalizationKey;

		public override void Activate(ITipViewHost host, TipBindContext ctx)
		{
			CombineTipViewBase combineTipViewBase = RentView(host);
			ApplyTo(combineTipViewBase, ctx);
			combineTipViewBase.ShowView();
		}

		protected abstract CombineTipViewBase RentView(ITipViewHost host);

		public void ApplyTo(CombineTipViewBase view, TipBindContext ctx)
		{
			TipIconBinder.Apply(view.PrimaryTipIcon, view.PrimaryTipIconText, PrimaryInputAction, PrimaryTipIconText, ctx.InputDeviceService, IconCustomizationType);
			TipIconBinder.Apply(view.SecondaryTipIcon, view.SecondaryTipIconText, SecondaryInputAction, SecondaryTipIconText, ctx.InputDeviceService, IconCustomizationType);
			view.TipText.SetText(ResolveTipText(ctx));
		}

		protected virtual string ResolveTipText(TipBindContext ctx)
		{
			return ctx.LocalizationService.GetLocalizedString(TipLocalizationKey);
		}

		protected void CopyDualFieldsTo(DualIconTipDisplayData target)
		{
			target.ShowOnInputDevices = ShowOnInputDevices;
			target.HideOnInputDevices = HideOnInputDevices;
			target.IconCustomizationType = IconCustomizationType;
			target.PrimaryTipIconText = PrimaryTipIconText;
			target.PrimaryInputAction = PrimaryInputAction;
			target.SecondaryTipIconText = SecondaryTipIconText;
			target.SecondaryInputAction = SecondaryInputAction;
			target.TipLocalizationKey = TipLocalizationKey;
		}
	}
}
