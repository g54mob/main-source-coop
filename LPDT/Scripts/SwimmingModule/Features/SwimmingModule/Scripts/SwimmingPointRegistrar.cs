using UnityEngine;
using Zenject;

namespace Features.SwimmingModule.Scripts
{
	public class SwimmingPointRegistrar : MonoBehaviour
	{
		[SerializeField]
		private GameObject _source;

		[SerializeField]
		private SwimmingPointType _swimmingPointType;

		private SwimmingFlowPointModel _swimmingFlowPointModel;

		[Inject]
		private void InjectDependencies(SwimmingFlowPointModel swimmingFlowPointModel)
		{
			_swimmingFlowPointModel = swimmingFlowPointModel;
		}

		private void Start()
		{
			if (_swimmingPointType != SwimmingPointType.None && !(_source == null))
			{
				_swimmingFlowPointModel.RegisterPoint(_source, _swimmingPointType, base.transform);
			}
		}
	}
}
