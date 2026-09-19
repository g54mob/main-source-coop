using Zenject;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public interface IAdditionalInputInstaller
	{
		void CallInstall(DiContainer container);
	}
}
