using Features.LevelGatesModule.Data;
using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(GateCorridorLightBehaviour))]
	public class GateCorridorLightAutoRegister : MonoBehaviour
	{
		private GateCorridorLightModel _gateCorridorLightModel;

		private GateCorridorLightBehaviour _corridorLight;

		[Inject]
		private void InjectDependencies(GateCorridorLightModel gateCorridorLightModel)
		{
			_gateCorridorLightModel = gateCorridorLightModel;
		}

		private void Awake()
		{
			_corridorLight = GetComponent<GateCorridorLightBehaviour>();
		}

		private void OnEnable()
		{
			_gateCorridorLightModel?.Register(_corridorLight);
		}

		private void OnDisable()
		{
			_gateCorridorLightModel?.Unregister(_corridorLight);
		}
	}
}
