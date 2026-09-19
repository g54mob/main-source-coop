using System.Linq;
using Features.CameraModelModule;
using Features.ItemDamageModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Features.TutorialProgressModule.Scripts;
using Features.WorldTokenModule.Scripts;
using Features.WorldTokenModule.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts
{
	public class QuotaContainerTokenSpawner : MonoBehaviour
	{
		private IWorldTokenService _worldTokenService;

		private CameraModel _cameraModel;

		private PlayerMovableModel _playerMovableModel;

		private MultiplayerModel _multiplayerModel;

		private ItemDamageDisplayModel _itemDamageDisplayModel;

		private TutorialProgressModel _tutorialProgressModel;

		private ISavingService _savingService;

		private WorldTokenPresenter _worldTokenPresenter;

		private QuotaContainerQuotaStates _quotaContainerQuotaStates;

		private QuotaContainerModel _quotaContainerModel;

		private TutorialModel _tutorialModel;

		[Inject]
		private void InjectDependencies(IWorldTokenService worldTokenService, CameraModel cameraModel, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, ItemDamageDisplayModel itemDamageDisplayModel, QuotaContainerModel quotaContainerModel, TutorialProgressModel tutorialProgressModel, ISavingService savingService, TutorialModel tutorialModel)
		{
			_worldTokenService = worldTokenService;
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_itemDamageDisplayModel = itemDamageDisplayModel;
			_quotaContainerModel = quotaContainerModel;
			_tutorialProgressModel = tutorialProgressModel;
			_savingService = savingService;
			_tutorialModel = tutorialModel;
		}

		private void Update()
		{
			if (!_tutorialProgressModel.QuotaContainerTokenShown && !_tutorialModel.IsTutorialInProgress)
			{
				if (CheckForPlayerToStart())
				{
					_worldTokenPresenter = _worldTokenService.CreateWorldToken(WorldTokenType.Arrow, base.transform.position);
					_worldTokenPresenter.SetCameraToLookAt(_cameraModel.CameraObject);
					_worldTokenPresenter.StartMovement();
					_quotaContainerQuotaStates = QuotaContainerQuotaStates.Exist;
				}
				else if (CheckForPlayerToStop())
				{
					_worldTokenPresenter.StopMovement();
					_worldTokenPresenter.HideView();
					_tutorialProgressModel.MarkQuotaContainerTokenShown();
					_savingService.SaveDataForGroup(SavingGroup.TutorialProgress);
					_quotaContainerQuotaStates = QuotaContainerQuotaStates.None;
				}
				else if (CheckForPlayerToPause())
				{
					_worldTokenPresenter.StopMovement();
					_worldTokenPresenter.HideView();
					_quotaContainerQuotaStates = QuotaContainerQuotaStates.Paused;
				}
				else if (CheckForPlayerToResume())
				{
					_worldTokenPresenter.ShowView();
					_worldTokenPresenter.StartMovement();
					_quotaContainerQuotaStates = QuotaContainerQuotaStates.Exist;
				}
			}
		}

		private bool CheckForPlayerToStart()
		{
			if (_quotaContainerQuotaStates != QuotaContainerQuotaStates.None)
			{
				return false;
			}
			return _itemDamageDisplayModel.CurrentCurrency > 0;
		}

		private bool CheckForPlayerToStop()
		{
			if (_quotaContainerQuotaStates != QuotaContainerQuotaStates.Exist)
			{
				return false;
			}
			return _quotaContainerModel.FreeContainerItems.Any((QuotaContainerItemData i) => i.Item.NetworkObject != null && i.Item.NetworkObject.StateAuthority == _multiplayerModel.NetworkRunner.LocalPlayer);
		}

		private bool CheckForPlayerToPause()
		{
			if (_quotaContainerQuotaStates != QuotaContainerQuotaStates.Exist)
			{
				return false;
			}
			return _itemDamageDisplayModel.CurrentCurrency <= 0;
		}

		private bool CheckForPlayerToResume()
		{
			if (_quotaContainerQuotaStates != QuotaContainerQuotaStates.Paused)
			{
				return false;
			}
			return _itemDamageDisplayModel.CurrentCurrency > 0;
		}
	}
}
