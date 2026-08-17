using EvilCore.Audio;
using EvilCore.Extensions;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.CustomEnviro
{
	public class EnviroAmbientSoundModuleBridge : MonoBehaviour
	{
		[SerializeField]
		private EnviroAmbientSoundModule ambientSoundModule;

		[Inject]
		private IAudioManager _audioManager;

		private EnviroAmbientSoundModule _instance;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		private void Update()
		{
			if (_instance == null)
			{
				if (_audioManager == null || ambientSoundModule == null)
				{
					return;
				}
				_instance = Object.Instantiate(ambientSoundModule);
				_instance.Initialize(_audioManager, base.gameObject);
				_instance.LoadModuleValues();
				_instance.Enable();
			}
			_instance.UpdateModule();
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
