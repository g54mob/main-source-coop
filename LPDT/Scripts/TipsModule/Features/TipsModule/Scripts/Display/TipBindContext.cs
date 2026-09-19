using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;

namespace Features.TipsModule.Scripts.Display
{
	public sealed class TipBindContext
	{
		public ILocalizationService LocalizationService { get; }

		public IInputDeviceService InputDeviceService { get; }

		public TipBindContext(ILocalizationService localizationService, IInputDeviceService inputDeviceService)
		{
			LocalizationService = localizationService;
			InputDeviceService = inputDeviceService;
		}
	}
}
