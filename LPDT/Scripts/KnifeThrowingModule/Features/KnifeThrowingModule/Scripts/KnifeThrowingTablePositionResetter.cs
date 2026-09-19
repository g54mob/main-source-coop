using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.KnifeThrowingModule.Scripts
{
	public class KnifeThrowingTablePositionResetter : MonoBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _bell;

		private IKnifeThrowingTableService _tableService;

		private MultiplayerModel _multiplayerModel;

		private bool _resetRequested;

		[Inject]
		private void InstallDependencies(IKnifeThrowingTableService tableService, MultiplayerModel multiplayerModel)
		{
			_tableService = tableService;
			_multiplayerModel = multiplayerModel;
		}

		private void OnEnable()
		{
			_bell.LocalOnGrab += OnBellGrabbed;
		}

		private void OnDisable()
		{
			_bell.LocalOnGrab -= OnBellGrabbed;
		}

		private void OnBellGrabbed(int i)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && !_resetRequested)
			{
				Debug.Log("[KnifeSpawn] Bell grabbed on master — requesting table object reset.");
				_resetRequested = true;
				ProcessResetRequestAsync().Forget();
			}
		}

		private async UniTaskVoid ProcessResetRequestAsync()
		{
			await UniTask.Yield(PlayerLoopTiming.Update);
			_resetRequested = false;
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient)
			{
				_tableService.TryResetObjectsPosition();
			}
		}
	}
}
