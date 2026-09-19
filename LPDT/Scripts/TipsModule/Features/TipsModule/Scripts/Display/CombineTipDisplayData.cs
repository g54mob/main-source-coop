using System;
using Features.TipsModule.Scripts.Views;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public class CombineTipDisplayData : DualIconTipDisplayData
	{
		protected override CombineTipViewBase RentView(ITipViewHost host)
		{
			return host.RentCombine();
		}

		public override TipDisplayDataBase Clone()
		{
			CombineTipDisplayData combineTipDisplayData = new CombineTipDisplayData();
			CopyDualFieldsTo(combineTipDisplayData);
			return combineTipDisplayData;
		}
	}
}
