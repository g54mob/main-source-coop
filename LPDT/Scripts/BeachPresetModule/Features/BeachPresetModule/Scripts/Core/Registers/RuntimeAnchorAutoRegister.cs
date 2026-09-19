using Features.BeachPresetModule.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core.Registers
{
	public class RuntimeAnchorAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private Transform _anchor;

		private BeachPresetRuntimeModel _runtimeModel;

		[Inject]
		private void InjectDependencies(BeachPresetRuntimeModel runtimeModel)
		{
			_runtimeModel = runtimeModel;
		}

		private void Awake()
		{
			_runtimeModel.RegisterRuntimeAnchor(_anchor);
		}

		private void OnDestroy()
		{
			_runtimeModel.ClearRuntimeAnchor(_anchor);
		}
	}
}
