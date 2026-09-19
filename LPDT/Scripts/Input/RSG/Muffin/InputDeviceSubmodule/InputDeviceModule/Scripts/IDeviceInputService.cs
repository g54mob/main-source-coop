using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public interface IDeviceInputService
	{
		InputVector2Actions AnyVectorChange { get; }
	}
}
