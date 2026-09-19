using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.ManInShadows
{
	public class ManInShadowPointRegistrar : MonoBehaviour
	{
		private ManInShadowsPointsModel _manInShadowsPointsModel;

		[Inject]
		private void InjectDependencies(ManInShadowsPointsModel manInShadowsPointsModel)
		{
			_manInShadowsPointsModel = manInShadowsPointsModel;
		}

		private void Start()
		{
			_manInShadowsPointsModel.RegisterPoint(base.transform);
		}

		private void OnDestroy()
		{
			_manInShadowsPointsModel.UnregisterPoint(base.transform);
		}
	}
}
