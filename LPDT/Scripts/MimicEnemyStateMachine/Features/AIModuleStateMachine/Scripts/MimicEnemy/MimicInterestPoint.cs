using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public class MimicInterestPoint : MonoBehaviour
	{
		[SerializeField]
		private int _priority;

		private MimicInterestPointsModel _mimicInterestPointsModel;

		public int PointPriority => _priority;

		[Inject]
		private void InjectDependencies(MimicInterestPointsModel model)
		{
			_mimicInterestPointsModel = model;
		}

		private void Awake()
		{
			_mimicInterestPointsModel.RegisterInterestPoint(this);
		}

		private void OnDestroy()
		{
			_mimicInterestPointsModel.UnregisterInterestPoint(this);
		}
	}
}
