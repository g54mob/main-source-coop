using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Consumables.UI;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Player;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Consumables
{
	public class OrganicFood : Food
	{
		[Header("Cooking")]
		[SerializeField]
		private CookedLevel _cookedLevelDebug;

		[SyncVar(hook = "OnCookedLevelChanged")]
		private byte _cookedLevelByte;

		[SyncVar(hook = "OnCurrentCookingDurationChange")]
		private float _currentCookingDuration;

		[SyncVar(hook = "OnCookingProcessStateChange")]
		private bool _isCookingProcessActive;

		public UnityEvent onMidCooked;

		public UnityEvent onWellCooked;

		public UnityEvent onBurned;

		public OrganicFoodConfig organicFoodConfig;

		[Header("Audio")]
		[Tooltip("Loop sound played while this food is actively cooking. Each client owns its own AudioHandle.")]
		[SerializeField]
		private SoundID cookingSound;

		private AudioHandle _cookingLoop;

		private float _baseNutritionValue;

		private bool _baseNutritionCaptured;

		[Inject]
		private OrganicFoodInfoPanel _cookingInfoPanel;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__cookedLevelByte;

		public Action<float, float> _Mirror_SyncVarHookDelegate__currentCookingDuration;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isCookingProcessActive;

		public CookedLevel CookedLevel
		{
			get
			{
				return (CookedLevel)_cookedLevelByte;
			}
			private set
			{
				Network_cookedLevelByte = (byte)value;
			}
		}

		public float CurrentCookingDuration
		{
			get
			{
				return _currentCookingDuration;
			}
			private set
			{
				Network_currentCookingDuration = value;
			}
		}

		public bool IsCookingProcessActive
		{
			get
			{
				return _isCookingProcessActive;
			}
			private set
			{
				Network_isCookingProcessActive = value;
			}
		}

		public bool IsSpoiled => base.FoodDurabilityType == FoodDurabilityType.Rotten;

		public float CurrentCookPhaseProgress01
		{
			get
			{
				if (CookedLevel == CookedLevel.Burned)
				{
					return 1f;
				}
				if (!TryGetCookPhaseBounds(out var start, out var end))
				{
					return 0f;
				}
				float num = end - start;
				if (num <= 0f)
				{
					return 1f;
				}
				return Mathf.Clamp01((CurrentCookingDuration - start) / num);
			}
		}

		public float SecondsToNextCookLevel
		{
			get
			{
				if (CookedLevel == CookedLevel.Burned)
				{
					return 0f;
				}
				if (!TryGetCookPhaseBounds(out var _, out var end))
				{
					return 0f;
				}
				float num = end - CurrentCookingDuration;
				if (!(num > 0f))
				{
					return 0f;
				}
				return num;
			}
		}

		public byte Network_cookedLevelByte
		{
			get
			{
				return _cookedLevelByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _cookedLevelByte, 2048uL, _Mirror_SyncVarHookDelegate__cookedLevelByte);
			}
		}

		public float Network_currentCookingDuration
		{
			get
			{
				return _currentCookingDuration;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _currentCookingDuration, 4096uL, _Mirror_SyncVarHookDelegate__currentCookingDuration);
			}
		}

		public bool Network_isCookingProcessActive
		{
			get
			{
				return _isCookingProcessActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isCookingProcessActive, 8192uL, _Mirror_SyncVarHookDelegate__isCookingProcessActive);
			}
		}

		private bool TryGetCookPhaseBounds(out float start, out float end)
		{
			start = 0f;
			end = 0f;
			SerializableDictionary<CookedLevel, float> serializableDictionary = ((organicFoodConfig != null) ? organicFoodConfig.cookingDurationsCollection : null);
			if (serializableDictionary == null)
			{
				return false;
			}
			switch (CookedLevel)
			{
			case CookedLevel.Raw:
				start = 0f;
				return serializableDictionary.TryGetValue(CookedLevel.MidCooked, out end);
			case CookedLevel.MidCooked:
				return serializableDictionary.TryGetValue(CookedLevel.MidCooked, out start) & serializableDictionary.TryGetValue(CookedLevel.WellCooked, out end);
			case CookedLevel.WellCooked:
				return serializableDictionary.TryGetValue(CookedLevel.WellCooked, out start) & serializableDictionary.TryGetValue(CookedLevel.Burned, out end);
			default:
				return false;
			}
		}

		protected override bool CanSpoil()
		{
			return CookedLevel == CookedLevel.Raw;
		}

		protected override void Awake()
		{
			base.Awake();
			onMidCooked.AddListener(OnMidCookedActions);
			onWellCooked.AddListener(OnWellCookedActions);
			onBurned.AddListener(OnBurnedActions);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			onMidCooked.RemoveListener(OnMidCookedActions);
			onWellCooked.RemoveListener(OnWellCookedActions);
			onBurned.RemoveListener(OnBurnedActions);
		}

		protected override void Start()
		{
			base.Start();
			CalculatePlayerStatEffects();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			InitOrganicFoodCo();
		}

		private async UniTask InitOrganicFoodCo()
		{
			await UniTask.Delay(100);
			if (organicFoodConfig == null)
			{
				throw new Exception("OrganicFoodConfig is not set for " + base.gameObject.name);
			}
			CaptureBaseNutrition();
			if (!_restoredFromSave)
			{
				CmdSetCurrentCookingDuration(0f);
				CmdSetCookedLevel(CookedLevel.Raw);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (organicFoodConfig == null)
			{
				throw new Exception("OrganicFoodConfig is not set for " + base.gameObject.name);
			}
			if (base.FoodDurabilityType != FoodDurabilityType.Rotten)
			{
				SetCookedMaterial(CookedLevel);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (base.FoodDurabilityType != FoodDurabilityType.Rotten)
			{
				SetCookedMaterial(CookedLevel);
			}
			CalculatePlayerStatEffects();
			if (_isCookingProcessActive)
			{
				StartCookingLoop();
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			if (IsCookingProcessActive)
			{
				StopCooking();
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			_cookingInfoPanel?.SetFood(this);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			_cookingInfoPanel?.ClearFood();
		}

		public void StartCooking()
		{
			if (base.IsCookable)
			{
				CmdStartCooking();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdStartCooking()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.OrganicFood::CmdStartCooking()", 1931827986, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void StopCooking()
		{
			CmdStopCooking();
		}

		[Command(requiresAuthority = false)]
		private void CmdStopCooking()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.OrganicFood::CmdStopCooking()", -949504060, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetCurrentCookingDuration(float value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(value);
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.OrganicFood::CmdSetCurrentCookingDuration(System.Single)", -1492771456, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdSetCookedLevel(CookedLevel level)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel(writer, level);
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.OrganicFood::CmdSetCookedLevel(NomadDrive.Features.Consumables.CookedLevel)", 2135336658, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void SetCookedMaterial(CookedLevel level)
		{
			if (organicFoodConfig == null)
			{
				EvilLogger.LogError("[OrganicFood] " + base.gameObject.name + ": organicFoodConfig is null!", "SetCookedMaterial", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\OrganicFood.cs", 248);
				return;
			}
			if (organicFoodConfig.cookingStateMaterials == null)
			{
				EvilLogger.LogError("[OrganicFood] " + base.gameObject.name + ": cookingStateMaterials is null!", "SetCookedMaterial", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\OrganicFood.cs", 254);
				return;
			}
			Renderer[] renderers = GetRenderers();
			if (renderers.Length == 0 || renderers[0] == null)
			{
				EvilLogger.LogError("[OrganicFood] " + base.gameObject.name + ": No renderers found!", "SetCookedMaterial", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\OrganicFood.cs", 261);
				return;
			}
			Material material = organicFoodConfig.cookingStateMaterials.GetMaterial(level);
			if (material != null)
			{
				renderers[0].material = material;
			}
			else
			{
				EvilLogger.LogError($"[OrganicFood] {base.gameObject.name}: Material not found for level {level}!", "SetCookedMaterial", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\OrganicFood.cs", 273);
			}
		}

		private void CalculatePlayerStatEffects()
		{
			CaptureBaseNutrition();
			if (_baseNutritionCaptured && currentPlayerStatCollection != null && organicFoodConfig.playerStatsModifiers.TryGetValue(CookedLevel, out var value))
			{
				currentPlayerStatCollection[PlayerStatType.Nutrition] = _baseNutritionValue * value;
			}
		}

		private void CaptureBaseNutrition()
		{
			if (!_baseNutritionCaptured && !(consumableConfig == null) && consumableConfig.playerStatCollection != null && consumableConfig.playerStatCollection.TryGetValue(PlayerStatType.Nutrition, out var value))
			{
				_baseNutritionValue = value;
				_baseNutritionCaptured = true;
			}
		}

		private void OnMidCookedActions()
		{
			if (NetworkServer.active)
			{
				CmdSetCookedLevel(CookedLevel.MidCooked);
			}
		}

		private void OnWellCookedActions()
		{
			if (NetworkServer.active)
			{
				CmdSetCookedLevel(CookedLevel.WellCooked);
			}
		}

		private void OnBurnedActions()
		{
			if (NetworkServer.active)
			{
				CmdSetCookedLevel(CookedLevel.Burned);
			}
		}

		private void SetCookingProgress(float cookingProgress)
		{
			CmdSetCurrentCookingDuration(cookingProgress);
		}

		private void OnCurrentCookingDurationChange(float _, float newValue)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			if (newValue >= organicFoodConfig.cookingDurationsCollection[CookedLevel.MidCooked] && newValue < organicFoodConfig.cookingDurationsCollection[CookedLevel.WellCooked])
			{
				if (CookedLevel.Equals(CookedLevel.MidCooked))
				{
					return;
				}
				onMidCooked.Invoke();
			}
			if (newValue >= organicFoodConfig.cookingDurationsCollection[CookedLevel.WellCooked] && newValue < organicFoodConfig.cookingDurationsCollection[CookedLevel.Burned])
			{
				if (CookedLevel.Equals(CookedLevel.WellCooked))
				{
					return;
				}
				onWellCooked.Invoke();
			}
			if (newValue >= organicFoodConfig.cookingDurationsCollection[CookedLevel.Burned] && !CookedLevel.Equals(CookedLevel.Burned))
			{
				onBurned.Invoke();
			}
		}

		private async void OnCookingProcessStateChange(bool _, bool newValue)
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (newValue)
			{
				StartCookingLoop();
				ObjectivesEventBus.Raise(ObjectiveSignal.CookingStarted, this);
			}
			else
			{
				StopCookingLoop();
			}
			if (!newValue)
			{
				return;
			}
			while (CurrentCookingDuration <= organicFoodConfig.cookingDurationsCollection[CookedLevel.Burned] && IsCookingProcessActive && base.IsCookable)
			{
				if (NetworkServer.active)
				{
					SetCookingProgress(CurrentCookingDuration + Time.deltaTime);
				}
				await UniTask.Yield();
			}
			if (NetworkServer.active)
			{
				CmdStopCooking();
			}
		}

		private void OnDestroy()
		{
			StopCookingLoop();
		}

		private void StartCookingLoop()
		{
			if (!_cookingLoop.IsValid && cookingSound.IsValid() && AudioManager != null)
			{
				_cookingLoop = AudioManager.PlayEventAttached(cookingSound, base.gameObject);
			}
		}

		private void StopCookingLoop()
		{
			if (_cookingLoop.IsValid)
			{
				AudioManager?.StopEvent(_cookingLoop);
				_cookingLoop = default(AudioHandle);
			}
		}

		private void OnCookedLevelChanged(byte _, byte newValue)
		{
			if (IsLateJoinCompleted)
			{
				CookedLevel cookedMaterial = (_cookedLevelDebug = (CookedLevel)newValue);
				if (base.FoodDurabilityType != FoodDurabilityType.Rotten)
				{
					SetCookedMaterial(cookedMaterial);
				}
				CalculatePlayerStatEffects();
			}
		}

		public override void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			base.CaptureState(writer, ctx);
			writer.Write(_cookedLevelByte);
			writer.Write(_currentCookingDuration);
		}

		public override void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			base.RestoreSelfState(reader, ctx);
			byte network_cookedLevelByte = reader.ReadByte();
			float network_currentCookingDuration = reader.ReadFloat();
			if (NetworkServer.active)
			{
				Network_cookedLevelByte = network_cookedLevelByte;
				Network_currentCookingDuration = network_currentCookingDuration;
				Network_isCookingProcessActive = false;
			}
		}

		public OrganicFood()
		{
			_Mirror_SyncVarHookDelegate__cookedLevelByte = OnCookedLevelChanged;
			_Mirror_SyncVarHookDelegate__currentCookingDuration = OnCurrentCookingDurationChange;
			_Mirror_SyncVarHookDelegate__isCookingProcessActive = OnCookingProcessStateChange;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdStartCooking()
		{
			if (base.IsCookable)
			{
				IsCookingProcessActive = true;
			}
		}

		protected static void InvokeUserCode_CmdStartCooking(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStartCooking called on client.");
			}
			else
			{
				((OrganicFood)obj).UserCode_CmdStartCooking();
			}
		}

		protected void UserCode_CmdStopCooking()
		{
			IsCookingProcessActive = false;
		}

		protected static void InvokeUserCode_CmdStopCooking(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStopCooking called on client.");
			}
			else
			{
				((OrganicFood)obj).UserCode_CmdStopCooking();
			}
		}

		protected void UserCode_CmdSetCurrentCookingDuration__Single(float value)
		{
			CurrentCookingDuration = value;
		}

		protected static void InvokeUserCode_CmdSetCurrentCookingDuration__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetCurrentCookingDuration called on client.");
			}
			else
			{
				((OrganicFood)obj).UserCode_CmdSetCurrentCookingDuration__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSetCookedLevel__CookedLevel(CookedLevel level)
		{
			CookedLevel = level;
		}

		protected static void InvokeUserCode_CmdSetCookedLevel__CookedLevel(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetCookedLevel called on client.");
			}
			else
			{
				((OrganicFood)obj).UserCode_CmdSetCookedLevel__CookedLevel(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel(reader));
			}
		}

		static OrganicFood()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(OrganicFood), "System.Void NomadDrive.Features.Consumables.OrganicFood::CmdStartCooking()", InvokeUserCode_CmdStartCooking, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(OrganicFood), "System.Void NomadDrive.Features.Consumables.OrganicFood::CmdStopCooking()", InvokeUserCode_CmdStopCooking, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(OrganicFood), "System.Void NomadDrive.Features.Consumables.OrganicFood::CmdSetCurrentCookingDuration(System.Single)", InvokeUserCode_CmdSetCurrentCookingDuration__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(OrganicFood), "System.Void NomadDrive.Features.Consumables.OrganicFood::CmdSetCookedLevel(NomadDrive.Features.Consumables.CookedLevel)", InvokeUserCode_CmdSetCookedLevel__CookedLevel, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _cookedLevelByte);
				writer.WriteFloat(_currentCookingDuration);
				writer.WriteBool(_isCookingProcessActive);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _cookedLevelByte);
			}
			if ((syncVarDirtyBits & 0x1000L) != 0L)
			{
				writer.WriteFloat(_currentCookingDuration);
			}
			if ((syncVarDirtyBits & 0x2000L) != 0L)
			{
				writer.WriteBool(_isCookingProcessActive);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _cookedLevelByte, _Mirror_SyncVarHookDelegate__cookedLevelByte, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _currentCookingDuration, _Mirror_SyncVarHookDelegate__currentCookingDuration, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _isCookingProcessActive, _Mirror_SyncVarHookDelegate__isCookingProcessActive, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _cookedLevelByte, _Mirror_SyncVarHookDelegate__cookedLevelByte, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x1000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _currentCookingDuration, _Mirror_SyncVarHookDelegate__currentCookingDuration, reader.ReadFloat());
			}
			if ((num & 0x2000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isCookingProcessActive, _Mirror_SyncVarHookDelegate__isCookingProcessActive, reader.ReadBool());
			}
		}
	}
}
