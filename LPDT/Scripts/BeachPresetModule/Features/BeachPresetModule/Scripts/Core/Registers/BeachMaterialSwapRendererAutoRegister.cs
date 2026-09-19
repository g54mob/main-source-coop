using Features.BeachPresetModule.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core.Registers
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	public class BeachMaterialSwapRendererAutoRegister : MonoBehaviour
	{
		private BeachMaterialSwapModel _materialSwapModel;

		private Renderer _renderer;

		[Inject]
		private void InjectDependencies(BeachMaterialSwapModel materialSwapModel)
		{
			_materialSwapModel = materialSwapModel;
		}

		private void Awake()
		{
			_renderer = GetComponent<Renderer>();
			_materialSwapModel?.Register(_renderer);
		}

		private void OnDestroy()
		{
			_materialSwapModel?.Unregister(_renderer);
		}
	}
}
