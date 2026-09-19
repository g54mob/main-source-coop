using Features.InputModule.Scripts.Generated;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;

namespace Features.InputDeviceModuleRealization.Scripts
{
	public class DeviceInputService : IDeviceInputService
	{
		private readonly IInputService _inputService;

		public InputVector2Actions AnyVectorChange => _inputService.AnyVectorChange;

		public DeviceInputService(IInputService inputService)
		{
			_inputService = inputService;
		}
	}
}
