using UnityEngine;

namespace NomadDrive.Features.CustomEnviro
{
	public class EnviroLensFlareModuleBridge : MonoBehaviour
	{
		[SerializeField]
		private EnviroLensFlareModule lensFlareModule;

		private EnviroLensFlareModule _instance;

		private void OnEnable()
		{
			if (!(lensFlareModule == null))
			{
				_instance = Object.Instantiate(lensFlareModule);
				_instance.LoadModuleValues();
				_instance.Enable();
			}
		}

		private void Update()
		{
			_instance?.UpdateModule();
		}

		private void OnDisable()
		{
			if (_instance != null)
			{
				_instance.Disable();
				Object.Destroy(_instance);
				_instance = null;
			}
		}
	}
}
