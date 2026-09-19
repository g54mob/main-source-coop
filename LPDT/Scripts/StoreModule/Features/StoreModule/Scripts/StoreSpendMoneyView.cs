using System.Collections.Generic;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreSpendMoneyView : MonoBehaviour
	{
		[SerializeField]
		private StoreBuyingZone _buyingZone;

		[SerializeField]
		private StoreTableBehaviour _storeTableBehaviour;

		[SerializeField]
		private TMP_Text _moneyText;

		[SerializeField]
		private Transform _rotationTransform;

		[SerializeField]
		private float _spendLerpSpeed = 8f;

		[SerializeField]
		private float _fadeLerpSpeed = 8f;

		private PlayerMovableModel _playerMovableModel;

		private MultiplayerModel _multiplayerModel;

		private IStoreSeatingService _storeSeatingService;

		private PlayerCharacterMovableBase _localMovable;

		private int _zoneIndex = -1;

		private Color _baseTextColor;

		private float _displayedSpend;

		private int _targetSpend;

		private float _displayedAlpha;

		private float _targetAlpha;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel, IStoreSeatingService storeSeatingService)
		{
			_playerMovableModel = playerMovableModel;
			_multiplayerModel = multiplayerModel;
			_storeSeatingService = storeSeatingService;
		}

		private void Awake()
		{
			if (_rotationTransform == null)
			{
				_rotationTransform = base.transform;
			}
			if (_moneyText != null)
			{
				_baseTextColor = _moneyText.color;
			}
			CacheZoneIndex();
		}

		private void OnEnable()
		{
			_targetSpend = CalculateZoneSpend();
			_displayedSpend = _targetSpend;
			_targetAlpha = ((_targetSpend > 0) ? 1f : 0f);
			_displayedAlpha = _targetAlpha;
			ApplySpendText();
			if (!(_buyingZone == null))
			{
				_buyingZone.OnCardInStoreZone += OnZoneCardsChanged;
				_buyingZone.OnCardInStoreZoneExit += OnZoneCardsChanged;
			}
		}

		private void OnDisable()
		{
			if (_buyingZone != null)
			{
				_buyingZone.OnCardInStoreZone -= OnZoneCardsChanged;
				_buyingZone.OnCardInStoreZoneExit -= OnZoneCardsChanged;
			}
			_localMovable = null;
			_displayedSpend = 0f;
			_targetSpend = 0;
			_displayedAlpha = 0f;
			_targetAlpha = 0f;
		}

		private void Update()
		{
			RefreshSpendText();
		}

		private void LateUpdate()
		{
			RotateTowardsLocalPlayer();
		}

		private void OnZoneCardsChanged(StoreBuyingZone _, StoreCardBehaviour __)
		{
			RefreshSpendText();
		}

		private void RotateTowardsLocalPlayer()
		{
			if (_rotationTransform == null || !IsOwnedByLocalPlayer())
			{
				return;
			}
			Transform localPlayerTransform = GetLocalPlayerTransform();
			if (!(localPlayerTransform == null))
			{
				Vector3 vector = localPlayerTransform.position - _rotationTransform.position;
				Vector3 flatDirection = new Vector3(vector.x, 0f, vector.z);
				if (!(flatDirection.sqrMagnitude < 0.0001f))
				{
					Vector3 dominantWorldAxisDirection = GetDominantWorldAxisDirection(flatDirection);
					float y = Vector3.SignedAngle(Vector3.forward, dominantWorldAxisDirection, Vector3.up);
					_rotationTransform.rotation = Quaternion.Euler(0f, y, 0f);
				}
			}
		}

		private static Vector3 GetDominantWorldAxisDirection(Vector3 flatDirection)
		{
			if (Mathf.Abs(flatDirection.z) >= Mathf.Abs(flatDirection.x))
			{
				return new Vector3(0f, 0f, Mathf.Sign(flatDirection.z));
			}
			return new Vector3(Mathf.Sign(flatDirection.x), 0f, 0f);
		}

		private void CacheZoneIndex()
		{
			_zoneIndex = -1;
			if (_buyingZone == null || _storeTableBehaviour == null)
			{
				return;
			}
			IReadOnlyList<StoreBuyingZone> storeBuyingZones = _storeTableBehaviour.StoreBuyingZones;
			for (int i = 0; i < storeBuyingZones.Count; i++)
			{
				if (!(storeBuyingZones[i] != _buyingZone))
				{
					_zoneIndex = i;
					break;
				}
			}
		}

		private bool IsOwnedByLocalPlayer()
		{
			if (_zoneIndex < 0)
			{
				return false;
			}
			return _storeSeatingService.GetPlayerIdBySeat(_zoneIndex) == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
		}

		private Transform GetLocalPlayerTransform()
		{
			if (_multiplayerModel?.NetworkRunner == null)
			{
				return null;
			}
			if (_localMovable == null && !_playerMovableModel.AllCharacterMovables.TryGetValue(_multiplayerModel.NetworkRunner.LocalPlayer, out _localMovable))
			{
				return null;
			}
			if (!(_localMovable != null))
			{
				return null;
			}
			return _localMovable.RotatoblePart;
		}

		private void RefreshSpendText()
		{
			if (!(_moneyText == null))
			{
				_targetSpend = CalculateZoneSpend();
				_displayedSpend = Mathf.Lerp(_displayedSpend, _targetSpend, Time.deltaTime * _spendLerpSpeed);
				if (Mathf.Abs(_displayedSpend - (float)_targetSpend) < 0.5f)
				{
					_displayedSpend = _targetSpend;
				}
				_targetAlpha = ((_targetSpend > 0) ? 1f : 0f);
				_displayedAlpha = Mathf.Lerp(_displayedAlpha, _targetAlpha, Time.deltaTime * _fadeLerpSpeed);
				if (Mathf.Abs(_displayedAlpha - _targetAlpha) < 0.01f)
				{
					_displayedAlpha = _targetAlpha;
				}
				ApplySpendText();
			}
		}

		private void ApplySpendText()
		{
			bool flag = _targetSpend > 0 || _displayedSpend > 0.5f;
			_moneyText.SetText(flag ? $"-{Mathf.RoundToInt(_displayedSpend)}" : string.Empty);
			Color baseTextColor = _baseTextColor;
			baseTextColor.a = _displayedAlpha;
			_moneyText.color = baseTextColor;
		}

		private int CalculateZoneSpend()
		{
			if (_buyingZone == null)
			{
				return 0;
			}
			int num = 0;
			foreach (StoreCardBehaviour item in _buyingZone.CardsInZone)
			{
				if (!(item == null) && item.IsInitialized && !(item.Object == null) && item.IsActivated)
				{
					num += item.GetCost();
				}
			}
			return num;
		}
	}
}
