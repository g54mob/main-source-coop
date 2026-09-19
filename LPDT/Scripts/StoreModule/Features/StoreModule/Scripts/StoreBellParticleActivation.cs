using System.Collections.Generic;
using Features.QuotaModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreBellParticleActivation : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystem;

		private CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private StoreDraftMoneyModel _storeDraftMoneyModel;

		private CardsOnTableModel _cardsOnTableModel;

		private bool _isActivated;

		private readonly HashSet<StoreCardBehaviour> _boundCards = new HashSet<StoreCardBehaviour>();

		[Inject]
		public void InjectDependencies(CurrentWalletSynchronizedModel currentWalletSynchronizedModel, StoreDraftMoneyModel storeDraftMoneyModel, CardsOnTableModel cardsOnTableModel)
		{
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_cardsOnTableModel = cardsOnTableModel;
		}

		private void Start()
		{
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged += OnAffordabilityChanged;
			_storeDraftMoneyModel.OnDraftMoneyChanged += OnDraftMoneyChanged;
			_cardsOnTableModel.OnCardRegistered += BindCard;
			_cardsOnTableModel.OnCardUnregistered += UnbindCard;
			foreach (StoreCardBehaviour item in _cardsOnTableModel.CardsOnTable)
			{
				BindCard(item);
			}
			CheckCosts();
		}

		private void OnDestroy()
		{
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged -= OnAffordabilityChanged;
			_storeDraftMoneyModel.OnDraftMoneyChanged -= OnDraftMoneyChanged;
			_cardsOnTableModel.OnCardRegistered -= BindCard;
			_cardsOnTableModel.OnCardUnregistered -= UnbindCard;
			foreach (StoreCardBehaviour boundCard in _boundCards)
			{
				if (boundCard != null)
				{
					UnbindCardEvents(boundCard);
				}
			}
			_boundCards.Clear();
		}

		private void OnAffordabilityChanged(float _)
		{
			CheckCosts();
		}

		private void OnDraftMoneyChanged(float _)
		{
			CheckCosts();
		}

		private void BindCard(StoreCardBehaviour card)
		{
			if (!(card == null) && _boundCards.Add(card))
			{
				card.OnCardDataSet += CheckCosts;
				card.OnActivationChanged += CheckCosts;
				CheckCosts();
			}
		}

		private void UnbindCard(StoreCardBehaviour card)
		{
			if (!(card == null) && _boundCards.Remove(card))
			{
				UnbindCardEvents(card);
				CheckCosts();
			}
		}

		private void UnbindCardEvents(StoreCardBehaviour card)
		{
			card.OnCardDataSet -= CheckCosts;
			card.OnActivationChanged -= CheckCosts;
		}

		private void CheckCosts()
		{
			if (_cardsOnTableModel == null || _particleSystem == null)
			{
				return;
			}
			if (_cardsOnTableModel.CardsOnTable.Count == 0)
			{
				SetParticlesActive(isActive: false);
				return;
			}
			float num = _currentWalletSynchronizedModel.CurrentSessionMoney - _storeDraftMoneyModel.DraftMoney;
			bool flag = false;
			foreach (StoreCardBehaviour item in _cardsOnTableModel.CardsOnTable)
			{
				if (!(item == null) && !item.IsActivatedForPurchase && num >= (float)item.GetCost())
				{
					flag = true;
					break;
				}
			}
			SetParticlesActive(!flag);
		}

		private void SetParticlesActive(bool isActive)
		{
			if (_isActivated != isActive)
			{
				_isActivated = isActive;
				if (isActive)
				{
					_particleSystem.Play();
				}
				else
				{
					_particleSystem.Stop();
				}
			}
		}
	}
}
