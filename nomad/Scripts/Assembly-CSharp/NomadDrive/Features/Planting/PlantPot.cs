using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using EvilCore.Localization;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Planting.UI;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Planting
{
	public class PlantPot : HeldItem, INetworkSaveable
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSpawnHarvestItemsAsync_003Ed__103 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

			public PlantPot _003C_003E4__this;

			public int maxCount;

			private PlantConfig _003Cconfig_003E5__2;

			private NetworkedTransform _003CpotNetTransform_003E5__3;

			private int _003CspawnedSoFar_003E5__4;

			private List<Transform>.Enumerator _003C_003E7__wrap4;

			private Vector3 _003ClocalPosition_003E5__6;

			private Vector3 _003ClocalRotation_003E5__7;

			private GameObject _003CharvestItem_003E5__8;

			private UniTask.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				PlantPot plantPot = _003C_003E4__this;
				try
				{
					if (num == 0)
					{
						goto IL_00c4;
					}
					_003Cconfig_003E5__2 = plantPot.ActivePlantConfig;
					if (!(_003Cconfig_003E5__2 == null) && !(_003Cconfig_003E5__2.HarvestPrefab == null))
					{
						PlantVisualContainer plantVisualContainer = plantPot.FindPlantVisualContainer(plantPot.PlantType);
						if (plantVisualContainer != null)
						{
							List<Transform> harvestSpawnPoints = plantVisualContainer.HarvestSpawnPoints;
							if (harvestSpawnPoints != null && harvestSpawnPoints.Count != 0)
							{
								plantPot.ClearSpawnedHarvestItems();
								_003CpotNetTransform_003E5__3 = plantPot.GetComponent<NetworkedTransform>();
								if (!(_003CpotNetTransform_003E5__3 == null))
								{
									_003CspawnedSoFar_003E5__4 = 0;
									_003C_003E7__wrap4 = harvestSpawnPoints.GetEnumerator();
									goto IL_00c4;
								}
								EvilLogger.LogError("[PlantPot] PlantPot does not have NetworkedTransform component", "SpawnHarvestItemsAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Planting\\Scripts\\PlantPot.cs", 735);
							}
						}
					}
					goto end_IL_000e;
					IL_00c4:
					try
					{
						if (num != 0)
						{
							goto IL_02f3;
						}
						UniTask.Awaiter awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(UniTask.Awaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0278;
						IL_0278:
						awaiter.GetResult();
						if (_003CharvestItem_003E5__8.TryGetComponent<NetworkedTransform>(out var component))
						{
							NetworkedTransformParentingConfig parentingConfig = new NetworkedTransformParentingConfig
							{
								PositionOffset = _003ClocalPosition_003E5__6,
								RotationOffset = _003ClocalRotation_003E5__7,
								KeepPositionAxes = false,
								KeepRotationAxes = false
							};
							component.SetParent(_003CpotNetTransform_003E5__3, parentingConfig, 0);
						}
						_003ClocalPosition_003E5__6 = default(Vector3);
						_003ClocalRotation_003E5__7 = default(Vector3);
						_003CharvestItem_003E5__8 = null;
						goto IL_02f3;
						IL_02f3:
						while (_003C_003E7__wrap4.MoveNext())
						{
							Transform current = _003C_003E7__wrap4.Current;
							if (maxCount < 0 || _003CspawnedSoFar_003E5__4 < maxCount)
							{
								if (current == null)
								{
									continue;
								}
								_003CspawnedSoFar_003E5__4++;
								_003ClocalPosition_003E5__6 = plantPot.transform.InverseTransformPoint(current.position);
								_003ClocalRotation_003E5__7 = (Quaternion.Inverse(plantPot.transform.rotation) * current.rotation).eulerAngles;
								Vector3 position = current.position;
								Quaternion rotation = current.rotation;
								_003CharvestItem_003E5__8 = UnityEngine.Object.Instantiate(_003Cconfig_003E5__2.HarvestPrefab, position, rotation);
								NetworkServer.Spawn(_003CharvestItem_003E5__8);
								PersistentObject.ServerEnsure(_003CharvestItem_003E5__8, _003Cconfig_003E5__2.HarvestPrefabAddressableGuid);
								if (_003CharvestItem_003E5__8.TryGetComponent<Food>(out var component2))
								{
									component2.ServerSetFreshMax();
								}
								if (_003CharvestItem_003E5__8.TryGetComponent<Interactable>(out var component3))
								{
									component3.SetRigidCollidersTriggered(newValue: true);
									component3.SetInteractionAvailability(newValue: true);
								}
								NetworkIdentity component4 = _003CharvestItem_003E5__8.GetComponent<NetworkIdentity>();
								if (component4 != null)
								{
									plantPot._spawnedHarvestItems.Add(component4);
									plantPot._harvestItemNetIds.Add(component4.netId);
								}
								awaiter = UniTask.Delay(50, ignoreTimeScale: false, PlayerLoopTiming.Update, plantPot.GetCancellationTokenOnDestroy()).GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									num = (_003C_003E1__state = 0);
									_003C_003Eu__1 = awaiter;
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
									return;
								}
								goto IL_0278;
							}
							break;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
						}
					}
					_003C_003E7__wrap4 = default(List<Transform>.Enumerator);
					end_IL_000e:;
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003Cconfig_003E5__2 = null;
					_003CpotNetTransform_003E5__3 = null;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003Cconfig_003E5__2 = null;
				_003CpotNetTransform_003E5__3 = null;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CStartGrowthTimerAsync_003Ed__101 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

			public PlantPot _003C_003E4__this;

			private double _003CtargetTime_003E5__2;

			private UniTask.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				PlantPot plantPot = _003C_003E4__this;
				try
				{
					UniTask.Awaiter awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(UniTask.Awaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00eb;
					}
					if (!plantPot._isGrowthTimerActive)
					{
						plantPot._isGrowthTimerActive = true;
						PlantConfig activePlantConfig = plantPot.ActivePlantConfig;
						if (!(activePlantConfig == null))
						{
							float growthTimeForPhase = activePlantConfig.GetGrowthTimeForPhase(plantPot._currentPhase);
							_003CtargetTime_003E5__2 = plantPot._phaseStartTime + (double)growthTimeForPhase;
							goto IL_00f2;
						}
						plantPot._isGrowthTimerActive = false;
					}
					goto end_IL_000e;
					IL_00eb:
					awaiter.GetResult();
					goto IL_00f2;
					IL_00f2:
					if (plantPot._isGrowthTimerActive && NetworkTime.time < _003CtargetTime_003E5__2)
					{
						if (plantPot._plantStateByte == 2)
						{
							awaiter = UniTask.Delay(TimeSpan.FromSeconds(1.0), ignoreTimeScale: false, PlayerLoopTiming.Update, plantPot.GetCancellationTokenOnDestroy()).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_00eb;
						}
						plantPot._isGrowthTimerActive = false;
					}
					else if (plantPot._isGrowthTimerActive)
					{
						plantPot._isGrowthTimerActive = false;
						plantPot.AdvanceToNextPhase();
					}
					end_IL_000e:;
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[Tooltip("Plant configuration database reference")]
		[SerializeField]
		private PlantConfigDatabase plantConfigDatabase;

		[Tooltip("Container GameObjects for each plant type (configured in prefab)")]
		[SerializeField]
		private List<PlantVisualContainer> plantVisualContainers = new List<PlantVisualContainer>();

		[Tooltip("MeshRenderer for soil visual (uses MaterialPropertyBlock for per-instance changes)")]
		[SerializeField]
		private MeshRenderer soilRenderer;

		[Tooltip("Smoothness remap range when soil is wet (min-max)")]
		[SerializeField]
		private Vector2 wetSoilSmoothness = new Vector2(0.9f, 1f);

		[Tooltip("Smoothness remap range when soil is dry (min-max)")]
		[SerializeField]
		private Vector2 drySoilSmoothness = new Vector2(0f, 0.2f);

		[SerializeField]
		private SoundID waterPlantSound;

		[SerializeField]
		private SoundID plantSeedSound;

		[SerializeField]
		private SoundID plantGrowthSound;

		[SyncVar(hook = "OnPlantTypeChanged")]
		private byte _plantTypeByte;

		[SyncVar(hook = "OnPlantStateChanged")]
		private byte _plantStateByte;

		[SyncVar(hook = "OnCurrentPhaseChanged")]
		private byte _currentPhase;

		[SyncVar]
		private double _lastWateredTime;

		[SyncVar]
		private double _phaseStartTime;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private PlantPotInfoPanel _plantPotInfoPanel;

		private InteractionStateMachine<PlantPotInteractionState> _plantPotStateMachine;

		private PlantConfig _cachedConfig;

		private bool _isGrowthTimerActive;

		private PlantSeed _equippedSeed;

		private ILiquidContainer _equippedWaterContainer;

		private LiquidContainerComponent _equippedWaterContainerComponent;

		private bool _hasWaterContainerButInsufficient;

		private MaterialPropertyBlock _soilPropertyBlock;

		private readonly List<NetworkIdentity> _spawnedHarvestItems = new List<NetworkIdentity>();

		private string _defaultPotName;

		private readonly SyncList<uint> _harvestItemNetIds = new SyncList<uint>();

		private static readonly int SmoothnessRemapMinProperty;

		private static readonly int SmoothnessRemapMaxProperty;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__plantTypeByte;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__plantStateByte;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__currentPhase;

		private string DebugPlantType
		{
			get
			{
				if (_plantTypeByte != 0)
				{
					PlantType plantTypeByte = (PlantType)_plantTypeByte;
					return plantTypeByte.ToString();
				}
				return "None";
			}
		}

		private string DebugPlantState
		{
			get
			{
				PlantState plantStateByte = (PlantState)_plantStateByte;
				return plantStateByte.ToString();
			}
		}

		private string DebugInteractionState => _plantPotStateMachine?.CurrentState.ToString() ?? "N/A";

		private string DebugCurrentPhase => $"Phase {_currentPhase}";

		private string DebugTimeToNextPhase => GetTimeToNextPhaseDisplay();

		public PlantPotInteractionState CurrentInteractionState => _plantPotStateMachine?.CurrentState ?? PlantPotInteractionState.EmptyNoItem;

		protected override bool UseDefaultStateMachine => false;

		protected override bool UseStateMachine => true;

		public PlantType PlantType
		{
			get
			{
				return (PlantType)_plantTypeByte;
			}
			private set
			{
				Network_plantTypeByte = (byte)value;
			}
		}

		public PlantState PlantState
		{
			get
			{
				return (PlantState)_plantStateByte;
			}
			private set
			{
				Network_plantStateByte = (byte)value;
			}
		}

		public int CurrentPhase => _currentPhase;

		public PlantConfig ActivePlantConfig => plantConfigDatabase?.GetConfig(PlantType);

		public bool IsWatered => PlantState == PlantState.Growing;

		public bool CanHarvest => PlantState == PlantState.FullyGrown;

		public bool HasPlant
		{
			get
			{
				if (PlantType != PlantType.None)
				{
					return PlantState != PlantState.Empty;
				}
				return false;
			}
		}

		public string PlantDisplayName
		{
			get
			{
				PlantConfig activePlantConfig = ActivePlantConfig;
				if (activePlantConfig != null && !string.IsNullOrEmpty(activePlantConfig.DisplayName))
				{
					return activePlantConfig.DisplayName;
				}
				if (PlantType != PlantType.None)
				{
					return PlantType.ToString();
				}
				return string.Empty;
			}
		}

		public int TotalPhases => ActivePlantConfig?.TotalPhases ?? 0;

		public float CurrentPhaseProgress01
		{
			get
			{
				if (PlantState != PlantState.Growing)
				{
					return 0f;
				}
				PlantConfig activePlantConfig = ActivePlantConfig;
				if (activePlantConfig == null)
				{
					return 0f;
				}
				float growthTimeForPhase = activePlantConfig.GetGrowthTimeForPhase(_currentPhase);
				if (growthTimeForPhase <= 0f)
				{
					return 1f;
				}
				return Mathf.Clamp01((float)((NetworkTime.time - _phaseStartTime) / (double)growthTimeForPhase));
			}
		}

		public float SecondsToNextPhase
		{
			get
			{
				if (PlantState != PlantState.Growing)
				{
					return 0f;
				}
				PlantConfig activePlantConfig = ActivePlantConfig;
				if (activePlantConfig == null)
				{
					return 0f;
				}
				double num = (double)activePlantConfig.GetGrowthTimeForPhase(_currentPhase) - (NetworkTime.time - _phaseStartTime);
				if (!(num > 0.0))
				{
					return 0f;
				}
				return (float)num;
			}
		}

		public string ContributorKey => "plant";

		public byte Network_plantTypeByte
		{
			get
			{
				return _plantTypeByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _plantTypeByte, 512uL, _Mirror_SyncVarHookDelegate__plantTypeByte);
			}
		}

		public byte Network_plantStateByte
		{
			get
			{
				return _plantStateByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _plantStateByte, 1024uL, _Mirror_SyncVarHookDelegate__plantStateByte);
			}
		}

		public byte Network_currentPhase
		{
			get
			{
				return _currentPhase;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _currentPhase, 2048uL, _Mirror_SyncVarHookDelegate__currentPhase);
			}
		}

		public double Network_lastWateredTime
		{
			get
			{
				return _lastWateredTime;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _lastWateredTime, 4096uL, null);
			}
		}

		public double Network_phaseStartTime
		{
			get
			{
				return _phaseStartTime;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _phaseStartTime, 8192uL, null);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_defaultPotName = interactableName;
			_soilPropertyBlock = new MaterialPropertyBlock();
			DeactivateAllVisuals();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_isGrowthTimerActive = false;
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			if (PlantState == PlantState.FullyGrown)
			{
				CmdCheckAndResetIfAllItemsEquipped();
			}
			_plantPotInfoPanel?.SetPlantPot(this);
			UpdateState();
		}

		[Command(requiresAuthority = false)]
		private void CmdCheckAndResetIfAllItemsEquipped()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Planting.PlantPot::CmdCheckAndResetIfAllItemsEquipped()", 24198251, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			_plantPotInfoPanel?.ClearPlantPot();
		}

		protected override void InitializeStateMachine()
		{
			_plantPotStateMachine = new InteractionStateMachine<PlantPotInteractionState>(this);
			base.BaseStateMachine = _plantPotStateMachine;
			ConfigureStates();
			_plantPotStateMachine.Initialize(DeterminePlantPotInteractionState());
		}

		protected override void ConfigureStates()
		{
			_plantPotStateMachine.RegisterState(PlantPotInteractionState.EmptyNoItem, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(PlantPotInteractionState.EmptyWithSeed, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.plant_seeds", HandlePlantSeed).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(PlantPotInteractionState.PlantedNoWater, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false))
				.RegisterState(PlantPotInteractionState.PlantedWithWater, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.water", HandleWaterPlant).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: true))
				.RegisterState(PlantPotInteractionState.PlantedWithInsufficientWater, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.not_enough_water", HandleInsufficientWaterFeedback).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: true))
				.RegisterState(PlantPotInteractionState.NeedsWaterNoItem, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
					.WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: false))
				.RegisterState(PlantPotInteractionState.NeedsWaterWithWater, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.water", HandleWaterPlant).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: true))
				.RegisterState(PlantPotInteractionState.NeedsWaterWithInsufficientWater, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.not_enough_water", HandleInsufficientWaterFeedback).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: true))
				.RegisterState(PlantPotInteractionState.Growing, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
					.WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: false))
				.RegisterState(PlantPotInteractionState.FullyGrown, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
					.WithNameLabelVisibility(visible: true)
					.WithInteractionLabelVisibility(visible: false));
		}

		private PlantPotInteractionState DeterminePlantPotInteractionState()
		{
			bool flag = HasEquippedSeed();
			bool flag2 = HasEquippedWaterContainer();
			return PlantState switch
			{
				PlantState.Empty => flag ? PlantPotInteractionState.EmptyWithSeed : PlantPotInteractionState.EmptyNoItem, 
				PlantState.Planted => flag2 ? PlantPotInteractionState.PlantedWithWater : (_hasWaterContainerButInsufficient ? PlantPotInteractionState.PlantedWithInsufficientWater : PlantPotInteractionState.PlantedNoWater), 
				PlantState.NeedsWater => flag2 ? PlantPotInteractionState.NeedsWaterWithWater : (_hasWaterContainerButInsufficient ? PlantPotInteractionState.NeedsWaterWithInsufficientWater : PlantPotInteractionState.NeedsWaterNoItem), 
				PlantState.Growing => PlantPotInteractionState.Growing, 
				PlantState.FullyGrown => PlantPotInteractionState.FullyGrown, 
				_ => PlantPotInteractionState.EmptyNoItem, 
			};
		}

		public override void UpdateState()
		{
			_plantPotStateMachine?.TransitionTo(DeterminePlantPotInteractionState());
		}

		private bool HasEquippedSeed()
		{
			HeldItem heldItem = playerService?.EquipmentManager?.EquippedEntity;
			if (heldItem == null)
			{
				return false;
			}
			_equippedSeed = heldItem.GetComponent<PlantSeed>();
			return _equippedSeed != null;
		}

		private bool HasEquippedWaterContainer()
		{
			_equippedWaterContainer = null;
			_equippedWaterContainerComponent = null;
			_hasWaterContainerButInsufficient = false;
			HeldItem heldItem = playerService?.EquipmentManager?.EquippedEntity;
			if (heldItem == null)
			{
				return false;
			}
			if (!heldItem.TryGetComponent<ILiquidContainer>(out var component))
			{
				return false;
			}
			if (component.CurrentLiquidType != LiquidType.Water)
			{
				return false;
			}
			_equippedWaterContainer = component;
			heldItem.TryGetComponent<LiquidContainerComponent>(out _equippedWaterContainerComponent);
			float waterRequiredForCurrentPhase = GetWaterRequiredForCurrentPhase();
			if (component.CurrentAmount < waterRequiredForCurrentPhase)
			{
				_hasWaterContainerButInsufficient = true;
				return false;
			}
			return true;
		}

		private float GetWaterRequiredForCurrentPhase()
		{
			return ActivePlantConfig?.GetWaterRequiredForPhase(_currentPhase) ?? 0f;
		}

		private PlantType GetEquippedSeedType()
		{
			if (_equippedSeed == null)
			{
				HasEquippedSeed();
			}
			return _equippedSeed?.PlantType ?? PlantType.None;
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (_plantTypeByte == 0)
			{
				PlantState = PlantState.Empty;
			}
			StartServerTimers();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isServer)
			{
				SetForLateJoiner();
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			UpdatePotName(PlantType);
			UpdateVisualsForCurrentState();
			UpdateState();
		}

		private void OnPlantTypeChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				_cachedConfig = null;
				UpdatePotName((PlantType)newValue);
				UpdateVisualsForCurrentState();
				UpdateState();
			}
		}

		private void UpdatePotName(PlantType plantType)
		{
			interactableName = ((plantType == PlantType.None) ? _defaultPotName : $"{plantType} Pot");
		}

		private void OnPlantStateChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue == 4)
				{
					ObjectivesEventBus.Raise(ObjectiveSignal.PlantHarvested, this);
				}
				UpdateVisualsForCurrentState();
				UpdateState();
			}
		}

		private void OnCurrentPhaseChanged(byte oldValue, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (newValue > oldValue && plantGrowthSound.IsValid())
				{
					AudioManager?.PlayOneShot(plantGrowthSound, base.transform.position);
				}
				UpdateVisualsForCurrentState();
			}
		}

		private void HandlePlantSeed()
		{
			PlantType equippedSeedType = GetEquippedSeedType();
			if (equippedSeedType != PlantType.None)
			{
				PlantSeed(equippedSeedType);
				ObjectivesEventBus.Raise(ObjectiveSignal.SeedPlanted, this);
				HeldItem heldItem = playerService?.EquipmentManager?.EquippedEntity;
				if (heldItem != null)
				{
					playerService.EquipmentManager.Consume();
					CmdDestroySeed(heldItem.netIdentity);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdDestroySeed(NetworkIdentity seedIdentity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(seedIdentity);
			SendCommandInternal("System.Void NomadDrive.Features.Planting.PlantPot::CmdDestroySeed(Mirror.NetworkIdentity)", 56893832, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void HandleWaterPlant()
		{
			if (_equippedWaterContainer != null && !(_equippedWaterContainerComponent == null))
			{
				float waterRequiredForCurrentPhase = GetWaterRequiredForCurrentPhase();
				if (waterPlantSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(waterPlantSound, base.transform.position);
				}
				CmdWaterPlantWithContainer(_equippedWaterContainerComponent.netIdentity, waterRequiredForCurrentPhase);
				ObjectivesEventBus.Raise(ObjectiveSignal.PlantWatered, this);
			}
		}

		private void HandleInsufficientWaterFeedback()
		{
			float waterRequiredForCurrentPhase = GetWaterRequiredForCurrentPhase();
			float num = _equippedWaterContainer?.CurrentAmount ?? 0f;
			string format = _localizationService?.Localize("@plant.water_insufficient") ?? "@plant.water_insufficient";
			_uiFeedbackManager?.CreateFloatingMessage(string.Format(format, waterRequiredForCurrentPhase.ToString("F0"), num.ToString("F0")), FeedbackType.Warning);
		}

		public void PlantSeed(PlantType plantType)
		{
			if (PlantState == PlantState.Empty)
			{
				if (plantSeedSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(plantSeedSound, base.transform.position);
				}
				CmdPlantSeed((byte)plantType);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdPlantSeed(byte plantTypeByte)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, plantTypeByte);
			SendCommandInternal("System.Void NomadDrive.Features.Planting.PlantPot::CmdPlantSeed(System.Byte)", -343247737, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void WaterPlant()
		{
			if (CanWater())
			{
				CmdWaterPlantDebug();
			}
		}

		public bool CanWater()
		{
			if (PlantState != PlantState.Planted)
			{
				return PlantState == PlantState.NeedsWater;
			}
			return true;
		}

		[Command(requiresAuthority = false)]
		private void CmdWaterPlantWithContainer(NetworkIdentity containerIdentity, float waterAmount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkIdentity(containerIdentity);
			writer.WriteFloat(waterAmount);
			SendCommandInternal("System.Void NomadDrive.Features.Planting.PlantPot::CmdWaterPlantWithContainer(Mirror.NetworkIdentity,System.Single)", 605547383, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdWaterPlantDebug()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Planting.PlantPot::CmdWaterPlantDebug()", -8371137, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ResetPot()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::ResetPot()' called when server was not active");
				return;
			}
			ClearSpawnedHarvestItems();
			Network_plantTypeByte = 0;
			Network_plantStateByte = 0;
			Network_currentPhase = 0;
			Network_lastWateredTime = 0.0;
			Network_phaseStartTime = 0.0;
			_isGrowthTimerActive = false;
		}

		[Server]
		private void StartServerTimers()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::StartServerTimers()' called when server was not active");
			}
			else if (PlantState == PlantState.Growing)
			{
				StartGrowthTimerAsync().Forget();
			}
		}

		[AsyncStateMachine(typeof(_003CStartGrowthTimerAsync_003Ed__101))]
		[Server]
		private UniTaskVoid StartGrowthTimerAsync()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'Cysharp.Threading.Tasks.UniTaskVoid NomadDrive.Features.Planting.PlantPot::StartGrowthTimerAsync()' called when server was not active");
				return default(UniTaskVoid);
			}
			_003CStartGrowthTimerAsync_003Ed__101 stateMachine = default(_003CStartGrowthTimerAsync_003Ed__101);
			stateMachine._003C_003Et__builder = AsyncUniTaskVoidMethodBuilder.Create();
			stateMachine._003C_003E4__this = this;
			stateMachine._003C_003E1__state = -1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			return stateMachine._003C_003Et__builder.Task;
		}

		[Server]
		private void AdvanceToNextPhase()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::AdvanceToNextPhase()' called when server was not active");
				return;
			}
			PlantConfig activePlantConfig = ActivePlantConfig;
			if (!(activePlantConfig == null))
			{
				Network_currentPhase = (byte)(_currentPhase + 1);
				if (activePlantConfig.IsLastPhase(_currentPhase))
				{
					Network_plantStateByte = 4;
					SpawnHarvestItemsAsync().Forget();
				}
				else
				{
					Network_plantStateByte = 3;
				}
			}
		}

		[AsyncStateMachine(typeof(_003CSpawnHarvestItemsAsync_003Ed__103))]
		[Server]
		private UniTaskVoid SpawnHarvestItemsAsync(int maxCount = -1)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'Cysharp.Threading.Tasks.UniTaskVoid NomadDrive.Features.Planting.PlantPot::SpawnHarvestItemsAsync(System.Int32)' called when server was not active");
				return default(UniTaskVoid);
			}
			_003CSpawnHarvestItemsAsync_003Ed__103 stateMachine = default(_003CSpawnHarvestItemsAsync_003Ed__103);
			stateMachine._003C_003Et__builder = AsyncUniTaskVoidMethodBuilder.Create();
			stateMachine._003C_003E4__this = this;
			stateMachine.maxCount = maxCount;
			stateMachine._003C_003E1__state = -1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			return stateMachine._003C_003Et__builder.Task;
		}

		private bool AreAllHarvestItemsEquipped()
		{
			foreach (NetworkIdentity spawnedHarvestItem in _spawnedHarvestItems)
			{
				if (!(spawnedHarvestItem == null) && spawnedHarvestItem.TryGetComponent<NetworkedTransform>(out var component) && component.ParentNetId == base.netId)
				{
					return false;
				}
			}
			return true;
		}

		[Server]
		private void CheckAndResetIfAllItemsEquipped()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::CheckAndResetIfAllItemsEquipped()' called when server was not active");
			}
			else if (PlantState == PlantState.FullyGrown && _spawnedHarvestItems.Count != 0 && AreAllHarvestItemsEquipped())
			{
				PlantConfig activePlantConfig = ActivePlantConfig;
				_spawnedHarvestItems.Clear();
				_harvestItemNetIds.Clear();
				if (activePlantConfig != null && activePlantConfig.ResetAfterHarvest)
				{
					ResetPotStateOnly();
				}
			}
		}

		[Server]
		private void ResetPotStateOnly()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::ResetPotStateOnly()' called when server was not active");
				return;
			}
			Network_plantTypeByte = 0;
			Network_plantStateByte = 0;
			Network_currentPhase = 0;
			Network_lastWateredTime = 0.0;
			Network_phaseStartTime = 0.0;
			_isGrowthTimerActive = false;
		}

		[Server]
		private void ClearSpawnedHarvestItems()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Planting.PlantPot::ClearSpawnedHarvestItems()' called when server was not active");
				return;
			}
			foreach (NetworkIdentity spawnedHarvestItem in _spawnedHarvestItems)
			{
				if (spawnedHarvestItem != null)
				{
					NetworkServer.Destroy(spawnedHarvestItem.gameObject);
				}
			}
			_spawnedHarvestItems.Clear();
			_harvestItemNetIds.Clear();
		}

		private void UpdateVisualsForCurrentState()
		{
			DeactivateAllVisuals();
			UpdateSoilMaterial();
			if (PlantType == PlantType.None || PlantState == PlantState.Empty)
			{
				return;
			}
			PlantVisualContainer plantVisualContainer = FindPlantVisualContainer(PlantType);
			if (plantVisualContainer != null)
			{
				PlantConfig activePlantConfig = ActivePlantConfig;
				if (!(activePlantConfig == null) && _currentPhase < activePlantConfig.Phases.Count)
				{
					int visualIndex = activePlantConfig.Phases[_currentPhase].VisualIndex;
					plantVisualContainer.ActivatePhase(visualIndex);
				}
			}
		}

		private void UpdateSoilMaterial()
		{
			if (!(soilRenderer == null))
			{
				soilRenderer.GetPropertyBlock(_soilPropertyBlock);
				Vector2 vector = ((PlantState == PlantState.Growing) ? wetSoilSmoothness : drySoilSmoothness);
				_soilPropertyBlock.SetFloat(SmoothnessRemapMinProperty, vector.x);
				_soilPropertyBlock.SetFloat(SmoothnessRemapMaxProperty, vector.y);
				soilRenderer.SetPropertyBlock(_soilPropertyBlock);
			}
		}

		private void DeactivateAllVisuals()
		{
			foreach (PlantVisualContainer plantVisualContainer in plantVisualContainers)
			{
				plantVisualContainer?.DeactivateAll();
			}
		}

		private PlantVisualContainer FindPlantVisualContainer(PlantType plantType)
		{
			foreach (PlantVisualContainer plantVisualContainer in plantVisualContainers)
			{
				if (plantVisualContainer != null && plantVisualContainer.PlantType == plantType)
				{
					return plantVisualContainer;
				}
			}
			return null;
		}

		private string GetTimeToNextPhaseDisplay()
		{
			if (PlantState != PlantState.Growing)
			{
				return "N/A";
			}
			PlantConfig activePlantConfig = ActivePlantConfig;
			if (activePlantConfig == null)
			{
				return "N/A";
			}
			double num = (double)activePlantConfig.GetGrowthTimeForPhase(_currentPhase) - (NetworkTime.time - _phaseStartTime);
			if (!(num > 0.0))
			{
				return "@plant.ready";
			}
			return $"{num:F1}s";
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)2);
			writer.Write(_plantTypeByte);
			writer.Write(_plantStateByte);
			writer.Write(_currentPhase);
			float value = ((_plantStateByte == 2) ? ((float)(NetworkTime.time - _phaseStartTime)) : 0f);
			writer.Write(value);
			List<(string, Vector3, Vector3)> list = new List<(string, Vector3, Vector3)>();
			foreach (NetworkIdentity spawnedHarvestItem in _spawnedHarvestItems)
			{
				if (!(spawnedHarvestItem == null) && spawnedHarvestItem.TryGetComponent<NetworkedTransform>(out var component) && component.ParentNetId == base.netId)
				{
					string text = ctx.ToGuid(spawnedHarvestItem.netId);
					if (!string.IsNullOrEmpty(text))
					{
						Vector3 item = base.transform.InverseTransformPoint(spawnedHarvestItem.transform.position);
						Vector3 eulerAngles = (Quaternion.Inverse(base.transform.rotation) * spawnedHarvestItem.transform.rotation).eulerAngles;
						list.Add((text, item, eulerAngles));
					}
				}
			}
			writer.Write(list.Count);
			foreach (var item2 in list)
			{
				writer.Write(item2.Item1);
				writer.Write(item2.Item2);
				writer.Write(item2.Item3);
			}
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte network_plantTypeByte = reader.ReadByte();
			byte b = reader.ReadByte();
			byte network_currentPhase = reader.ReadByte();
			float num = reader.ReadFloat();
			if (NetworkServer.active)
			{
				Network_plantTypeByte = network_plantTypeByte;
				Network_currentPhase = network_currentPhase;
				Network_lastWateredTime = NetworkTime.time;
				Network_phaseStartTime = NetworkTime.time - (double)num;
				Network_plantStateByte = b;
				if (b == 2)
				{
					StartGrowthTimerAsync().Forget();
				}
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
			byte b = reader.ReadByte();
			reader.ReadByte();
			reader.ReadByte();
			reader.ReadByte();
			reader.ReadFloat();
			if (!NetworkServer.active || b < 2)
			{
				return;
			}
			int num = reader.ReadInt();
			NetworkedTransform component = GetComponent<NetworkedTransform>();
			for (int i = 0; i < num; i++)
			{
				string guid = reader.ReadString();
				Vector3 positionOffset = reader.ReadVector3();
				Vector3 rotationOffset = reader.ReadVector3();
				if (ctx.TryToNetId(guid, out var num2) && NetworkServer.spawned.TryGetValue(num2, out var value) && !(value == null))
				{
					_spawnedHarvestItems.Add(value);
					_harvestItemNetIds.Add(num2);
					if (component != null && value.TryGetComponent<NetworkedTransform>(out var component2))
					{
						NetworkedTransformParentingConfig parentingConfig = new NetworkedTransformParentingConfig
						{
							PositionOffset = positionOffset,
							RotationOffset = rotationOffset,
							KeepPositionAxes = false,
							KeepRotationAxes = false
						};
						component2.SetParent(component, parentingConfig, 0);
					}
				}
			}
			if (_spawnedHarvestItems.Count == 0 && _plantStateByte == 4)
			{
				PlantConfig activePlantConfig = ActivePlantConfig;
				if ((object)activePlantConfig != null && activePlantConfig.ResetAfterHarvest)
				{
					ResetPotStateOnly();
				}
			}
		}

		public PlantPot()
		{
			InitSyncObject(_harvestItemNetIds);
			_Mirror_SyncVarHookDelegate__plantTypeByte = OnPlantTypeChanged;
			_Mirror_SyncVarHookDelegate__plantStateByte = OnPlantStateChanged;
			_Mirror_SyncVarHookDelegate__currentPhase = OnCurrentPhaseChanged;
		}

		static PlantPot()
		{
			SmoothnessRemapMinProperty = Shader.PropertyToID("_SmoothnessRemapMin");
			SmoothnessRemapMaxProperty = Shader.PropertyToID("_SmoothnessRemapMax");
			RemoteProcedureCalls.RegisterCommand(typeof(PlantPot), "System.Void NomadDrive.Features.Planting.PlantPot::CmdCheckAndResetIfAllItemsEquipped()", InvokeUserCode_CmdCheckAndResetIfAllItemsEquipped, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlantPot), "System.Void NomadDrive.Features.Planting.PlantPot::CmdDestroySeed(Mirror.NetworkIdentity)", InvokeUserCode_CmdDestroySeed__NetworkIdentity, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlantPot), "System.Void NomadDrive.Features.Planting.PlantPot::CmdPlantSeed(System.Byte)", InvokeUserCode_CmdPlantSeed__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlantPot), "System.Void NomadDrive.Features.Planting.PlantPot::CmdWaterPlantWithContainer(Mirror.NetworkIdentity,System.Single)", InvokeUserCode_CmdWaterPlantWithContainer__NetworkIdentity__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlantPot), "System.Void NomadDrive.Features.Planting.PlantPot::CmdWaterPlantDebug()", InvokeUserCode_CmdWaterPlantDebug, requiresAuthority: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdCheckAndResetIfAllItemsEquipped()
		{
			CheckAndResetIfAllItemsEquipped();
		}

		protected static void InvokeUserCode_CmdCheckAndResetIfAllItemsEquipped(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdCheckAndResetIfAllItemsEquipped called on client.");
			}
			else
			{
				((PlantPot)obj).UserCode_CmdCheckAndResetIfAllItemsEquipped();
			}
		}

		protected void UserCode_CmdDestroySeed__NetworkIdentity(NetworkIdentity seedIdentity)
		{
			if (seedIdentity != null)
			{
				NetworkServer.Destroy(seedIdentity.gameObject);
			}
		}

		protected static void InvokeUserCode_CmdDestroySeed__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDestroySeed called on client.");
			}
			else
			{
				((PlantPot)obj).UserCode_CmdDestroySeed__NetworkIdentity(reader.ReadNetworkIdentity());
			}
		}

		protected void UserCode_CmdPlantSeed__Byte(byte plantTypeByte)
		{
			if (_plantStateByte == 0)
			{
				if (plantConfigDatabase?.GetConfig((PlantType)plantTypeByte) == null)
				{
					EvilLogger.LogError($"[PlantPot] No config found for PlantType: {(PlantType)plantTypeByte}", "CmdPlantSeed", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Planting\\Scripts\\PlantPot.cs", 535);
					return;
				}
				Network_plantTypeByte = plantTypeByte;
				Network_plantStateByte = 1;
				Network_currentPhase = 0;
				Network_lastWateredTime = 0.0;
				Network_phaseStartTime = 0.0;
			}
		}

		protected static void InvokeUserCode_CmdPlantSeed__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPlantSeed called on client.");
			}
			else
			{
				((PlantPot)obj).UserCode_CmdPlantSeed__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdWaterPlantWithContainer__NetworkIdentity__Single(NetworkIdentity containerIdentity, float waterAmount)
		{
			if ((_plantStateByte == 1 || _plantStateByte == 3) && !(ActivePlantConfig == null) && !(containerIdentity == null) && containerIdentity.TryGetComponent<LiquidContainerComponent>(out var component) && component.CurrentLiquidType == LiquidType.Water && !(component.CurrentAmount < waterAmount))
			{
				component.Drain(waterAmount);
				Network_lastWateredTime = NetworkTime.time;
				Network_phaseStartTime = NetworkTime.time;
				Network_plantStateByte = 2;
				StartGrowthTimerAsync().Forget();
			}
		}

		protected static void InvokeUserCode_CmdWaterPlantWithContainer__NetworkIdentity__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdWaterPlantWithContainer called on client.");
			}
			else
			{
				((PlantPot)obj).UserCode_CmdWaterPlantWithContainer__NetworkIdentity__Single(reader.ReadNetworkIdentity(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdWaterPlantDebug()
		{
			if ((_plantStateByte == 1 || _plantStateByte == 3) && !(ActivePlantConfig == null))
			{
				Network_lastWateredTime = NetworkTime.time;
				Network_phaseStartTime = NetworkTime.time;
				Network_plantStateByte = 2;
				StartGrowthTimerAsync().Forget();
			}
		}

		protected static void InvokeUserCode_CmdWaterPlantDebug(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdWaterPlantDebug called on client.");
			}
			else
			{
				((PlantPot)obj).UserCode_CmdWaterPlantDebug();
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _plantTypeByte);
				NetworkWriterExtensions.WriteByte(writer, _plantStateByte);
				NetworkWriterExtensions.WriteByte(writer, _currentPhase);
				writer.WriteDouble(_lastWateredTime);
				writer.WriteDouble(_phaseStartTime);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _plantTypeByte);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _plantStateByte);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _currentPhase);
			}
			if ((syncVarDirtyBits & 0x1000L) != 0L)
			{
				writer.WriteDouble(_lastWateredTime);
			}
			if ((syncVarDirtyBits & 0x2000L) != 0L)
			{
				writer.WriteDouble(_phaseStartTime);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _plantTypeByte, _Mirror_SyncVarHookDelegate__plantTypeByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _plantStateByte, _Mirror_SyncVarHookDelegate__plantStateByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _currentPhase, _Mirror_SyncVarHookDelegate__currentPhase, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _lastWateredTime, null, reader.ReadDouble());
				GeneratedSyncVarDeserialize(ref _phaseStartTime, null, reader.ReadDouble());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _plantTypeByte, _Mirror_SyncVarHookDelegate__plantTypeByte, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _plantStateByte, _Mirror_SyncVarHookDelegate__plantStateByte, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _currentPhase, _Mirror_SyncVarHookDelegate__currentPhase, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x1000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _lastWateredTime, null, reader.ReadDouble());
			}
			if ((num & 0x2000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _phaseStartTime, null, reader.ReadDouble());
			}
		}
	}
}
