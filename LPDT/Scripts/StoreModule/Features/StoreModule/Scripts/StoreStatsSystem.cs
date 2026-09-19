using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Fusion;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreStatsSystem : MonoBehaviour
	{
		private const float SEATS_READY_TIMEOUT_SECONDS = 10f;

		[SerializeField]
		private List<Transform> _statsPositionsPlayer1;

		[SerializeField]
		private List<Transform> _statsPositionsPlayer2;

		[SerializeField]
		private List<Transform> _statsPositionsPlayer3;

		[SerializeField]
		private List<Transform> _statsPositionsPlayer4;

		[SerializeField]
		private GameObject _statsPrefab;

		private readonly List<GameObject> _createdStatsViews = new List<GameObject>();

		private WorldCanvasWindow _worldCanvasWindow;

		private IWindowsService _windowsService;

		private DiContainer _container;

		private MultiplayerModel _multiplayerModel;

		private IStoreSeatingService _storeSeatingService;

		private StoreStatsModel _storeStatsModel;

		private CancellationTokenSource _buildStatsViewsCts;

		[Inject]
		public void InjectDependencies(WorldCanvasWindow worldCanvasWindow, IWindowsService windowsService, DiContainer container, MultiplayerModel multiplayerModel, IStoreSeatingService storeSeatingService, StoreStatsModel storeStatsModel)
		{
			_worldCanvasWindow = worldCanvasWindow;
			_windowsService = windowsService;
			_container = container;
			_multiplayerModel = multiplayerModel;
			_storeSeatingService = storeSeatingService;
			_storeStatsModel = storeStatsModel;
		}

		private void OnEnable()
		{
			_storeStatsModel.Cleanup();
			_buildStatsViewsCts = new CancellationTokenSource();
			BuildStatsViewsWhenSeatsReadyAsync(_buildStatsViewsCts.Token).Forget();
		}

		private void OnDisable()
		{
			_buildStatsViewsCts?.Cancel();
			_buildStatsViewsCts?.Dispose();
			_buildStatsViewsCts = null;
			CleanupCreatedStatsViews();
		}

		private void CleanupCreatedStatsViews()
		{
			foreach (GameObject createdStatsView in _createdStatsViews)
			{
				if (createdStatsView != null)
				{
					Object.Destroy(createdStatsView);
				}
			}
			_createdStatsViews.Clear();
			_storeStatsModel.Cleanup();
		}

		private async UniTaskVoid BuildStatsViewsWhenSeatsReadyAsync(CancellationToken cancellationToken)
		{
			if (await WaitForActivePlayersSeatsReadyAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
			{
				EnsureWorldCanvasWindowOpen();
				BuildStatsViews();
			}
		}

		private async UniTask<bool> WaitForActivePlayersSeatsReadyAsync(CancellationToken cancellationToken)
		{
			float deadline = Time.unscaledTime + 10f;
			while (!cancellationToken.IsCancellationRequested && Time.unscaledTime < deadline)
			{
				if (AreActivePlayersSeatsReady())
				{
					return true;
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				Debug.LogWarning("StoreStatsSystem: timed out waiting for active players store seats before building stats views.");
			}
			return false;
		}

		private bool AreActivePlayersSeatsReady()
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				if (_storeSeatingService.GetSeatIndex(activePlayer.PlayerId) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private void BuildStatsViews()
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			List<Transform> list = PositionsForLocalSeat(_storeSeatingService.GetSeatIndex(playerId));
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(i);
				if (playerIdBySeat >= 0 && playerIdBySeat != playerId)
				{
					Transform transform = list[i];
					if (!(transform == null))
					{
						CreateStatsView(playerIdBySeat, transform.position);
					}
				}
			}
		}

		private void EnsureWorldCanvasWindowOpen()
		{
			if (_worldCanvasWindow.WindowStatus == WindowStatus.Closed)
			{
				_windowsService.OpenWindow<WorldCanvasWindow>();
			}
		}

		private List<Transform> PositionsForLocalSeat(int localSeat)
		{
			return localSeat switch
			{
				0 => _statsPositionsPlayer1, 
				1 => _statsPositionsPlayer2, 
				2 => _statsPositionsPlayer3, 
				3 => _statsPositionsPlayer4, 
				_ => null, 
			};
		}

		private void CreateStatsView(int playerId, Vector3 position)
		{
			if (_storeStatsModel.InitializedPlayerIds.Contains(playerId))
			{
				return;
			}
			GameObject gameObject = _container.InstantiatePrefab(_statsPrefab);
			_createdStatsViews.Add(gameObject);
			_worldCanvasWindow.AddView(gameObject.transform, worldPositionStays: false);
			gameObject.transform.position = position;
			foreach (StoreStatsViewBase item in gameObject.GetComponentsInChildren<StoreStatsViewBase>().ToList())
			{
				_worldCanvasWindow.GetPresenterForView<StoreStatsPresenter>(item).SetPlayerId(playerId);
			}
			gameObject.GetComponent<StoreStatsRotator>().SetPlayerId(playerId);
			_storeStatsModel.InitializedPlayerIds.Add(playerId);
		}
	}
}
