using Features.NetworkServices.Scripts.InterestManagement;
using Global.StateMachinesModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class GlobalSceneBootstrap : MonoBehaviour
	{
		[SerializeField]
		private GameObject _projectMetaDataOverlayPrefab;

		private GameFlowStateMachine _gameFlowStateMachine;

		private IObjectInterestService _objectInterestService;

		private DiContainer _diContainer;

		[Inject]
		public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine, IObjectInterestService objectInterestService, DiContainer diContainer)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_objectInterestService = objectInterestService;
			_diContainer = diContainer;
		}

		private void Start()
		{
			if (_projectMetaDataOverlayPrefab != null)
			{
				_diContainer.InstantiatePrefab(_projectMetaDataOverlayPrefab);
			}
			_objectInterestService.OverrideActiveAOILayer(AOILayer.Default);
			_gameFlowStateMachine.EnterState(GameFlowState.MenuGameState);
		}
	}
}
