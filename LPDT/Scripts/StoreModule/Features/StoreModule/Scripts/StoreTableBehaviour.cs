using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AudioServiceModule.Scripts;
using Features.CartUpgradesModule.Scripts.Core;
using Features.LevelModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.QuotaModule.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StoreModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class StoreTableBehaviour : NetworkBehaviour
	{
		private const string CARD_COUNT_PARAM = "CardCount";

		[SerializeField]
		private List<StoreBuyingZone> _storeBuyingZones;

		[SerializeField]
		private TeleportationPointsRegistrar _seatPoints;

		[SerializeField]
		private float _cardSpawningRadius;

		[SerializeField]
		private Transform _cardSpawningCenter;

		[SerializeField]
		private float _groupSpacing = 10f;

		[SerializeField]
		private int _cardsPerGroup = 5;

		[SerializeField]
		private EventReference _cardSwooshSound;

		[SerializeField]
		private List<LightList> _playersLights;

		[SerializeField]
		private EventReference _successReference;

		[SerializeField]
		private EventReference _failureReference;

		[SerializeField]
		private AnimationCurve _fadeInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _fadeOutCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		[SerializeField]
		private AnimationCurve _cardFlyYCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _cardFlyHorizontalCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _closeDelay = 1.25f;

		[SerializeField]
		private float _disableLightsDelay = 0.4f;

		[SerializeField]
		private int _cardCountToPlayShuffleSound = 3;

		[SerializeField]
		private float _zoneReconcileInterval = 0.15f;

		[SerializeField]
		private float _zoneReconcileGraceAfterReserve = 0.35f;

		private StoreLevelConfiguration _storeLevelConfiguration;

		private StorePoolConfiguration _storePoolConfiguration;

		private LevelModel _levelModel;

		private CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private MultiplayerModel _multiplayerModel;

		private IStoreReadyRoster _storeReadyRoster;

		private bool _isActive = true;

		private int _deadPlayersCount;

		private int _notFullHpPlayersCount;

		private StoreDraftMoneyModel _storeDraftMoneyModel;

		private StoreRewardModel _storeRewardModel;

		private LineArmsModel _lineArmsModel;

		private StorePhaseModel _storePhaseModel;

		private NetworkRunnerEventBus _networkRunnerEventBus;

		private IStoreSeatingService _storeSeatingService;

		private PlayerMovableModel _playerMovableModel;

		private ICartUpgradeService _cartUpgradeService;

		private const float SEAT_PRESENCE_RADIUS = 0.01f;

		private IAudioService _audioService;

		private MonkeyPorterRunModel _monkeyPorterRunModel;

		private readonly HashSet<StoreCardBehaviour> _hostReservedCards = new HashSet<StoreCardBehaviour>();

		private readonly Dictionary<StoreCardBehaviour, int> _hostCardZoneByCard = new Dictionary<StoreCardBehaviour, int>();

		private readonly Dictionary<StoreCardBehaviour, float> _hostReservedAtTime = new Dictionary<StoreCardBehaviour, float>();

		private float _nextZoneReconcileTime;

		private const float SEATING_LIGHT_REFRESH_INTERVAL = 0.25f;

		private float _nextSeatingLightRefreshTime;

		private readonly List<int> _seatOccupancy = new List<int>();

		private readonly List<int> _seatOccupancyScratch = new List<int>();

		public IReadOnlyList<StoreBuyingZone> StoreBuyingZones => _storeBuyingZones;

		public event Action<int> OnSuccessesAddCartToStore;

		public event Action<int, int> OnRemoveCardFromZone;

		[Inject]
		public void InjectDependencies(StoreLevelConfiguration storeLevelConfiguration, StorePoolConfiguration storePoolConfiguration, LevelModel levelModel, CurrentWalletSynchronizedModel currentWalletSynchronizedModel, MultiplayerModel multiplayerModel, IStoreReadyRoster storeReadyRoster, StoreDraftMoneyModel storeDraftMoneyModel, StoreRewardModel storeRewardModel, LineArmsModel lineArmsModel, StorePhaseModel storePhaseModel, NetworkRunnerEventBus networkRunnerEventBus, IStoreSeatingService storeSeatingService, PlayerMovableModel playerMovableModel, IAudioService audioService, ICartUpgradeService cartUpgradeService, MonkeyPorterRunModel monkeyPorterRunModel)
		{
			_monkeyPorterRunModel = monkeyPorterRunModel;
			_storeLevelConfiguration = storeLevelConfiguration;
			_storePoolConfiguration = storePoolConfiguration;
			_levelModel = levelModel;
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_multiplayerModel = multiplayerModel;
			_storeReadyRoster = storeReadyRoster;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_storeRewardModel = storeRewardModel;
			_lineArmsModel = lineArmsModel;
			_storePhaseModel = storePhaseModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_storeSeatingService = storeSeatingService;
			_playerMovableModel = playerMovableModel;
			_audioService = audioService;
			_cartUpgradeService = cartUpgradeService;
		}

		public override void Spawned()
		{
			base.Spawned();
			RefreshPlayerLights();
			ReconcileZoneStates();
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_storePhaseModel.SetStoreActive(isActive: true);
				if (!_storeLevelConfiguration.StoreByLevelData.TryGetValue(_levelModel.CurrentLevel, out var value))
				{
					value = _storeLevelConfiguration.DefaultStoreByLevelData;
				}
				SpawnArcs(value, _deadPlayersCount, _notFullHpPlayersCount);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_isActive && !(runner == null) && runner.IsRunning && runner.IsSharedModeMasterClient)
			{
				_isActive = false;
				_storePhaseModel.SetStoreActive(isActive: false);
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			foreach (StoreBuyingZone storeBuyingZone in _storeBuyingZones)
			{
				storeBuyingZone.OnCardInStoreZone += StoreBuyingZoneOnCardInStoreZone;
			}
			foreach (StoreBuyingZone storeBuyingZone2 in _storeBuyingZones)
			{
				storeBuyingZone2.OnCardInStoreZoneExit += StoreBuyingZoneOnCardInStoreZoneExit;
			}
			_storeReadyRoster.OnPlayerIdsChanged += ChangeLight;
			_networkRunnerEventBus.Subscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			foreach (StoreBuyingZone storeBuyingZone in _storeBuyingZones)
			{
				storeBuyingZone.OnCardInStoreZone -= StoreBuyingZoneOnCardInStoreZone;
			}
			foreach (StoreBuyingZone storeBuyingZone2 in _storeBuyingZones)
			{
				storeBuyingZone2.OnCardInStoreZoneExit -= StoreBuyingZoneOnCardInStoreZoneExit;
			}
			_storeReadyRoster.OnPlayerIdsChanged -= ChangeLight;
			_networkRunnerEventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_hostReservedCards.Clear();
			_hostCardZoneByCard.Clear();
			_hostReservedAtTime.Clear();
			_nextZoneReconcileTime = 0f;
		}

		private void Update()
		{
			if (_isActive && !(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsRunning)
			{
				RefreshSeatingLightsIfChanged();
				if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && !(Time.time < _nextZoneReconcileTime))
				{
					_nextZoneReconcileTime = Time.time + _zoneReconcileInterval;
					ReconcileReservedCardsOutsideZones();
				}
			}
		}

		private void RefreshSeatingLightsIfChanged()
		{
			if (!(Time.time < _nextSeatingLightRefreshTime))
			{
				_nextSeatingLightRefreshTime = Time.time + 0.25f;
				_seatOccupancyScratch.Clear();
				for (int i = 0; i < _playersLights.Count; i++)
				{
					_seatOccupancyScratch.Add(_storeSeatingService.GetPlayerIdBySeat(i));
				}
				if (!SeatOccupancyEquals(_seatOccupancyScratch, _seatOccupancy))
				{
					_seatOccupancy.Clear();
					_seatOccupancy.AddRange(_seatOccupancyScratch);
					RefreshPlayerLights();
					ReconcileZoneStates();
				}
			}
		}

		private static bool SeatOccupancyEquals(List<int> a, List<int> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		private void ReconcileReservedCardsOutsideZones()
		{
			foreach (StoreCardBehaviour item in _hostReservedCards.ToList())
			{
				if (!item.IsActivated || (_hostReservedAtTime.TryGetValue(item, out var value) && Time.time - value < _zoneReconcileGraceAfterReserve))
				{
					continue;
				}
				if (!_hostCardZoneByCard.TryGetValue(item, out var value2) || value2 < 0 || value2 >= _storeBuyingZones.Count)
				{
					HostDeactivateCard(item, 0);
					continue;
				}
				StoreBuyingZone storeBuyingZone = _storeBuyingZones[value2];
				if (!storeBuyingZone.IsClosed && !storeBuyingZone.ContainsCard(item))
				{
					HostDeactivateCard(item, value2);
				}
			}
		}

		private void SpawnArcs(StoreByLevelData levelData, int deadPlayersCount, int notFullHpPlayersCount)
		{
			int num = 0;
			foreach (StoreArcData arc in levelData.Arcs)
			{
				num += arc.TotalCards;
			}
			float num2 = _groupSpacing * (float)levelData.Arcs.Count;
			float num3 = (360f - num2) / (float)num;
			float num4 = 0f;
			Dictionary<int, int> spawnedCounts = new Dictionary<int, int>();
			foreach (StoreArcData arc2 in levelData.Arcs)
			{
				foreach (int item in BuildArcCardSlots(arc2, levelData, deadPlayersCount, notFullHpPlayersCount, spawnedCounts))
				{
					float f = num4 * (MathF.PI / 180f);
					Vector3 vector = _cardSpawningCenter.position + new Vector3(Mathf.Cos(f) * _cardSpawningRadius, 0f, Mathf.Sin(f) * _cardSpawningRadius);
					Quaternion value = Quaternion.LookRotation(-(vector - _cardSpawningCenter.position).normalized);
					StoreCardBehaviour storeCardBehaviour = base.Runner.Spawn(_storePoolConfiguration.StoreCardPrefab, vector, value);
					switch (item)
					{
					case -1:
						storeCardBehaviour.SetDeadPlayerDataRPC();
						break;
					case -2:
						storeCardBehaviour.SetHealPotionDataRPC();
						break;
					default:
						storeCardBehaviour.SetDataRPC(item);
						break;
					}
					num4 += num3;
				}
				num4 += _groupSpacing;
			}
		}

		private List<int> BuildArcCardSlots(StoreArcData arcData, StoreByLevelData levelData, int deadPlayersCount, int notFullHpPlayersCount, Dictionary<int, int> spawnedCounts)
		{
			List<int> list = new List<int>();
			int num = Mathf.Min(GetConditionalCount(arcData.ConditionalCardType, deadPlayersCount, notFullHpPlayersCount), arcData.MaxConditionalCards);
			for (int i = 0; i < num; i++)
			{
				if (list.Count >= arcData.TotalCards)
				{
					break;
				}
				list.Add(GetConditionalSlot(arcData.ConditionalCardType));
			}
			foreach (KeyValuePair<CardItemType, int> item in arcData.MinimumPerType)
			{
				item.Deconstruct(out var key, out var value);
				CardItemType cardItemType = key;
				int num2 = value;
				for (int j = 0; j < num2; j++)
				{
					if (list.Count >= arcData.TotalCards)
					{
						break;
					}
					int randomCardId = GetRandomCardId(new CardItemType[1] { cardItemType }, levelData, spawnedCounts);
					list.Add(randomCardId);
					spawnedCounts.TryGetValue(randomCardId, out var value2);
					spawnedCounts[randomCardId] = value2 + 1;
				}
			}
			while (list.Count < arcData.TotalCards)
			{
				int randomCardId2 = GetRandomCardId(arcData.MinimumPerType.Keys.ToArray(), levelData, spawnedCounts);
				list.Add(randomCardId2);
				spawnedCounts.TryGetValue(randomCardId2, out var value3);
				spawnedCounts[randomCardId2] = value3 + 1;
			}
			return list;
		}

		private int GetConditionalCount(ConditionalCardType type, int deadPlayersCount, int notFullHpPlayersCount)
		{
			return type switch
			{
				ConditionalCardType.HealPotion => notFullHpPlayersCount, 
				ConditionalCardType.DeadPlayer => deadPlayersCount, 
				_ => 0, 
			};
		}

		private int GetConditionalSlot(ConditionalCardType type)
		{
			return type switch
			{
				ConditionalCardType.HealPotion => -2, 
				ConditionalCardType.DeadPlayer => -1, 
				_ => 0, 
			};
		}

		private int GetRandomCardId(CardItemType[] types, StoreByLevelData levelData, Dictionary<int, int> spawnedCounts)
		{
			List<(StoreCardData, int)> list = (from x in _storePoolConfiguration.RewardPoolWithChances.Select((StoreCardData data, int index) => (data: data, index: index))
				where types.Contains(x.data.GetRewardCardItemType())
				where IsCardDrawable(x.data)
				select x).Where(delegate((StoreCardData data, int index) x)
			{
				spawnedCounts.TryGetValue(x.index, out var value);
				return value < GetMaxDuplicates(x.data, levelData);
			}).ToList();
			if (list.Count == 0)
			{
				list = (from x in _storePoolConfiguration.RewardPoolWithChances.Select((StoreCardData data, int index) => (data: data, index: index))
					where types.Contains(x.data.GetRewardCardItemType())
					where IsCardDrawable(x.data)
					select x).ToList();
			}
			if (list.Count == 0)
			{
				Debug.LogError("StoreTableBehaviour: no drawable card for types [" + string.Join(", ", types) + "] — falling back to the heal potion. Check the reward pool for this arc.");
				return -2;
			}
			float maxInclusive = list.Sum<(StoreCardData, int)>(((StoreCardData data, int index) x) => GetEffectiveWeight(x.data, levelData));
			float num = UnityEngine.Random.Range(0f, maxInclusive);
			float num2 = 0f;
			foreach (var item3 in list)
			{
				StoreCardData item = item3.Item1;
				int item2 = item3.Item2;
				num2 += GetEffectiveWeight(item, levelData);
				if (num <= num2)
				{
					return item2;
				}
			}
			List<(StoreCardData, int)> list2 = list;
			return list2[list2.Count - 1].Item2;
		}

		private bool IsCardDrawable(StoreCardData card)
		{
			StoreRewardDataBase rewardData = card.GetRewardData();
			if (rewardData is CartUpgradeStoreRewardData cartUpgradeStoreRewardData)
			{
				return _cartUpgradeService.IsModulePurchasable(cartUpgradeStoreRewardData.TargetTier);
			}
			if (rewardData is SpawnMonkeyPorterStoreRewardData)
			{
				return !_monkeyPorterRunModel.IsOwned.Value;
			}
			return true;
		}

		private float GetEffectiveWeight(StoreCardData card, StoreByLevelData levelData)
		{
			if (levelData.LevelRewardsSettings != null)
			{
				foreach (LevelRewardSetting levelRewardsSetting in levelData.LevelRewardsSettings)
				{
					if (levelRewardsSetting.CardId == card.Id)
					{
						return levelRewardsSetting.Weight;
					}
				}
			}
			return 1f;
		}

		private int GetMaxDuplicates(StoreCardData card, StoreByLevelData levelData)
		{
			if (levelData.LevelRewardsSettings != null)
			{
				foreach (LevelRewardSetting levelRewardsSetting in levelData.LevelRewardsSettings)
				{
					if (levelRewardsSetting.CardId == card.Id)
					{
						return levelRewardsSetting.MaxDuplicates;
					}
				}
			}
			return 1;
		}

		private void StoreBuyingZoneOnCardInStoreZoneExit(StoreBuyingZone storeBuyingZone, StoreCardBehaviour storeCardBehaviour)
		{
			if (storeBuyingZone.IsClosed)
			{
				return;
			}
			storeBuyingZone.CardsInZone.Remove(storeCardBehaviour);
			int num = _storeBuyingZones.IndexOf(storeBuyingZone);
			this.OnRemoveCardFromZone?.Invoke(num, storeBuyingZone.CardsInZone.Count);
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				if (_hostReservedCards.Contains(storeCardBehaviour))
				{
					HostDeactivateCard(storeCardBehaviour, num);
				}
				else if (storeCardBehaviour.IsActivated)
				{
					storeCardBehaviour.DeactivateCard();
					_storeRewardModel.RemoveRewardsForCard(storeCardBehaviour);
				}
			}
		}

		private void StoreBuyingZoneOnCardInStoreZone(StoreBuyingZone storeBuyingZone, StoreCardBehaviour storeCardBehaviour)
		{
			if (storeBuyingZone.IsClosed)
			{
				return;
			}
			if (!storeBuyingZone.CardsInZone.Contains(storeCardBehaviour))
			{
				storeBuyingZone.CardsInZone.Add(storeCardBehaviour);
			}
			int num = _storeBuyingZones.IndexOf(storeBuyingZone);
			float num2 = storeCardBehaviour.GetCost();
			int playerIdByBuyingZoneIndex = GetPlayerIdByBuyingZoneIndex(num);
			bool flag = _currentWalletSynchronizedModel.CurrentSessionMoney - _storeDraftMoneyModel.DraftMoney >= num2 && _isActive && _storePhaseModel.IsStoreActive.Value && playerIdByBuyingZoneIndex >= 0;
			bool isSharedModeMasterClient = _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
			bool hasStateAuthority = storeCardBehaviour.HasStateAuthority;
			if (isSharedModeMasterClient && storeCardBehaviour.IsActivated && _hostReservedCards.Contains(storeCardBehaviour))
			{
				return;
			}
			if (!flag)
			{
				if (isSharedModeMasterClient)
				{
					ReportPurchaseFailure(num, storeCardBehaviour.Object.Id, storeCardBehaviour);
				}
				return;
			}
			this.OnSuccessesAddCartToStore?.Invoke(num);
			if (hasStateAuthority)
			{
				ApplyLocalPurchaseOnCardAuthority(storeCardBehaviour, storeBuyingZone, num2, isSharedModeMasterClient);
			}
			if (isSharedModeMasterClient)
			{
				if (_hostReservedCards.Contains(storeCardBehaviour))
				{
					_hostReservedCards.Remove(storeCardBehaviour);
					_hostCardZoneByCard.Remove(storeCardBehaviour);
				}
				if (storeCardBehaviour.IsActivated)
				{
					storeCardBehaviour.DeactivateCard();
					_storeRewardModel.RemoveRewardsForCard(storeCardBehaviour);
				}
				SuccessRPC(num);
				_storeDraftMoneyModel.AddDraftMoney(num2);
				storeCardBehaviour.ActivateCard(playerIdByBuyingZoneIndex);
				_storeRewardModel.AddReward(storeCardBehaviour, playerIdByBuyingZoneIndex);
				_hostReservedCards.Add(storeCardBehaviour);
				_hostCardZoneByCard[storeCardBehaviour] = num;
				_hostReservedAtTime[storeCardBehaviour] = Time.time;
			}
		}

		private void ApplyLocalPurchaseOnCardAuthority(StoreCardBehaviour storeCardBehaviour, StoreBuyingZone storeBuyingZone, float cardCost, bool isMaster)
		{
			if (!isMaster)
			{
				_storeDraftMoneyModel.AddDraftMoneyLocal(cardCost);
			}
			MoveCardToNearestZoneSlot(storeCardBehaviour, storeBuyingZone);
		}

		private static void MoveCardToNearestZoneSlot(StoreCardBehaviour storeCardBehaviour, StoreBuyingZone storeBuyingZone)
		{
			int nearestCardPointIndex = GetNearestCardPointIndex(storeBuyingZone, storeCardBehaviour.Rigidbody.position);
			if (nearestCardPointIndex >= 0)
			{
				Transform transform = storeBuyingZone.CardPoint[nearestCardPointIndex];
				storeCardBehaviour.MoveToWorldTarget(transform.position, transform.eulerAngles.y);
			}
		}

		private static int GetNearestCardPointIndex(StoreBuyingZone storeBuyingZone, Vector3 fromPosition)
		{
			if (storeBuyingZone.CardPoint == null || storeBuyingZone.CardPoint.Count == 0)
			{
				return -1;
			}
			int result = 0;
			float num = float.MaxValue;
			for (int i = 0; i < storeBuyingZone.CardPoint.Count; i++)
			{
				Transform transform = storeBuyingZone.CardPoint[i];
				if (!(transform == null))
				{
					float num2 = Vector3.Distance(transform.position, fromPosition);
					if (!(num2 >= num))
					{
						num = num2;
						result = i;
					}
				}
			}
			if (!(num < float.MaxValue))
			{
				return -1;
			}
			return result;
		}

		private int GetPlayerIdByBuyingZoneIndex(int buyingZoneIndex)
		{
			return _storeSeatingService.GetPlayerIdBySeat(buyingZoneIndex);
		}

		private void HostDeactivateCard(StoreCardBehaviour storeCardBehaviour, int zoneIndex)
		{
			_hostReservedCards.Remove(storeCardBehaviour);
			_hostCardZoneByCard.Remove(storeCardBehaviour);
			_hostReservedAtTime.Remove(storeCardBehaviour);
			if (zoneIndex >= 0 && zoneIndex < _storeBuyingZones.Count)
			{
				_storeBuyingZones[zoneIndex].CardsInZone.Remove(storeCardBehaviour);
			}
			storeCardBehaviour.DeactivateCard();
			_storeDraftMoneyModel.RemoveDraftMoney(storeCardBehaviour.GetCost());
			_storeRewardModel.RemoveReward(storeCardBehaviour, GetPlayerIdByBuyingZoneIndex(zoneIndex));
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2386621145u)]
		private void SuccessRPC([RpcPayload(4)] int indexOf)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2386621145u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreTableBehaviour::SuccessRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(indexOf, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Light successLight = _storeBuyingZones[indexOf].SuccessLight;
			_audioService.PlayOneShotAttached(_successReference, _storeBuyingZones[indexOf].SoundSourceBehaviour);
			if (successLight != null)
			{
				StartCoroutine(LightImpulseCoroutine(successLight));
			}
		}

		private void ReportPurchaseFailure(int indexOf, NetworkId cardNetworkId, StoreCardBehaviour storeCardBehaviour)
		{
			storeCardBehaviour.ApplyRandomForce();
			_storeDraftMoneyModel.SynchronizeDraftMoney();
			FailureRPC(indexOf);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3103694734u)]
		private void FailureRPC([RpcPayload(4)] int indexOf)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3103694734u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreTableBehaviour::FailureRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(indexOf, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Light failureLight = _storeBuyingZones[indexOf].FailureLight;
			_audioService.PlayOneShotAttached(_failureReference, _storeBuyingZones[indexOf].SoundSourceBehaviour);
			_storeDraftMoneyModel.OnDraftFailureInvoke();
			if (failureLight != null)
			{
				StartCoroutine(LightImpulseCoroutine(failureLight));
			}
		}

		private IEnumerator LightImpulseCoroutine(Light targetLight)
		{
			float duration = 0.3f;
			float elapsedTime = 0f;
			targetLight.gameObject.SetActive(value: true);
			float fadeInDuration = duration * 0.3f;
			while (elapsedTime < fadeInDuration)
			{
				elapsedTime += Time.deltaTime;
				float time = elapsedTime / fadeInDuration;
				targetLight.intensity = _fadeInCurve.Evaluate(time);
				yield return null;
			}
			elapsedTime = 0f;
			float fadeOutDuration = duration * 0.7f;
			while (elapsedTime < fadeOutDuration)
			{
				elapsedTime += Time.deltaTime;
				float time2 = elapsedTime / fadeOutDuration;
				targetLight.intensity = _fadeOutCurve.Evaluate(time2);
				yield return null;
			}
			targetLight.gameObject.SetActive(value: false);
		}

		public void Deactivate()
		{
			_isActive = false;
			_storeReadyRoster.OnPlayerIdsChanged -= ChangeLight;
			_networkRunnerEventBus.Unsubscribe<OnPlayerJoinedEvent>(OnPlayerJoined);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_hostReservedCards.Clear();
			_hostCardZoneByCard.Clear();
			_hostReservedAtTime.Clear();
			_nextZoneReconcileTime = 0f;
		}

		public void SetDeadPlayers(int deadPlayersCount)
		{
			_deadPlayersCount = deadPlayersCount;
		}

		public void SetNotFullHpPlayersCount(int count)
		{
			_notFullHpPlayersCount = count;
		}

		private void ChangeLight()
		{
			if (_storeReadyRoster.ReadyPlayers.Count == 0)
			{
				RefreshPlayerLights();
				ReconcileZoneStates();
				return;
			}
			for (int i = 0; i < _storeBuyingZones.Count; i++)
			{
				if (_storeBuyingZones[i].IsClosed)
				{
					continue;
				}
				int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(i);
				if (playerIdBySeat >= 0)
				{
					bool flag = _storeReadyRoster.IsPlayerReady(playerIdBySeat);
					if (flag)
					{
						ProcessCloseAnimation(i, flag);
					}
				}
			}
			ReconcileZoneStates();
		}

		private void RefreshPlayerLights()
		{
			for (int i = 0; i < _playersLights.Count; i++)
			{
				int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(i);
				bool num = playerIdBySeat >= 0;
				bool flag = num && _storeReadyRoster.IsPlayerReady(playerIdBySeat);
				bool active = num && !flag;
				foreach (Light light in _playersLights[i].Lights)
				{
					light.gameObject.SetActive(active);
				}
			}
		}

		private void OnPlayerJoined(OnPlayerJoinedEvent playerJoinedEvent)
		{
			RefreshPlayerLights();
			ReconcileZoneStates();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent playerLeftEvent)
		{
			RefreshPlayerLights();
			ReconcileZoneStates();
		}

		private bool IsSeatRosterComplete()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (_storeSeatingService.GetSeatIndex(activePlayer.PlayerId) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private void ReconcileZoneStates()
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning)
			{
				return;
			}
			bool flag = IsSeatRosterComplete();
			for (int i = 0; i < _storeBuyingZones.Count; i++)
			{
				int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(i);
				bool num = playerIdBySeat >= 0;
				bool flag2 = num && _storeReadyRoster.IsPlayerReady(playerIdBySeat);
				bool num2 = num && !flag2;
				StoreBuyingZone storeBuyingZone = _storeBuyingZones[i];
				if (num2)
				{
					storeBuyingZone.OpenBuyingZone();
				}
				else if (flag)
				{
					storeBuyingZone.CloseBuyingZone();
				}
			}
		}

		public bool TryVerifyLocalSeatSpendable(int localPlayerId, out string problem)
		{
			int seatIndex = _storeSeatingService.GetSeatIndex(localPlayerId);
			if (seatIndex < 0)
			{
				problem = "not seated at the store table";
				return false;
			}
			if (_storeReadyRoster.IsPlayerReady(localPlayerId))
			{
				problem = null;
				return true;
			}
			if (seatIndex >= _storeBuyingZones.Count)
			{
				problem = $"seat {seatIndex} has no buying zone";
				return false;
			}
			if (_storeBuyingZones[seatIndex].IsClosed)
			{
				problem = $"buying zone for seat {seatIndex} is closed — seated and not ready but cannot spend";
				return false;
			}
			if (seatIndex >= _playersLights.Count)
			{
				problem = $"seat {seatIndex} has no lamp";
				return false;
			}
			if (!AnySeatLightActive(seatIndex))
			{
				problem = $"seat {seatIndex} lamp is off while seated and not ready";
				return false;
			}
			problem = null;
			return true;
		}

		private bool AnySeatLightActive(int seatIndex)
		{
			foreach (Light light in _playersLights[seatIndex].Lights)
			{
				if (light != null && light.gameObject.activeSelf)
				{
					return true;
				}
			}
			return false;
		}

		public bool TryVerifyAllSeatedAvatarsPresent(out string problem)
		{
			List<string> list = new List<string>();
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				int seatIndex = _storeSeatingService.GetSeatIndex(activePlayer.PlayerId);
				if (seatIndex < 0 || seatIndex >= _storeBuyingZones.Count)
				{
					continue;
				}
				if (!_playerMovableModel.AllCharacterMovables.TryGetValue(activePlayer, out var value) || value == null)
				{
					list.Add($"player {activePlayer.PlayerId} seated at {seatIndex} has no live body");
					continue;
				}
				if (!TryGetSeatPose(seatIndex, out var position, out var _))
				{
					list.Add($"player {activePlayer.PlayerId} is seated at {seatIndex}, which has no authored seat point");
					continue;
				}
				Vector3 position2 = value.GetPosition();
				float num = position2.x - position.x;
				float num2 = position2.z - position.z;
				float num3 = Mathf.Sqrt(num * num + num2 * num2);
				if (num3 > 0.01f)
				{
					list.Add($"player {activePlayer.PlayerId} avatar {num3:F3}m from seat {seatIndex}");
				}
			}
			if (list.Count > 0)
			{
				problem = string.Join("; ", list);
				return false;
			}
			problem = null;
			return true;
		}

		public bool TryGetSeatPose(int seatIndex, out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			if (_seatPoints == null)
			{
				return false;
			}
			List<Transform> teleportationPoints = _seatPoints.TeleportationPoints;
			if (teleportationPoints == null || seatIndex < 0 || seatIndex >= teleportationPoints.Count)
			{
				return false;
			}
			Transform transform = teleportationPoints[seatIndex];
			if (transform == null)
			{
				return false;
			}
			position = transform.position;
			rotation = transform.rotation;
			return true;
		}

		public bool TryVerifyLocalSeatedAvatarPresent(out string problem)
		{
			PlayerRef localPlayer = _multiplayerModel.NetworkRunner.LocalPlayer;
			int seatIndex = _storeSeatingService.GetSeatIndex(localPlayer.PlayerId);
			if (seatIndex < 0 || seatIndex >= _storeBuyingZones.Count)
			{
				problem = null;
				return true;
			}
			if (!_playerMovableModel.AllCharacterMovables.TryGetValue(localPlayer, out var value) || value == null)
			{
				problem = $"local player seated at {seatIndex} has no live body";
				return false;
			}
			if (!TryGetSeatPose(seatIndex, out var position, out var _))
			{
				problem = $"local player is seated at {seatIndex}, which has no authored seat point";
				return false;
			}
			Vector3 position2 = value.GetPosition();
			float num = position2.x - position.x;
			float num2 = position2.z - position.z;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 > 0.01f)
			{
				problem = $"local avatar {num3:F3}m from seat {seatIndex}";
				return false;
			}
			problem = null;
			return true;
		}

		private void ProcessCloseAnimation(int index, bool isReady)
		{
			StartCoroutine(CloseAnimationCoroutine(isReady, index));
			foreach (StoreCardBehaviour item in _storeBuyingZones[index].CardsInZone)
			{
				if (!isReady)
				{
					break;
				}
				if (item.HasStateAuthority)
				{
					item.SimplePointGrabable.BlockGrabRPC();
					item.SimplePointGrabable.Rigidbody.isKinematic = true;
				}
			}
			int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(index);
			if (playerIdBySeat == base.Runner.LocalPlayer.PlayerId)
			{
				_lineArmsModel.GetAllLineArmsForPlayer(playerIdBySeat)[LineArmType.RightArmByMouse].UnJoinAll(throwItem: false);
				_lineArmsModel.GetAllLineArmsForPlayer(playerIdBySeat)[LineArmType.RightArmByMouse].EnableGrabbing(!isReady);
			}
		}

		private IEnumerator CloseAnimationCoroutine(bool isReady, int index)
		{
			if (isReady)
			{
				_storeBuyingZones[index].CloseBuyingZone();
			}
			float timer = 0f;
			float duration = _closeDelay;
			Dictionary<StoreCardBehaviour, Vector3> startPositions = new Dictionary<StoreCardBehaviour, Vector3>();
			Dictionary<StoreCardBehaviour, Vector3> endPositions = new Dictionary<StoreCardBehaviour, Vector3>();
			Dictionary<StoreCardBehaviour, float> horizontalMultipliers = new Dictionary<StoreCardBehaviour, float>();
			Dictionary<StoreCardBehaviour, float> verticalMultipliers = new Dictionary<StoreCardBehaviour, float>();
			int count = _storeBuyingZones[index].CardsInZone.Count;
			int num = 0;
			if (_storeBuyingZones[index].CardsInZone.Count != 0)
			{
				EventInstance soundInstance = _audioService.CreateInstance(_cardSwooshSound);
				if (_storeBuyingZones[index].CardsInZone.Count >= _cardCountToPlayShuffleSound)
				{
					soundInstance.setParameterByName("CardCount", 1f);
				}
				_audioService.StartInstanceWith3DAttributes(soundInstance, _storeBuyingZones[index].SoundSourceBehaviour);
				_audioService.ReleaseInstance(soundInstance);
			}
			Vector3 position = _storeBuyingZones[index].TargetTransform.position;
			foreach (StoreCardBehaviour item in _storeBuyingZones[index].CardsInZone)
			{
				if (item.IsActivated)
				{
					Vector3 position2 = item.Visual.transform.position;
					startPositions[item] = position2;
					float num2 = (float)num - (float)(count - 1) * 0.5f;
					horizontalMultipliers[item] = num2 * 0.35f;
					verticalMultipliers[item] = 1f + Mathf.Abs(num2) * 0.15f;
					endPositions[item] = position;
					position += Vector3.up * 0.01f;
					num++;
				}
			}
			while (timer < duration)
			{
				timer += Time.deltaTime;
				foreach (StoreCardBehaviour item2 in _storeBuyingZones[index].CardsInZone)
				{
					if (item2.IsActivated && startPositions.TryGetValue(item2, out var value) && endPositions.TryGetValue(item2, out var value2) && horizontalMultipliers.TryGetValue(item2, out var value3) && verticalMultipliers.TryGetValue(item2, out var value4))
					{
						float num3 = Mathf.Clamp01(timer / duration);
						Vector3 position3 = Vector3.Lerp(value, value2, num3);
						float num4 = _cardFlyHorizontalCurve.Evaluate(num3) * value3;
						float num5 = _cardFlyYCurve.Evaluate(num3) * value4;
						position3.y = value2.y + num5;
						position3 += _storeBuyingZones[index].TargetTransform.right * num4;
						item2.Visual.transform.position = position3;
					}
				}
				yield return null;
			}
			yield return new WaitForSeconds(_disableLightsDelay);
			foreach (Light light in _playersLights[index].Lights)
			{
				light.gameObject.SetActive(!isReady);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2386621145u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SuccessRPC_0040Invoker2386621145([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreTableBehaviour)context.TargetBehaviour).SuccessRPC(value);
		}

		[NetworkRpcWeavedInvoker(3103694734u)]
		[Preserve]
		[WeaverGenerated]
		protected static void FailureRPC_0040Invoker3103694734([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreTableBehaviour)context.TargetBehaviour).FailureRPC(value);
		}
	}
}
