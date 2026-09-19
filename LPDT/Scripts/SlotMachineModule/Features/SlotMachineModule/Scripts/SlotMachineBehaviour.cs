using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.SlotMachineModule.Scripts.Rewards;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.SlotMachineModule.Scripts
{
	[NetworkBehaviourWeaved(16)]
	public class SlotMachineBehaviour : NetworkBehaviour, ISlotMachinePayoutBudget
	{
		private struct ZoneItemCache
		{
			public IItem Item;

			public int ColliderCount;
		}

		private const int REEL_COUNT = 3;

		private const int SYMBOL_COUNT = 8;

		private const int JUNK_EJECT_INTERVAL_TICKS = 6;

		private static readonly int OpenHash = Animator.StringToHash("Open");

		private static readonly int CloseHash = Animator.StringToHash("Close");

		private static readonly int DestroyHash = Animator.StringToHash("Destroy");

		[SerializeField]
		private Transform[] _reelTransforms;

		[SerializeField]
		private Animator _boxAnimator;

		[SerializeField]
		private Transform _ejectOrigin;

		[Tooltip("Where reward items are spawned and launched from. Falls back to Eject Origin when unset.")]
		[SerializeField]
		private Transform _payoutOrigin;

		[SerializeField]
		private float _symbolStepDegrees = 45f;

		[SerializeField]
		private float _symbol1LocalEulerX;

		[SerializeField]
		private int _minFullSpins = 4;

		[SerializeField]
		private int _maxFullSpins = 7;

		[SerializeField]
		private float _baseSpinDuration = 1.5f;

		[SerializeField]
		private float _reelStopStagger = 0.35f;

		[SerializeField]
		private float _ejectForce = 6f;

		[SerializeField]
		private float _ejectUpwardBias = 1.25f;

		[SerializeField]
		private AnimationCurve _spinEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[Header("Win rate")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _winChance = 0.25f;

		[SerializeField]
		private int _minSpinsUntilGuaranteedWin = 2;

		[SerializeField]
		private int _maxSpinsUntilGuaranteedWin = 4;

		[Header("Bet")]
		[Tooltip("How many coins may be inserted before a spin. The reward scales with the accepted count.")]
		[Min(1f)]
		[SerializeField]
		private int _maxBetCoins = 5;

		[Header("Reward")]
		[Tooltip("Reward push strength relative to a junk eject. Above 1 throws further than ejected items.")]
		[Min(0f)]
		[SerializeField]
		private float _rewardEjectForceMultiplier = 0.5f;

		[Tooltip("Launch angle above the horizon, in degrees. 0 shoots flat forward, 90 straight up.")]
		[Range(0f, 90f)]
		[SerializeField]
		private float _rewardEjectAngleDegrees = 40f;

		[Tooltip("Random left/right spread around forward, per item. 45 gives a wide fan in front of the machine.")]
		[Range(0f, 180f)]
		[SerializeField]
		private float _rewardEjectSpreadDegrees = 45f;

		[Tooltip("Random variation of the launch angle per item, so coins do not share one trajectory.")]
		[Range(0f, 45f)]
		[SerializeField]
		private float _rewardEjectAngleSpreadDegrees = 12f;

		[Tooltip("Pause between the reels stopping and the first item flying out — lets the win read before the payout.")]
		[Min(0f)]
		[SerializeField]
		private float _payoutStartDelay = 1.5f;

		[Tooltip("Machine stays locked this long after the last item, so the payout reads as finished.")]
		[Min(0f)]
		[SerializeField]
		private float _payoutHoldDelay = 0.5f;

		[Tooltip("Safety net: a payout is never allowed to lock the machine longer than this.")]
		[Min(1f)]
		[SerializeField]
		private float _payoutTimeout = 15f;

		[Tooltip("Where every slot machine particle plays. Falls back to Payout Origin when unset.")]
		[SerializeField]
		private Transform _particleOrigin;

		[Tooltip("Played on every peer at the spawn point of each reward item.")]
		[SerializeField]
		private ParticleSystem _payoutParticlePrefab;

		[Tooltip("Played at the payout origin the moment the reels stop on a winning combination.")]
		[SerializeField]
		private ParticleSystem _winParticlePrefab;

		[Tooltip("Played at the payout origin the moment the reels stop without a combination.")]
		[SerializeField]
		private ParticleSystem _loseParticlePrefab;

		[Header("Break")]
		[Tooltip("Lever network object. It is despawned when the machine breaks.")]
		[SerializeField]
		private NetworkObject _leverNetworkObject;

		[Tooltip("Played where the lever was, at the moment the machine breaks.")]
		[SerializeField]
		private ParticleSystem _destroyParticlePrefab;

		[Tooltip("How long a paid out item is ignored by the intake zone, so the machine cannot play itself.")]
		[Min(0f)]
		[SerializeField]
		private float _rewardItemIgnoreDuration = 5f;

		[Header("Audio")]
		[Tooltip("Looping spin bed. Starts with the networked spin on every peer and stops when the reels do.")]
		[SerializeField]
		private EventReference _spinSound;

		[Tooltip("One-shot when the lever actually starts a spin. Plays on every peer from SpinVersion.")]
		[SerializeField]
		private EventReference _leverSound;

		[Tooltip("One-shot when the reels land on a combination.")]
		[SerializeField]
		private EventReference _winSound;

		[Tooltip("One-shot when the reels land without a combination.")]
		[SerializeField]
		private EventReference _failSound;

		[Tooltip("One-shot when the box pushes an item back out. Broadcast from state authority.")]
		[SerializeField]
		private EventReference _ejectSound;

		[Tooltip("One-shot when the machine breaks. Plays on every peer from IsBroken, not on late join.")]
		[SerializeField]
		private EventReference _destroySound;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		private readonly float[] _spinStartAngles = new float[3];

		private readonly float[] _spinEndAngles = new float[3];

		private readonly SlotSymbol[] _pendingResults = new SlotSymbol[3];

		private readonly Dictionary<NetworkId, ZoneItemCache> _itemsInZone = new Dictionary<NetworkId, ZoneItemCache>();

		private readonly HashSet<NetworkId> _ejectedWhilePresent = new HashSet<NetworkId>();

		private readonly HashSet<NetworkId> _ejectSoundPlayedWhilePresent = new HashSet<NetworkId>();

		private readonly Dictionary<NetworkId, int> _lastJunkEjectTick = new Dictionary<NetworkId, int>();

		private readonly List<NetworkId> _zoneItemIdsScratch = new List<NetworkId>();

		private readonly List<NetworkId> _betCoinIds = new List<NetworkId>();

		private readonly Dictionary<NetworkId, float> _rewardItemIgnoreUntil = new Dictionary<NetworkId, float>();

		private readonly List<int> _grabReleaseIdsScratch = new List<int>();

		private Predicate<IPointGrabable> _forceUngrabMatch;

		private NetworkId _forceUngrabItemId;

		private bool _isSpinning;

		private bool _lastBoxOpen = true;

		private bool _lastSeenBroken;

		private int _lastSeenSpinVersion = -1;

		private Coroutine _spinRoutine;

		private EventInstance _spinSoundInstance;

		private bool _hasSpinSoundInstance;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsSpinReady", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsSpinReady;

		[WeaverGenerated]
		[DefaultForProperty("IsBoxOpen", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsBoxOpen;

		[WeaverGenerated]
		[DefaultForProperty("SpinVersion", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SpinVersion;

		[WeaverGenerated]
		[DefaultForProperty("SpinReel1", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinReel1;

		[WeaverGenerated]
		[DefaultForProperty("SpinReel2", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinReel2;

		[WeaverGenerated]
		[DefaultForProperty("SpinReel3", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinReel3;

		[WeaverGenerated]
		[DefaultForProperty("SpinFullSpins1", 6, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinFullSpins1;

		[WeaverGenerated]
		[DefaultForProperty("SpinFullSpins2", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinFullSpins2;

		[WeaverGenerated]
		[DefaultForProperty("SpinFullSpins3", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _SpinFullSpins3;

		[WeaverGenerated]
		[DefaultForProperty("LosingSpinsStreak", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _LosingSpinsStreak;

		[WeaverGenerated]
		[DefaultForProperty("SpinsUntilGuaranteedWin", 10, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SpinsUntilGuaranteedWin;

		[WeaverGenerated]
		[DefaultForProperty("PaidOutItemCount", 11, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PaidOutItemCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsPayingOut", 12, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsPayingOut;

		[WeaverGenerated]
		[DefaultForProperty("PayoutDeadlineTick", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PayoutDeadlineTick;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BetCoinCount", 14, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BetCoinCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsBroken", 15, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsBroken;

		private int _spinBetMultiplier = 1;

		private IItemSpawnService _itemSpawnService;

		private SlotMachineRewardConfig _slotMachineRewardConfig;

		private LineArmsModel _lineArmsModel;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe NetworkBool IsSpinReady
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsSpinReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsSpinReady. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkBool IsBoxOpen
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsBoxOpen. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsBoxOpen. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int SpinVersion
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinVersion. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinVersion. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe byte SpinReel1
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel1. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[12];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel1. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[12] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe byte SpinReel2
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel2. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[16];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel2. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[16] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		private unsafe byte SpinReel3
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel3. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[20];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinReel3. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[20] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 1)]
		private unsafe byte SpinFullSpins1
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins1. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[24];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins1. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[24] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		private unsafe byte SpinFullSpins2
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins2. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[28];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins2. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[28] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(8, 1)]
		private unsafe byte SpinFullSpins3
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins3. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[32];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinFullSpins3. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[32] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(9, 1)]
		private unsafe int LosingSpinsStreak
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.LosingSpinsStreak. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[9];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.LosingSpinsStreak. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[9] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(10, 1)]
		private unsafe int SpinsUntilGuaranteedWin
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinsUntilGuaranteedWin. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[10];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.SpinsUntilGuaranteedWin. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[10] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(11, 1)]
		private unsafe int PaidOutItemCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.PaidOutItemCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[11];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.PaidOutItemCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[11] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(12, 1)]
		public unsafe NetworkBool IsPayingOut
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsPayingOut. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 12);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsPayingOut. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 12) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 1)]
		private unsafe int PayoutDeadlineTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.PayoutDeadlineTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[13];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.PayoutDeadlineTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[13] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(14, 1)]
		public unsafe int BetCoinCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.BetCoinCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[14];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.BetCoinCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[14] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(15, 1)]
		public unsafe NetworkBool IsBroken
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsBroken. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 15);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlotMachineBehaviour.IsBroken. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 15) = value;
			}
		}

		public bool IsSpinning => _isSpinning;

		public SlotSymbol Reel1Result { get; private set; }

		public SlotSymbol Reel2Result { get; private set; }

		public SlotSymbol Reel3Result { get; private set; }

		public bool IsLastSpinWin
		{
			get
			{
				if (Reel1Result != SlotSymbol.None && Reel1Result == Reel2Result)
				{
					return Reel2Result == Reel3Result;
				}
				return false;
			}
		}

		public int RemainingItemCount => Mathf.Max(0, _slotMachineRewardConfig.MaxTotalItemCount - PaidOutItemCount);

		public event Action<SlotSymbol, SlotSymbol, SlotSymbol> OnSpinCompleted;

		[Inject]
		public void InjectDependencies(IItemSpawnService itemSpawnService, SlotMachineRewardConfig slotMachineRewardConfig, LineArmsModel lineArmsModel, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_itemSpawnService = itemSpawnService;
			_slotMachineRewardConfig = slotMachineRewardConfig;
			_lineArmsModel = lineArmsModel;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				IsBoxOpen = true;
				if (SpinsUntilGuaranteedWin <= 0)
				{
					RollSpinsUntilGuaranteedWin();
				}
			}
			_lastBoxOpen = IsBoxOpen;
			ApplyBoxOpen(IsBoxOpen);
			_lastSeenSpinVersion = SpinVersion;
			SnapReelsToNetworkedResult();
			_lastSeenBroken = IsBroken;
			if ((bool)IsBroken)
			{
				ApplyBroken();
			}
		}

		private void SnapReelsToNetworkedResult()
		{
			if (SpinVersion > 0 && AreReelsValid())
			{
				_pendingResults[0] = (SlotSymbol)SpinReel1;
				_pendingResults[1] = (SlotSymbol)SpinReel2;
				_pendingResults[2] = (SlotSymbol)SpinReel3;
				for (int i = 0; i < 3; i++)
				{
					float symbolLocalEulerX = GetSymbolLocalEulerX(_pendingResults[i]);
					_reelTransforms[i].localRotation = Quaternion.Euler(symbolLocalEulerX, 0f, 0f);
				}
				Reel1Result = _pendingResults[0];
				Reel2Result = _pendingResults[1];
				Reel3Result = _pendingResults[2];
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			StopSpinRoutine();
			StopSpinSound(FMOD.Studio.STOP_MODE.IMMEDIATE);
			_itemsInZone.Clear();
			_ejectedWhilePresent.Clear();
			_ejectSoundPlayedWhilePresent.Clear();
			_lastJunkEjectTick.Clear();
			_rewardItemIgnoreUntil.Clear();
			_isSpinning = false;
			_betCoinIds.Clear();
		}

		public void HandleZoneItemEnter(IItem item, NetworkId itemId)
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority)
			{
				if (_itemsInZone.TryGetValue(itemId, out var value))
				{
					value.ColliderCount++;
					value.Item = item;
					_itemsInZone[itemId] = value;
				}
				else
				{
					_itemsInZone[itemId] = new ZoneItemCache
					{
						Item = item,
						ColliderCount = 1
					};
				}
				if (IsZoneItemValid(item) && !IsIgnoredRewardItem(itemId) && ((bool)IsBroken || item.Type != ItemType.Coin))
				{
					RejectJunkItem(itemId, item);
				}
			}
		}

		public void HandleZoneItemExit(IItem item, NetworkId itemId)
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority && _itemsInZone.TryGetValue(itemId, out var value))
			{
				value.ColliderCount--;
				if (value.ColliderCount <= 0)
				{
					RemoveZoneItem(itemId);
				}
				else
				{
					_itemsInZone[itemId] = value;
				}
			}
		}

		public void RequestSpin()
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority && !_isSpinning && !IsBroken && (bool)IsSpinReady)
			{
				IsSpinReady = false;
				IsBoxOpen = false;
				_spinBetMultiplier = Mathf.Max(1, BetCoinCount);
				ConsumeBetCoins();
				BetCoinCount = 0;
				RollSpinSymbols();
				SpinFullSpins1 = (byte)UnityEngine.Random.Range(_minFullSpins, _maxFullSpins + 1);
				SpinFullSpins2 = (byte)UnityEngine.Random.Range(_minFullSpins, _maxFullSpins + 1);
				SpinFullSpins3 = (byte)UnityEngine.Random.Range(_minFullSpins, _maxFullSpins + 1);
				SpinVersion++;
				_lastSeenSpinVersion = SpinVersion;
				BeginSpinFromNetworkedState();
			}
		}

		private void RollSpinSymbols()
		{
			if (LosingSpinsStreak + 1 >= SpinsUntilGuaranteedWin || UnityEngine.Random.value < _winChance)
			{
				byte b = (SpinReel1 = (byte)UnityEngine.Random.Range(1, 9));
				SpinReel2 = b;
				SpinReel3 = b;
				LosingSpinsStreak = 0;
				RollSpinsUntilGuaranteedWin();
				return;
			}
			SpinReel1 = (byte)UnityEngine.Random.Range(1, 9);
			SpinReel2 = (byte)UnityEngine.Random.Range(1, 9);
			SpinReel3 = (byte)UnityEngine.Random.Range(1, 9);
			if (SpinReel1 == SpinReel2 && SpinReel2 == SpinReel3)
			{
				SpinReel3 = (byte)(SpinReel3 % 8 + 1);
			}
			LosingSpinsStreak++;
		}

		private void RollSpinsUntilGuaranteedWin()
		{
			int num = Mathf.Max(1, _minSpinsUntilGuaranteedWin);
			int num2 = Mathf.Max(num, _maxSpinsUntilGuaranteedWin);
			SpinsUntilGuaranteedWin = UnityEngine.Random.Range(num, num2 + 1);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				ExpirePayoutIfStuck();
				ProcessZone();
			}
		}

		private void ExpirePayoutIfStuck()
		{
			if ((bool)IsPayingOut && !(base.Runner.Tick < PayoutDeadlineTick))
			{
				IsPayingOut = false;
				PayoutDeadlineTick = 0;
			}
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			StopSpinSound(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}

		private void Update()
		{
			if ((bool)base.Object && base.Object.IsValid)
			{
				SyncBrokenFromNetworked();
				SyncBoxAnimatorFromNetworked();
				SyncSpinFromNetworked();
				UpdateSpinSound3D();
			}
		}

		private void ProcessZone()
		{
			if ((bool)IsBroken)
			{
				RejectEverythingInZone();
				return;
			}
			bool flag = !_isSpinning && !IsPayingOut;
			_zoneItemIdsScratch.Clear();
			_zoneItemIdsScratch.AddRange(_itemsInZone.Keys);
			if (flag)
			{
				_betCoinIds.Clear();
			}
			for (int i = 0; i < _zoneItemIdsScratch.Count; i++)
			{
				NetworkId networkId = _zoneItemIdsScratch[i];
				if (!_itemsInZone.TryGetValue(networkId, out var value) || !IsZoneItemValid(value.Item))
				{
					RemoveZoneItem(networkId);
					continue;
				}
				IItem item = value.Item;
				if (IsIgnoredRewardItem(networkId))
				{
					continue;
				}
				if (item.Type == ItemType.Coin)
				{
					if (flag && !IsItemGrabbed(item))
					{
						if (_betCoinIds.Count < _maxBetCoins)
						{
							_betCoinIds.Add(networkId);
						}
						else
						{
							TryEjectItem(networkId, item);
						}
					}
				}
				else
				{
					RejectJunkItem(networkId, item);
				}
			}
			if (flag)
			{
				BetCoinCount = _betCoinIds.Count;
				IsSpinReady = BetCoinCount > 0;
			}
		}

		private void RejectEverythingInZone()
		{
			BetCoinCount = 0;
			IsSpinReady = false;
			_betCoinIds.Clear();
			_zoneItemIdsScratch.Clear();
			_zoneItemIdsScratch.AddRange(_itemsInZone.Keys);
			for (int i = 0; i < _zoneItemIdsScratch.Count; i++)
			{
				NetworkId networkId = _zoneItemIdsScratch[i];
				if (!_itemsInZone.TryGetValue(networkId, out var value) || !IsZoneItemValid(value.Item))
				{
					RemoveZoneItem(networkId);
				}
				else if (!IsIgnoredRewardItem(networkId))
				{
					RejectJunkItem(networkId, value.Item);
				}
			}
		}

		private void RejectJunkItem(NetworkId itemId, IItem item)
		{
			if (!_lastJunkEjectTick.TryGetValue(itemId, out var value) || (int)base.Runner.Tick - value >= 6)
			{
				_lastJunkEjectTick[itemId] = base.Runner.Tick;
				if (IsItemGrabbed(item))
				{
					ForceReleaseItemGrabs(item);
				}
				EjectItem(item);
				TryPlayEjectSound(itemId);
			}
		}

		private void ForceReleaseItemGrabs(IItem item)
		{
			ForceUngrabItemRpc(item.NetworkObject.Id);
			if (!item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return;
			}
			ReleaseExternalHolds(component);
			List<SimplePointGrabable> connectedGrabables = component.ConnectedGrabables;
			for (int i = 0; i < connectedGrabables.Count; i++)
			{
				if (connectedGrabables[i] != null)
				{
					ReleaseExternalHolds(connectedGrabables[i]);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1529468489u)]
		private void ForceUngrabItemRpc([RpcPayload(4)] NetworkId itemNetworkId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1529468489u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SlotMachineModule.Scripts.SlotMachineBehaviour::ForceUngrabItemRpc(Fusion.NetworkId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(itemNetworkId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(base.Runner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer == null)
			{
				return;
			}
			_forceUngrabItemId = itemNetworkId;
			if (_forceUngrabMatch == null)
			{
				_forceUngrabMatch = MatchesForceUngrabItem;
			}
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null))
				{
					value.UnJoinAllMatching(_forceUngrabMatch);
				}
			}
		}

		private bool MatchesForceUngrabItem(IPointGrabable grabable)
		{
			if (grabable != null && grabable.NetworkObject != null && grabable.NetworkObject.IsValid)
			{
				return grabable.NetworkObject.Id == _forceUngrabItemId;
			}
			return false;
		}

		private void ReleaseExternalHolds(SimplePointGrabable grabable)
		{
			_grabReleaseIdsScratch.Clear();
			_grabReleaseIdsScratch.AddRange(grabable.GrabbedByExternals);
			for (int i = 0; i < _grabReleaseIdsScratch.Count; i++)
			{
				grabable.UnGrabbedByExternal(_grabReleaseIdsScratch[i]);
			}
		}

		private void ConsumeBetCoins()
		{
			for (int i = 0; i < _betCoinIds.Count; i++)
			{
				NetworkId networkId = _betCoinIds[i];
				if (_itemsInZone.TryGetValue(networkId, out var value) && IsZoneItemValid(value.Item))
				{
					if (value.Item is MonoItem monoItem && value.Item.NetworkObject.HasStateAuthority)
					{
						monoItem.SetDespawnEffectsSuppressed(suppressed: true);
					}
					value.Item.Consume();
					RemoveZoneItem(networkId);
				}
			}
			_betCoinIds.Clear();
		}

		private void RegisterRewardItem(NetworkObject rewardItem)
		{
			if (!(rewardItem == null) && rewardItem.IsValid)
			{
				_rewardItemIgnoreUntil[rewardItem.Id] = Time.time + _rewardItemIgnoreDuration;
				PlayPayoutParticleRPC();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2835890747u)]
		private void PlayPayoutParticleRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2835890747u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SlotMachineModule.Scripts.SlotMachineBehaviour::PlayPayoutParticleRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlayParticle(_payoutParticlePrefab, GetPayoutOrigin());
		}

		private void PlayParticle(ParticleSystem particlePrefab, Transform origin)
		{
			PlayParticleAt(particlePrefab, origin.position, origin.rotation);
		}

		private void PlayParticleAt(ParticleSystem particlePrefab, Vector3 position, Quaternion rotation)
		{
			if (!(particlePrefab == null))
			{
				UnityEngine.Object.Instantiate(particlePrefab.gameObject, position, rotation);
			}
		}

		private Transform GetResultParticleOrigin()
		{
			if (!(_particleOrigin != null))
			{
				return GetPayoutOrigin();
			}
			return _particleOrigin;
		}

		private void PruneExpiredRewardItems()
		{
			_zoneItemIdsScratch.Clear();
			foreach (KeyValuePair<NetworkId, float> item in _rewardItemIgnoreUntil)
			{
				if (Time.time >= item.Value)
				{
					_zoneItemIdsScratch.Add(item.Key);
				}
			}
			for (int i = 0; i < _zoneItemIdsScratch.Count; i++)
			{
				_rewardItemIgnoreUntil.Remove(_zoneItemIdsScratch[i]);
			}
			_zoneItemIdsScratch.Clear();
		}

		private bool IsIgnoredRewardItem(NetworkId itemId)
		{
			if (!_rewardItemIgnoreUntil.TryGetValue(itemId, out var value))
			{
				return false;
			}
			if (Time.time < value)
			{
				return true;
			}
			_rewardItemIgnoreUntil.Remove(itemId);
			return false;
		}

		private static bool IsZoneItemValid(IItem item)
		{
			if (item != null && item.IsSpawned && !item.IsConsumed && item.NetworkObject != null)
			{
				return item.NetworkObject.IsValid;
			}
			return false;
		}

		private void RemoveZoneItem(NetworkId itemId)
		{
			_itemsInZone.Remove(itemId);
			_ejectedWhilePresent.Remove(itemId);
			_ejectSoundPlayedWhilePresent.Remove(itemId);
			_lastJunkEjectTick.Remove(itemId);
		}

		private void SyncBoxAnimatorFromNetworked()
		{
			bool flag = IsBoxOpen;
			if (flag != _lastBoxOpen)
			{
				_lastBoxOpen = flag;
				ApplyBoxOpen(flag);
			}
		}

		private void SyncSpinFromNetworked()
		{
			if (SpinVersion != _lastSeenSpinVersion)
			{
				_lastSeenSpinVersion = SpinVersion;
				if (SpinVersion > 0)
				{
					BeginSpinFromNetworkedState();
				}
			}
		}

		private void BeginSpinFromNetworkedState()
		{
			if (AreReelsValid())
			{
				StopSpinRoutine();
				StopSpinSound(FMOD.Studio.STOP_MODE.IMMEDIATE);
				PlayParticle(_payoutParticlePrefab, GetPayoutOrigin());
				_pendingResults[0] = (SlotSymbol)SpinReel1;
				_pendingResults[1] = (SlotSymbol)SpinReel2;
				_pendingResults[2] = (SlotSymbol)SpinReel3;
				byte[] array = new byte[3] { SpinFullSpins1, SpinFullSpins2, SpinFullSpins3 };
				for (int i = 0; i < 3; i++)
				{
					float num = NormalizeDegrees(_reelTransforms[i].localEulerAngles.x);
					float symbolLocalEulerX = GetSymbolLocalEulerX(_pendingResults[i]);
					float num2 = (float)(int)array[i] * 360f + ForwardDeltaDegrees(num, symbolLocalEulerX);
					_spinStartAngles[i] = num;
					_spinEndAngles[i] = num + num2;
				}
				_isSpinning = true;
				PlayOneShotSound(_leverSound);
				StartSpinSound();
				_spinRoutine = StartCoroutine(SpinRoutine());
			}
		}

		private IEnumerator SpinRoutine()
		{
			float longestDuration = _baseSpinDuration + _reelStopStagger * 2f;
			float elapsed = 0f;
			bool[] reelFinished = new bool[3];
			while (elapsed < longestDuration)
			{
				elapsed += Time.deltaTime;
				for (int i = 0; i < 3; i++)
				{
					if (!reelFinished[i])
					{
						float num = _baseSpinDuration + _reelStopStagger * (float)i;
						float num2 = Mathf.Clamp01(elapsed / num);
						float t = _spinEase.Evaluate(num2);
						float x = Mathf.Lerp(_spinStartAngles[i], _spinEndAngles[i], t);
						_reelTransforms[i].localRotation = Quaternion.Euler(x, 0f, 0f);
						if (num2 >= 1f)
						{
							float symbolLocalEulerX = GetSymbolLocalEulerX(_pendingResults[i]);
							_reelTransforms[i].localRotation = Quaternion.Euler(symbolLocalEulerX, 0f, 0f);
							reelFinished[i] = true;
						}
					}
				}
				yield return null;
			}
			for (int j = 0; j < 3; j++)
			{
				float symbolLocalEulerX2 = GetSymbolLocalEulerX(_pendingResults[j]);
				_reelTransforms[j].localRotation = Quaternion.Euler(symbolLocalEulerX2, 0f, 0f);
			}
			Reel1Result = _pendingResults[0];
			Reel2Result = _pendingResults[1];
			Reel3Result = _pendingResults[2];
			_isSpinning = false;
			_spinRoutine = null;
			StopSpinSound(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			PlayParticle(IsLastSpinWin ? _winParticlePrefab : _loseParticlePrefab, GetResultParticleOrigin());
			PlayOneShotSound(IsLastSpinWin ? _winSound : _failSound);
			if (base.HasStateAuthority)
			{
				IsBoxOpen = true;
				if (IsLastSpinWin)
				{
					GrantReward(Reel1Result).Forget();
				}
			}
			this.OnSpinCompleted?.Invoke(Reel1Result, Reel2Result, Reel3Result);
		}

		public int ConsumeItemCount(int requestedCount)
		{
			if (!base.HasStateAuthority || requestedCount <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(requestedCount, RemainingItemCount);
			if (num <= 0)
			{
				return 0;
			}
			PaidOutItemCount += num;
			return num;
		}

		private async UniTaskVoid GrantReward(SlotSymbol symbol)
		{
			SlotMachineRewardBase reward = _slotMachineRewardConfig.GetReward(symbol);
			if (!(reward == null))
			{
				SlotMachineRewardContext context = new SlotMachineRewardContext(symbol, GetPayoutOrigin().position, GetRewardEjectDirection, _ejectForce * _rewardEjectForceMultiplier, _itemSpawnService, this, RegisterRewardItem, _spinBetMultiplier);
				BeginPayout();
				if (_payoutStartDelay > 0f)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(_payoutStartDelay));
				}
				await reward.Grant(context);
				if (_payoutHoldDelay > 0f)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(_payoutHoldDelay));
				}
				EndPayout();
			}
		}

		private void BeginPayout()
		{
			PruneExpiredRewardItems();
			IsPayingOut = true;
			PayoutDeadlineTick = (int)base.Runner.Tick + Mathf.CeilToInt(_payoutTimeout * (float)base.Runner.TickRate);
		}

		private void EndPayout()
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority)
			{
				IsPayingOut = false;
				PayoutDeadlineTick = 0;
				if (RemainingItemCount <= 0)
				{
					IsBroken = true;
				}
			}
		}

		private void SyncBrokenFromNetworked()
		{
			if (!(IsBroken == _lastSeenBroken))
			{
				_lastSeenBroken = IsBroken;
				if ((bool)IsBroken)
				{
					ApplyBroken();
					PlayOneShotSound(_destroySound);
				}
			}
		}

		private void ApplyBroken()
		{
			if (_boxAnimator != null)
			{
				_boxAnimator.ResetTrigger(OpenHash);
				_boxAnimator.ResetTrigger(CloseHash);
				_boxAnimator.SetTrigger(DestroyHash);
			}
			RemoveLever();
		}

		private void RemoveLever()
		{
			Transform transform = ((_leverNetworkObject != null && _leverNetworkObject.IsValid) ? _leverNetworkObject.transform : GetResultParticleOrigin());
			PlayParticleAt(_destroyParticlePrefab, transform.position, transform.rotation);
			if (base.HasStateAuthority && !(_leverNetworkObject == null) && _leverNetworkObject.IsValid)
			{
				base.Runner.Despawn(_leverNetworkObject);
			}
		}

		private void TryEjectItem(NetworkId itemId, IItem item)
		{
			if (_ejectedWhilePresent.Add(itemId))
			{
				EjectItem(item);
				TryPlayEjectSound(itemId);
			}
		}

		private void EjectItem(IItem item)
		{
			if (item is MonoItem monoItem)
			{
				monoItem.AddForce(_ejectForce, GetEjectDirection(), ForceMode.Impulse);
			}
		}

		private Vector3 GetRewardEjectDirection()
		{
			Transform payoutOrigin = GetPayoutOrigin();
			float num = _rewardEjectAngleDegrees + UnityEngine.Random.Range(0f - _rewardEjectAngleSpreadDegrees, _rewardEjectAngleSpreadDegrees);
			float angle = UnityEngine.Random.Range(0f - _rewardEjectSpreadDegrees, _rewardEjectSpreadDegrees);
			Quaternion quaternion = Quaternion.AngleAxis(0f - num, payoutOrigin.right);
			return (Quaternion.AngleAxis(angle, payoutOrigin.up) * quaternion * payoutOrigin.forward).normalized;
		}

		private Transform GetPayoutOrigin()
		{
			if (_payoutOrigin != null)
			{
				return _payoutOrigin;
			}
			if (!(_ejectOrigin != null))
			{
				return base.transform;
			}
			return _ejectOrigin;
		}

		private Vector3 GetEjectDirection()
		{
			Transform transform = ((_ejectOrigin != null) ? _ejectOrigin : base.transform);
			Vector3 normalized = (Vector3.up * _ejectUpwardBias + transform.forward).normalized;
			if (normalized.sqrMagnitude < 0.001f)
			{
				normalized = (Vector3.up * _ejectUpwardBias + base.transform.forward).normalized;
			}
			return normalized;
		}

		private static bool IsItemGrabbed(IItem item)
		{
			if (item?.NetworkObject == null)
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<SimplePointGrabable>(out var component))
			{
				return false;
			}
			if (component.GrabbedByPlayersCount <= 0)
			{
				return component.GrabbedBySomethingCount > 0;
			}
			return true;
		}

		private void ApplyBoxOpen(bool isOpen)
		{
			if (!(_boxAnimator == null))
			{
				if (isOpen)
				{
					_boxAnimator.ResetTrigger(CloseHash);
					_boxAnimator.SetTrigger(OpenHash);
				}
				else
				{
					_boxAnimator.ResetTrigger(OpenHash);
					_boxAnimator.SetTrigger(CloseHash);
				}
			}
		}

		private void StopSpinRoutine()
		{
			if (_spinRoutine != null)
			{
				StopCoroutine(_spinRoutine);
				_spinRoutine = null;
			}
		}

		private void StartSpinSound()
		{
			StopSpinSound(FMOD.Studio.STOP_MODE.IMMEDIATE);
			if (_audioService != null && !(_soundSource == null) && !_spinSound.IsNull)
			{
				_spinSoundInstance = _audioService.CreateInstance(_spinSound);
				_hasSpinSoundInstance = true;
				SetSoundImmediately(_spinSoundInstance);
				_audioService.StartInstanceWith3DAttributes(_spinSoundInstance, _soundSource);
			}
		}

		private void UpdateSpinSound3D()
		{
			if (_hasSpinSoundInstance && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				_spinSoundInstance.set3DAttributes(_soundSource.SoundSourceTransform.To3DAttributes());
				ProcessSoundOcclusion(_spinSoundInstance);
			}
		}

		private void StopSpinSound(FMOD.Studio.STOP_MODE stopMode)
		{
			if (_hasSpinSoundInstance)
			{
				if (_audioService != null)
				{
					_audioService.StopInstance(_spinSoundInstance, stopMode);
					_audioService.ReleaseInstance(_spinSoundInstance);
				}
				_spinSoundInstance = default(EventInstance);
				_hasSpinSoundInstance = false;
			}
		}

		private void PlayOneShotSound(EventReference eventReference)
		{
			if (_audioService != null && !(_soundSource == null) && !eventReference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(eventReference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void TryPlayEjectSound(NetworkId itemId)
		{
			if (_ejectSoundPlayedWhilePresent.Add(itemId) && !_ejectSound.IsNull)
			{
				PlayEjectSoundRpc();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1330724775u)]
		private void PlayEjectSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1330724775u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.SlotMachineModule.Scripts.SlotMachineBehaviour::PlayEjectSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlayOneShotSound(_ejectSound);
		}

		private float GetSymbolLocalEulerX(SlotSymbol symbol)
		{
			return _symbol1LocalEulerX + (float)(symbol - 1) * _symbolStepDegrees;
		}

		private static float ForwardDeltaDegrees(float fromDegrees, float toDegrees)
		{
			float num = Mathf.Repeat(toDegrees - fromDegrees, 360f);
			if (num <= 0.001f)
			{
				num = 360f;
			}
			return num;
		}

		private static float NormalizeDegrees(float degrees)
		{
			return Mathf.Repeat(degrees, 360f);
		}

		private bool AreReelsValid()
		{
			if (_reelTransforms == null || _reelTransforms.Length != 3)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				if (_reelTransforms[i] == null)
				{
					return false;
				}
			}
			return true;
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, out var value);
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float b = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value3);
				}
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsSpinReady = _IsSpinReady;
			IsBoxOpen = _IsBoxOpen;
			SpinVersion = _SpinVersion;
			SpinReel1 = _SpinReel1;
			SpinReel2 = _SpinReel2;
			SpinReel3 = _SpinReel3;
			SpinFullSpins1 = _SpinFullSpins1;
			SpinFullSpins2 = _SpinFullSpins2;
			SpinFullSpins3 = _SpinFullSpins3;
			LosingSpinsStreak = _LosingSpinsStreak;
			SpinsUntilGuaranteedWin = _SpinsUntilGuaranteedWin;
			PaidOutItemCount = _PaidOutItemCount;
			IsPayingOut = _IsPayingOut;
			PayoutDeadlineTick = _PayoutDeadlineTick;
			BetCoinCount = _BetCoinCount;
			IsBroken = _IsBroken;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsSpinReady = IsSpinReady;
			_IsBoxOpen = IsBoxOpen;
			_SpinVersion = SpinVersion;
			_SpinReel1 = SpinReel1;
			_SpinReel2 = SpinReel2;
			_SpinReel3 = SpinReel3;
			_SpinFullSpins1 = SpinFullSpins1;
			_SpinFullSpins2 = SpinFullSpins2;
			_SpinFullSpins3 = SpinFullSpins3;
			_LosingSpinsStreak = LosingSpinsStreak;
			_SpinsUntilGuaranteedWin = SpinsUntilGuaranteedWin;
			_PaidOutItemCount = PaidOutItemCount;
			_IsPayingOut = IsPayingOut;
			_PayoutDeadlineTick = PayoutDeadlineTick;
			_BetCoinCount = BetCoinCount;
			_IsBroken = IsBroken;
		}

		[NetworkRpcWeavedInvoker(1529468489u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForceUngrabItemRpc_0040Invoker1529468489([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out NetworkId value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SlotMachineBehaviour)context.TargetBehaviour).ForceUngrabItemRpc(value);
		}

		[NetworkRpcWeavedInvoker(2835890747u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayPayoutParticleRPC_0040Invoker2835890747([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SlotMachineBehaviour)context.TargetBehaviour).PlayPayoutParticleRPC();
		}

		[NetworkRpcWeavedInvoker(1330724775u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayEjectSoundRpc_0040Invoker1330724775([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SlotMachineBehaviour)context.TargetBehaviour).PlayEjectSoundRpc();
		}
	}
}
