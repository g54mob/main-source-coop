using System;
using Features.TipsModule.Scripts.Views;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public class SplitTipDisplayData : DualIconTipDisplayData
	{
		public LocalizationKey PrimaryTipLocalizationKey;

		public LocalizationKey SecondaryTipLocalizationKey;

		protected override CombineTipViewBase RentView(ITipViewHost host)
		{
			return host.RentSplit();
		}

		protected override string ResolveTipText(TipBindContext ctx)
		{
			string localizedString = ctx.LocalizationService.GetLocalizedString(PrimaryTipLocalizationKey);
			string localizedString2 = ctx.LocalizationService.GetLocalizedString(SecondaryTipLocalizationKey);
			return localizedString + "/" + localizedString2;
		}

		public override TipDisplayDataBase Clone()
		{
			SplitTipDisplayData splitTipDisplayData = new SplitTipDisplayData();
			CopyDualFieldsTo(splitTipDisplayData);
			splitTipDisplayData.PrimaryTipLocalizationKey = PrimaryTipLocalizationKey;
			splitTipDisplayData.SecondaryTipLocalizationKey = SecondaryTipLocalizationKey;
			return splitTipDisplayData;
		}
	}
}
