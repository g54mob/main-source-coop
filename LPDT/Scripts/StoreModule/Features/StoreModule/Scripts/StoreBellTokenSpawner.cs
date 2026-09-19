using System.Collections;
using Features.CameraModelModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.QuotaModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Features.TutorialProgressModule.Scripts;
using Features.WorldTokenModule.Scripts;
using Features.WorldTokenModule.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreBellTokenSpawner : MonoBehaviour
	{
		private const float REFERENCE_ASPECT_RATIO = 1.7777778f;

		[SerializeField]
		private StoreTableBehaviour _storeTableBehaviour;

		[SerializeField]
		private Transform[] _pointsForSpawnHint;

		[SerializeField]
		private float _delayForHint = 5f;

		[SerializeField]
		private Vector3 _hintArrowTipLocalOffsetAt16X9;

		private IWorldTokenService _worldTokenService;

		private CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		private IStoreSeatingService _storeSeatingService;

		private TutorialProgressModel _tutorialProgressModel;

		private ISavingService _savingService;

		private IStoreReadyRoster _storeReadyRoster;

		private StoreDraftMoneyModel _storeDraftMoneyModel;

		private CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private CardsOnTableModel _cardsOnTableModel;

		private TutorialModel _tutorialModel;

		private WorldTokenPresenter _worldTokenPresenter;

		private bool _isShowed;

		private bool _wereChanges;

		private Coroutine _delayCoroutine;

		private bool CanShowHint
		{
			get
			{
				if (_tutorialProgressModel.CardTableBellTokenShown)
				{
					return false;
				}
				if (_tutorialModel.IsTutorialInProgress)
				{
					return false;
				}
				if (_isShowed)
				{
					return false;
				}
				if (!IsSeatReady)
				{
					return false;
				}
				return true;
			}
		}

		private bool IsSeatReady
		{
			get
			{
				int localPlayerIndex = GetLocalPlayerIndex();
				if (localPlayerIndex >= 0)
				{
					return localPlayerIndex < _pointsForSpawnHint.Length;
				}
				return false;
			}
		}

		[Inject]
		private void InjectDependencies(IWorldTokenService worldTokenService, CameraModel cameraModel, MultiplayerModel multiplayerModel, IStoreSeatingService storeSeatingService, TutorialProgressModel tutorialProgressModel, ISavingService savingService, IStoreReadyRoster storeReadyRoster, StoreDraftMoneyModel storeDraftMoneyModel, CurrentWalletSynchronizedModel currentWalletSynchronizedModel, CardsOnTableModel cardsOnTableModel, TutorialModel tutorialModel)
		{
			_worldTokenService = worldTokenService;
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_storeSeatingService = storeSeatingService;
			_tutorialProgressModel = tutorialProgressModel;
			_savingService = savingService;
			_storeReadyRoster = storeReadyRoster;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_cardsOnTableModel = cardsOnTableModel;
			_tutorialModel = tutorialModel;
		}

		private void Start()
		{
			if (!_tutorialProgressModel.CardTableBellTokenShown && !_tutorialModel.IsTutorialInProgress)
			{
				StartCoroutine(CheckForDraftMoneyOnStart(_storeDraftMoneyModel.DraftMoney));
				_storeDraftMoneyModel.OnDraftMoneyChanged += CheckForDraftMoney;
				_storeTableBehaviour.OnSuccessesAddCartToStore += StartDelayForHint;
				_storeTableBehaviour.OnRemoveCardFromZone += OnRemoveCardFromZone;
			}
		}

		private void OnDestroy()
		{
			_storeDraftMoneyModel.OnDraftMoneyChanged -= CheckForDraftMoney;
			_storeTableBehaviour.OnSuccessesAddCartToStore -= StartDelayForHint;
			_storeTableBehaviour.OnRemoveCardFromZone -= OnRemoveCardFromZone;
		}

		private void StartDelayForHint(int zoneId)
		{
			if (CheckForPlayer(zoneId))
			{
				TryHideHintOnBell();
				if (_delayCoroutine != null)
				{
					StopCoroutine(_delayCoroutine);
				}
				_delayCoroutine = StartCoroutine(DelayForHintRoutine());
			}
		}

		private bool CheckForPlayer(int zoneId)
		{
			if (zoneId != GetLocalPlayerIndex())
			{
				return false;
			}
			return true;
		}

		private void OnRemoveCardFromZone(int zoneId, int cardsLeft)
		{
			if (CheckForPlayer(zoneId))
			{
				TryHideHintOnBell();
			}
			if (cardsLeft > 0)
			{
				StartDelayForHint(zoneId);
			}
		}

		private IEnumerator CheckForDraftMoneyOnStart(float draftMoney)
		{
			yield return new WaitForSeconds(1f);
			CheckForDraftMoney(draftMoney);
		}

		private void CheckForDraftMoney(float draftMoney)
		{
			if (_cardsOnTableModel.CardsOnTable.Count == 0)
			{
				TryShowHint();
				return;
			}
			foreach (StoreCardBehaviour item in _cardsOnTableModel.CardsOnTable)
			{
				if (!item.IsActivated && _currentWalletSynchronizedModel.CurrentSessionMoney - draftMoney >= (float)item.GetCost())
				{
					return;
				}
			}
			if (_delayCoroutine != null)
			{
				StopCoroutine(_delayCoroutine);
			}
			TryShowHint();
		}

		private IEnumerator DelayForHintRoutine()
		{
			yield return new WaitForSeconds(_delayForHint);
			TryShowHint();
			_delayCoroutine = null;
		}

		private void TryShowHint()
		{
			if (CanShowHint)
			{
				if (_worldTokenPresenter == null)
				{
					TryShowHintOnBellFirstTime();
				}
				else
				{
					TryShowHintAfterResume();
				}
				_isShowed = true;
			}
		}

		private void TryShowHintOnBellFirstTime()
		{
			_storeReadyRoster.OnPlayerReady += SaveOnPlayerReady;
			Vector3 positionForSpawn = GetPositionForSpawn();
			_worldTokenPresenter = _worldTokenService.CreateWorldToken(WorldTokenType.ArrowSmall, positionForSpawn);
			_worldTokenPresenter.SetCameraToLookAt(_cameraModel.CameraObject);
			PlaceHintAtBell(positionForSpawn);
			_worldTokenPresenter.StartMovement();
		}

		private void PlaceHintAtBell(Vector3 worldAnchor)
		{
			Vector3 aspectScaledTipOffset = GetAspectScaledTipOffset();
			PlaceAtAnchor(worldAnchor, aspectScaledTipOffset);
		}

		private Vector3 GetPositionForSpawn()
		{
			int localPlayerIndex = GetLocalPlayerIndex();
			return _pointsForSpawnHint[localPlayerIndex].position;
		}

		private int GetLocalPlayerIndex()
		{
			return _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
		}

		private void TryShowHintAfterResume()
		{
			Vector3 positionForSpawn = GetPositionForSpawn();
			PlaceHintAtBell(positionForSpawn);
			_worldTokenPresenter.ShowView();
			_worldTokenPresenter.StartMovement();
		}

		private void TryHideHintOnBell()
		{
			if (_worldTokenPresenter != null && _isShowed)
			{
				_isShowed = false;
				_worldTokenPresenter.StopMovement();
				_worldTokenPresenter.HideView();
			}
		}

		private void SaveOnPlayerReady(int playerID)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerID)
			{
				_storeReadyRoster.OnPlayerReady -= SaveOnPlayerReady;
				_tutorialProgressModel.MarkCardTableBellTokenShown();
				_savingService.SaveDataForGroup(SavingGroup.TutorialProgress);
				TryHideHintOnBell();
			}
		}

		private Vector3 GetAspectScaledTipOffset()
		{
			if (_hintArrowTipLocalOffsetAt16X9 == Vector3.zero)
			{
				return Vector3.zero;
			}
			float num = _cameraModel.CameraObject.aspect / 1.7777778f;
			return _hintArrowTipLocalOffsetAt16X9 * num;
		}

		private void PlaceAtAnchor(Vector3 worldAnchor, Vector3 tipOffsetFromPivotToTip)
		{
			Transform tokenTransform = _worldTokenPresenter.TokenTransform;
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject != null)
			{
				tokenTransform.rotation = cameraObject.transform.rotation;
			}
			tokenTransform.position = worldAnchor;
			if (!(tipOffsetFromPivotToTip == Vector3.zero))
			{
				tokenTransform.position -= tokenTransform.TransformVector(tipOffsetFromPivotToTip);
			}
		}
	}
}
