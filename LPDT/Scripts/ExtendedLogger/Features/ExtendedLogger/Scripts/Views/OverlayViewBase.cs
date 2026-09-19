using Global.SerializableDictionary;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;

namespace Features.ExtendedLogger.Scripts.Views
{
	public abstract class OverlayViewBase : ViewBehaviour
	{
		public SerializableDictionary<DebugFilterType, TMP_Text> OverlaysByFilters = new SerializableDictionary<DebugFilterType, TMP_Text>();
	}
}
