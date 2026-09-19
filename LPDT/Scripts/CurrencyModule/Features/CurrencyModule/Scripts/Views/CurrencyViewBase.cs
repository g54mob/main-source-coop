using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.CurrencyModule.Scripts.Views
{
	public abstract class CurrencyViewBase : ViewBehaviour
	{
		public abstract void SetCurrentCurrency(int currency);
	}
}
