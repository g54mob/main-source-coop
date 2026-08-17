using System;
using System.Collections;
using System.Runtime.InteropServices;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Consumables
{
	public class Food : Consumable, INetworkSaveable
	{
		[FormerlySerializedAs("FoodConfig")]
		[SerializeField]
		public FoodConfig foodConfig;

		[SyncVar(hook = "OnDurabilityChanged")]
		public float durability;

		[SyncVar(hook = "OnFoodDurabilityTypeChanged")]
		private FoodDurabilityType _foodDurabilityType;

		[Inject]
		private ITimeManager _timeManager;

		public UnityEvent onRotten;

		private bool _isSpoilagePaused;

		private bool _spoilageStarted;

		protected bool _restoredFromSave;

		private bool _durabilityInitialized;

		public Action<float, float> _Mirror_SyncVarHookDelegate_durability;

		public Action<FoodDurabilityType, FoodDurabilityType> _Mirror_SyncVarHookDelegate__foodDurabilityType;

		public override string UseActionPromptId => "Consumable_Eat";

		[field: SerializeField]
		public FoodDurabilityType FoodDurabilityType { get; set; }

		public bool IsCookable
		{
			get
			{
				if (_foodDurabilityType != FoodDurabilityType.Rotten)
				{
					return !foodConfig.isPoisonous;
				}
				return false;
			}
		}

		public virtual string ContributorKey => "food";

		public float Networkdurability
		{
			get
			{
				return durability;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref durability, 512uL, _Mirror_SyncVarHookDelegate_durability);
			}
		}

		public FoodDurabilityType Network_foodDurabilityType
		{
			get
			{
				return _foodDurabilityType;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _foodDurabilityType, 1024uL, _Mirror_SyncVarHookDelegate__foodDurabilityType);
			}
		}

		protected virtual bool CanSpoil()
		{
			return true;
		}

		public void PauseSpoilage()
		{
			_isSpoilagePaused = true;
		}

		public void ResumeSpoilage()
		{
			_isSpoilagePaused = false;
		}

		public override void OnUseButtonDown()
		{
			if (foodConfig != null && (foodConfig.isPoisonous || (FoodDurabilityType == FoodDurabilityType.Rotten && foodConfig.poisonousWhenRotten)))
			{
				playerService.StatsManager.ApplyPoison(foodConfig.poisonRatePerMinute, foodConfig.poisonInitialAmount);
			}
			base.OnUseButtonDown();
			ObjectivesEventBus.Raise(ObjectiveSignal.ConsumableEaten, this);
		}

		protected override void Awake()
		{
			base.Awake();
			onRotten.AddListener(OnRottenActions);
		}

		protected override void Start()
		{
			base.Start();
			if (base.isServer)
			{
				_timeManager?.OnMinutePassed.AddListener(DecreaseDurabilityOnTimePassed);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			StartCoroutine(InitFoodCo());
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SetupFoodConfig();
			if (_foodDurabilityType == FoodDurabilityType.Rotten && foodConfig.hasRottenMaterial)
			{
				UpdateRottenMaterial();
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (NetworkServer.active)
			{
				_timeManager?.OnMinutePassed.RemoveListener(DecreaseDurabilityOnTimePassed);
			}
			onRotten.RemoveListener(OnRottenActions);
		}

		public override void OnEquip()
		{
			base.OnEquip();
			if (!_spoilageStarted)
			{
				CmdStartSpoilage();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdStartSpoilage()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.Food::CmdStartSpoilage()", -1102292087, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private IEnumerator InitFoodCo()
		{
			yield return new WaitForSeconds(0.5f);
			if (!_restoredFromSave && !_durabilityInitialized)
			{
				if (foodConfig == null)
				{
					throw new Exception("FoodConfig is not set for " + base.gameObject.name);
				}
				SetDurability(Mathf.Max(foodConfig.durabilityRange.x, foodConfig.durabilityRange.y));
			}
		}

		public void ServerSetFreshMax()
		{
			if (NetworkServer.active && !_restoredFromSave)
			{
				_durabilityInitialized = true;
				if (!(foodConfig == null))
				{
					Networkdurability = Mathf.Max(foodConfig.durabilityRange.x, foodConfig.durabilityRange.y);
				}
			}
		}

		public void ServerInitializeDurability(int seed)
		{
			if (!NetworkServer.active || _restoredFromSave)
			{
				return;
			}
			_durabilityInitialized = true;
			if (!(foodConfig == null))
			{
				float a = Mathf.Min(foodConfig.durabilityRange.x, foodConfig.durabilityRange.y);
				float num = Mathf.Max(foodConfig.durabilityRange.x, foodConfig.durabilityRange.y);
				if (!foodConfig.randomizeInitialDurability)
				{
					Networkdurability = num;
					return;
				}
				System.Random random = new System.Random(seed ^ 0xF00D);
				Networkdurability = Mathf.Lerp(a, num, (float)random.NextDouble());
			}
		}

		private void SetupFoodConfig()
		{
			if (foodConfig == null)
			{
				throw new Exception("FoodConfig is not set for " + base.gameObject.name);
			}
		}

		[Command]
		private void SetDurability(float value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(value);
			SendCommandInternal("System.Void NomadDrive.Features.Consumables.Food::SetDurability(System.Single)", -904708013, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void DecreaseDurabilityOnTimePassed()
		{
			if (_spoilageStarted && CanSpoil() && !_isSpoilagePaused && !GameSaveService.IsRestoringWorld)
			{
				float num = foodConfig.spoilageMultiplier * 0.01f;
				if (durability - num <= 0f)
				{
					SetDurability(0f);
				}
				else
				{
					SetDurability(durability - num);
				}
			}
		}

		private void OnDurabilityChanged(float _, float newValue)
		{
			if (!_foodDurabilityType.Equals(FoodDurabilityType.Rotten) && newValue <= 0f && NetworkServer.active)
			{
				Network_foodDurabilityType = FoodDurabilityType.Rotten;
			}
		}

		private void OnFoodDurabilityTypeChanged(FoodDurabilityType oldType, FoodDurabilityType newType)
		{
			FoodDurabilityType = newType;
			if (newType == FoodDurabilityType.Rotten && foodConfig.hasRottenMaterial && NetworkServer.active)
			{
				RpcSetRottenMaterial();
			}
		}

		private void OnRottenActions()
		{
			if (foodConfig.hasRottenMaterial)
			{
				UpdateRottenMaterial();
			}
		}

		[ClientRpc]
		private void RpcSetRottenMaterial()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Consumables.Food::RpcSetRottenMaterial()", -738621547, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void UpdateRottenMaterial()
		{
			if (foodConfig.hasRottenMaterial)
			{
				GetRenderers()[0].material = foodConfig.rottenMaterial;
			}
		}

		public virtual void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(durability);
			writer.Write((byte)_foodDurabilityType);
			writer.Write(_spoilageStarted);
		}

		public virtual void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			float networkdurability = reader.ReadFloat();
			FoodDurabilityType network_foodDurabilityType = (FoodDurabilityType)reader.ReadByte();
			bool spoilageStarted = reader.ReadBool();
			if (NetworkServer.active)
			{
				_restoredFromSave = true;
				_spoilageStarted = spoilageStarted;
				Network_foodDurabilityType = network_foodDurabilityType;
				Networkdurability = networkdurability;
			}
		}

		public virtual void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public Food()
		{
			_Mirror_SyncVarHookDelegate_durability = OnDurabilityChanged;
			_Mirror_SyncVarHookDelegate__foodDurabilityType = OnFoodDurabilityTypeChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdStartSpoilage()
		{
			_spoilageStarted = true;
		}

		protected static void InvokeUserCode_CmdStartSpoilage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStartSpoilage called on client.");
			}
			else
			{
				((Food)obj).UserCode_CmdStartSpoilage();
			}
		}

		protected void UserCode_SetDurability__Single(float value)
		{
			if (NetworkServer.active)
			{
				Networkdurability = value;
			}
		}

		protected static void InvokeUserCode_SetDurability__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetDurability called on client.");
			}
			else
			{
				((Food)obj).UserCode_SetDurability__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_RpcSetRottenMaterial()
		{
			UpdateRottenMaterial();
		}

		protected static void InvokeUserCode_RpcSetRottenMaterial(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetRottenMaterial called on server.");
			}
			else
			{
				((Food)obj).UserCode_RpcSetRottenMaterial();
			}
		}

		static Food()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Food), "System.Void NomadDrive.Features.Consumables.Food::CmdStartSpoilage()", InvokeUserCode_CmdStartSpoilage, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Food), "System.Void NomadDrive.Features.Consumables.Food::SetDurability(System.Single)", InvokeUserCode_SetDurability__Single, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(Food), "System.Void NomadDrive.Features.Consumables.Food::RpcSetRottenMaterial()", InvokeUserCode_RpcSetRottenMaterial);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(durability);
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(writer, _foodDurabilityType);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteFloat(durability);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(writer, _foodDurabilityType);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref durability, _Mirror_SyncVarHookDelegate_durability, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _foodDurabilityType, _Mirror_SyncVarHookDelegate__foodDurabilityType, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref durability, _Mirror_SyncVarHookDelegate_durability, reader.ReadFloat());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _foodDurabilityType, _Mirror_SyncVarHookDelegate__foodDurabilityType, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(reader));
			}
		}
	}
}
