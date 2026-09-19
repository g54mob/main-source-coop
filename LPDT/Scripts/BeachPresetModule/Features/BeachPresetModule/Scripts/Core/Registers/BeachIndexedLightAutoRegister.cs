using Features.BeachPresetModule.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core.Registers
{
	[DisallowMultipleComponent]
	public class BeachIndexedLightAutoRegister : MonoBehaviour
	{
		[Tooltip("Index referenced by LightingBehaviour indexed overrides on the active beach preset.")]
		[SerializeField]
		private int _lightIndex;

		[Tooltip("Light to register; if empty, uses a Light on this GameObject.")]
		[SerializeField]
		private Light _light;

		private BeachIndexedLightModel _model;

		[Inject]
		private void InjectDependencies(BeachIndexedLightModel model)
		{
			_model = model;
		}

		private void Awake()
		{
			Light light = ((_light != null) ? _light : GetComponent<Light>());
			if (!(light == null) && _lightIndex >= 0)
			{
				_model?.Register(_lightIndex, light);
			}
		}

		private void OnDestroy()
		{
			_model?.Unregister(_lightIndex);
		}
	}
}
