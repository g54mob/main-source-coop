using System.Collections;
using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class StoreActivationComponent_Tutorial : StoreActivationComponent
	{
		[SerializeField]
		private CardsJumper _cardsJumper;

		private StoreCardBehaviour _cachedCardToDeliver;

		private CardsOnTableModel _cardsOnTableModel;

		private IStoreSeatingService _storeSeatingService;

		private MultiplayerModel _multiplayerModel;

		private BaseTutorialStoreDataHolder _storeDataHolder;

		private TutorialStepsConfiguration _tutorialStepsConfiguration;

		[Inject]
		public void InjectDependencies(CardsOnTableModel cardsOnTableModel, IStoreSeatingService storeSeatingService, MultiplayerModel multiplayerModel, BaseTutorialStoreDataHolder storeDataHolder, TutorialStepsConfiguration tutorialStepsConfiguration)
		{
			_cardsOnTableModel = cardsOnTableModel;
			_storeSeatingService = storeSeatingService;
			_multiplayerModel = multiplayerModel;
			_storeDataHolder = storeDataHolder;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
		}

		protected override void OnRingGrabbed(int i)
		{
			if (base.IsReadyBlocked)
			{
				return;
			}
			int seatIndex = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			if (seatIndex < 0 || seatIndex >= _storeTableBehaviour.StoreBuyingZones.Count || _cardsOnTableModel.CardsOnTable.Where((StoreCardBehaviour x) => !x.IsActivated).ToList().Count == 0)
			{
				return;
			}
			StoreBuyingZone storeBuyingZone = _storeTableBehaviour.StoreBuyingZones[seatIndex];
			if (storeBuyingZone.CardsInZone.Count != 0)
			{
				SetReady();
				return;
			}
			if (_storeDataHolder.CurrentProcessedCard != null)
			{
				_cachedCardToDeliver = _storeDataHolder.CurrentProcessedCard;
			}
			else
			{
				StoreCardBehaviour storeCardBehaviour = _cardsOnTableModel.CardsOnTable.FirstOrDefault((StoreCardBehaviour x) => !x.IsActivated);
				if (storeCardBehaviour == null)
				{
					return;
				}
				_cachedCardToDeliver = storeCardBehaviour;
			}
			_cardsJumper.JumpCardToTarget(_cachedCardToDeliver, storeBuyingZone.CardPoint.First(), 0f);
			_cardsJumper.OnCardJumpFinished += OnCardJumpFinished;
			_cachedCardToDeliver.SimplePointGrabable.GrabBlocked = true;
		}

		protected override void SetReady()
		{
			if (!base.IsReadyBlocked)
			{
				_storeReadyModel.SetReady(isReady: true);
				_shopVotePersistence.SaveVote(hasVoted: true);
			}
		}

		private void OnCardJumpFinished(StoreCardBehaviour storeCardBehaviour)
		{
			if (!(_cachedCardToDeliver != storeCardBehaviour))
			{
				StartCoroutine(SetReadyDelayed(_tutorialStepsConfiguration.ReadyDelayIfCardAutomaticallySubmitted));
			}
		}

		private IEnumerator SetReadyDelayed(float delay)
		{
			yield return new WaitForSeconds(delay);
			SetReady();
		}
	}
}
