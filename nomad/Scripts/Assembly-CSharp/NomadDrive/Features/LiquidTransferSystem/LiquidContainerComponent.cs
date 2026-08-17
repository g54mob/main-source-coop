using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.EvilSave;
using EvilCore.Networking;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[RequireComponent(typeof(LiquidContainerDebugger))]
	public class LiquidContainerComponent : NetworkBehaviour, ILiquidContainer, INetworkSaveable
	{
		public bool IsLateJoinCompleted;

		[SyncVar]
		[SerializeField]
		private LiquidType currentLiquidType;

		[Header("Initial Values")]
		[SyncVar]
		[SerializeField]
		private float liquidCapacity = 100f;

		[SyncVar(hook = "OnLiquidAmountChanged")]
		private float _liquidAmount;

		public readonly UnityEvent OnLiquidEmpty = new UnityEvent();

		public readonly UnityEvent OnLiquidFull = new UnityEvent();

		[Header("Objectives Integration")]
		[Tooltip("Optional. When set, every liquid-amount change raises ObjectivesEventBus.RaiseRatio(key, FillRatio). Use this on a vehicle's gas FluidContainer with e.g. 'vehicle_gas' and reference the same key from a LiquidContainerFillTrigger.")]
		[SerializeField]
		private string objectivesSignalKey;

		[Header("Randomized Initial Fill (server, seed-deterministic)")]
		[Tooltip("When enabled, this container spawns with a random fill between min/max ratio of capacity, derived deterministically from the world seed + spawn position. Leave off for containers that should start empty (coffee mug/pot, vehicle tanks) or full sources (gas pump).")]
		[SerializeField]
		private bool randomizeInitialFill;

		[SerializeField]
		[Range(0f, 1f)]
		private float minInitialFillRatio = 0.25f;

		[SerializeField]
		[Range(0f, 1f)]
		private float maxInitialFillRatio = 1f;

		public Action<float, float> _Mirror_SyncVarHookDelegate__liquidAmount;

		public Vector3 Position => base.transform.position;

		public string Name => GetComponent<Interactable>()?.interactableName;

		[field: SerializeField]
		public SoundID TransferSound { get; set; }

		[field: SerializeField]
		public AudioParameter TransferActiveParameter { get; private set; }

		public LiquidContainerState State
		{
			get
			{
				if (_liquidAmount <= 0f)
				{
					return LiquidContainerState.Empty;
				}
				if (Mathf.Approximately(_liquidAmount, liquidCapacity) || _liquidAmount >= liquidCapacity)
				{
					return LiquidContainerState.Full;
				}
				return LiquidContainerState.HasLiquid;
			}
		}

		[field: SerializeField]
		public LiquidType AllowedLiquidTypes { get; set; }

		public LiquidType CurrentLiquidType
		{
			get
			{
				if (_liquidAmount <= 0f)
				{
					return LiquidType.Empty;
				}
				if (currentLiquidType != LiquidType.Empty)
				{
					return currentLiquidType;
				}
				if (!IsSingleAllowedType)
				{
					return LiquidType.Empty;
				}
				return AllowedLiquidTypes;
			}
			set
			{
				if (base.isServer)
				{
					NetworkcurrentLiquidType = value;
				}
			}
		}

		private bool IsSingleAllowedType
		{
			get
			{
				if (AllowedLiquidTypes != LiquidType.Empty)
				{
					return (AllowedLiquidTypes & (AllowedLiquidTypes - 1)) == 0;
				}
				return false;
			}
		}

		[field: SerializeField]
		public LiquidTransferType TransferType { get; set; }

		public UnityEvent<float, float> OnLiquidAmountChangedEvent { get; set; } = new UnityEvent<float, float>();

		public float CurrentAmount
		{
			get
			{
				return _liquidAmount;
			}
			set
			{
				if (base.isServer)
				{
					if (float.IsNaN(value) || float.IsInfinity(value))
					{
						value = 0f;
					}
					Network_liquidAmount = Mathf.Clamp(value, 0f, liquidCapacity);
				}
			}
		}

		public float Capacity
		{
			get
			{
				return liquidCapacity;
			}
			set
			{
				if (base.isServer)
				{
					NetworkliquidCapacity = value;
				}
			}
		}

		public float FillRatio
		{
			get
			{
				if (!(liquidCapacity > 0f))
				{
					return 0f;
				}
				return _liquidAmount / liquidCapacity;
			}
		}

		[field: SerializeField]
		public float FillingSpeed { get; set; } = 1f;

		public bool RandomizeInitialFill => randomizeInitialFill;

		public string ContributorKey => "liquid";

		public LiquidType NetworkcurrentLiquidType
		{
			get
			{
				return currentLiquidType;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref currentLiquidType, 1uL, null);
			}
		}

		public float NetworkliquidCapacity
		{
			get
			{
				return liquidCapacity;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref liquidCapacity, 2uL, null);
			}
		}

		public float Network_liquidAmount
		{
			get
			{
				return _liquidAmount;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _liquidAmount, 4uL, _Mirror_SyncVarHookDelegate__liquidAmount);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			IsLateJoinCompleted = true;
			RaiseObjectivesSnapshot();
			if (randomizeInitialFill)
			{
				ServerApplyRandomizedFill();
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			IsLateJoinCompleted = true;
			RaiseObjectivesSnapshot();
		}

		public void SetLateJoinCompleted()
		{
			IsLateJoinCompleted = true;
		}

		public void ApplyLateJoinerState(float amount, LiquidType type)
		{
			if (float.IsNaN(amount) || float.IsInfinity(amount))
			{
				amount = 0f;
			}
			Network_liquidAmount = Mathf.Clamp(amount, 0f, liquidCapacity);
			NetworkcurrentLiquidType = type;
			if (IsLateJoinCompleted)
			{
				RaiseObjectivesSnapshot();
			}
		}

		private void RaiseObjectivesSnapshot()
		{
			if (!string.IsNullOrEmpty(objectivesSignalKey))
			{
				ObjectivesEventBus.RaiseRatio(objectivesSignalKey, FillRatio);
				ObjectivesEventBus.RaiseAmount(objectivesSignalKey, _liquidAmount);
			}
		}

		private void OnLiquidAmountChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				OnLiquidAmountChangedEvent?.Invoke(oldValue, newValue);
				if (!string.IsNullOrEmpty(objectivesSignalKey))
				{
					ObjectivesEventBus.RaiseRatio(objectivesSignalKey, FillRatio);
					ObjectivesEventBus.RaiseAmount(objectivesSignalKey, newValue);
				}
				if (oldValue > 0f && newValue <= 0f)
				{
					OnLiquidEmpty.Invoke();
				}
				else if (oldValue < liquidCapacity && Mathf.Approximately(newValue, liquidCapacity))
				{
					OnLiquidFull.Invoke();
				}
			}
		}

		public float Fill(float amount)
		{
			if (!base.isServer)
			{
				CmdAddLiquid(amount);
				return 0f;
			}
			return ServerAddLiquid(amount);
		}

		public float Drain(float amount)
		{
			if (!base.isServer)
			{
				CmdRemoveLiquid(amount);
				return 0f;
			}
			return ServerRemoveLiquid(amount);
		}

		public void RequestTransferTo(LiquidContainerComponent target, float amount)
		{
			if (!(target == null) && !(amount <= 0f))
			{
				if (base.isServer)
				{
					ServerTransferTo(target, amount);
				}
				else
				{
					CmdTransferTo(target, amount);
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdTransferTo(LiquidContainerComponent target, float amount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkBehaviour(target);
			writer.WriteFloat(amount);
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdTransferTo(NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent,System.Single)", -1617149218, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerTransferTo(LiquidContainerComponent target, float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerTransferTo(NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent,System.Single)' called when server was not active");
			}
			else
			{
				if (target == null || amount <= 0f)
				{
					return;
				}
				float num = Mathf.Min(amount, _liquidAmount);
				if (!(num <= 0f))
				{
					if (target.State == LiquidContainerState.Empty && currentLiquidType != LiquidType.Empty)
					{
						target.NetworkcurrentLiquidType = currentLiquidType;
					}
					Network_liquidAmount = _liquidAmount - num;
					float num2 = target.ServerAddLiquid(num);
					Network_liquidAmount = _liquidAmount + num2;
				}
			}
		}

		public void SetAmount(float amount)
		{
			if (!base.isServer)
			{
				CmdSetAmount(amount);
			}
			else
			{
				ServerSetAmount(amount);
			}
		}

		public void SetLiquidType(LiquidType type)
		{
			if (!base.isServer)
			{
				CmdSetLiquidType(type);
			}
			else
			{
				ServerSetLiquidType(type);
			}
		}

		[Server]
		public void ServerApplyRandomizedFill()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerApplyRandomizedFill()' called when server was not active");
			}
			else if (!(liquidCapacity <= 0f))
			{
				float num = Mathf.Clamp01(Mathf.Min(minInitialFillRatio, maxInitialFillRatio));
				float num2 = Mathf.Clamp01(Mathf.Max(minInitialFillRatio, maxInitialFillRatio));
				WorldGenerator instance = NetworkSingleton<WorldGenerator>.Instance;
				float num3;
				if (instance != null && instance.SeedManager != null)
				{
					System.Random random = instance.SeedManager.CreateRandomFromPosition(base.transform.position);
					num3 = num + (float)random.NextDouble() * (num2 - num);
				}
				else
				{
					num3 = num2;
				}
				ServerSetAmount(liquidCapacity * num3);
				_ = currentLiquidType;
			}
		}

		[Server]
		private float ServerAddLiquid(float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Single NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerAddLiquid(System.Single)' called when server was not active");
				return default(float);
			}
			if (float.IsNaN(amount) || float.IsInfinity(amount))
			{
				return 0f;
			}
			Network_liquidAmount = _liquidAmount + amount;
			if (_liquidAmount > liquidCapacity)
			{
				float result = _liquidAmount - liquidCapacity;
				Network_liquidAmount = liquidCapacity;
				return result;
			}
			return 0f;
		}

		[Server]
		private float ServerRemoveLiquid(float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Single NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerRemoveLiquid(System.Single)' called when server was not active");
				return default(float);
			}
			if (float.IsNaN(amount) || float.IsInfinity(amount))
			{
				return 0f;
			}
			Network_liquidAmount = _liquidAmount - amount;
			if (_liquidAmount < 0f)
			{
				float result = Mathf.Abs(_liquidAmount);
				Network_liquidAmount = 0f;
				return result;
			}
			return 0f;
		}

		[Server]
		private void ServerSetAmount(float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerSetAmount(System.Single)' called when server was not active");
				return;
			}
			if (float.IsNaN(amount) || float.IsInfinity(amount))
			{
				amount = 0f;
			}
			Network_liquidAmount = Mathf.Clamp(amount, 0f, liquidCapacity);
		}

		[Server]
		private void ServerSetLiquidType(LiquidType type)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::ServerSetLiquidType(NomadDrive.Features.LiquidTransferSystem.LiquidType)' called when server was not active");
			}
			else
			{
				NetworkcurrentLiquidType = type;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdAddLiquid(float amount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(amount);
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdAddLiquid(System.Single)", 1773329007, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRemoveLiquid(float amount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(amount);
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdRemoveLiquid(System.Single)", 387527454, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void CmdSetAmount(float amount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(amount);
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdSetAmount(System.Single)", 131922454, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void CmdSetLiquidType(LiquidType type)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(writer, type);
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdSetLiquidType(NomadDrive.Features.LiquidTransferSystem.LiquidType)", -1049104367, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_liquidAmount);
			writer.Write((int)currentLiquidType);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			float num = reader.ReadFloat();
			LiquidType liquidType = (LiquidType)reader.ReadInt();
			if (NetworkServer.active)
			{
				if (float.IsNaN(num) || float.IsInfinity(num))
				{
					num = 0f;
				}
				SetLiquidType(liquidType);
				SetAmount(num);
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public LiquidContainerComponent()
		{
			_Mirror_SyncVarHookDelegate__liquidAmount = OnLiquidAmountChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdTransferTo__LiquidContainerComponent__Single(LiquidContainerComponent target, float amount)
		{
			ServerTransferTo(target, amount);
		}

		protected static void InvokeUserCode_CmdTransferTo__LiquidContainerComponent__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTransferTo called on client.");
			}
			else
			{
				((LiquidContainerComponent)obj).UserCode_CmdTransferTo__LiquidContainerComponent__Single(reader.ReadNetworkBehaviour<LiquidContainerComponent>(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdAddLiquid__Single(float amount)
		{
			ServerAddLiquid(amount);
		}

		protected static void InvokeUserCode_CmdAddLiquid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAddLiquid called on client.");
			}
			else
			{
				((LiquidContainerComponent)obj).UserCode_CmdAddLiquid__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_CmdRemoveLiquid__Single(float amount)
		{
			ServerRemoveLiquid(amount);
		}

		protected static void InvokeUserCode_CmdRemoveLiquid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRemoveLiquid called on client.");
			}
			else
			{
				((LiquidContainerComponent)obj).UserCode_CmdRemoveLiquid__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSetAmount__Single(float amount)
		{
			ServerSetAmount(amount);
		}

		protected static void InvokeUserCode_CmdSetAmount__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetAmount called on client.");
			}
			else
			{
				((LiquidContainerComponent)obj).UserCode_CmdSetAmount__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_CmdSetLiquidType__LiquidType(LiquidType type)
		{
			ServerSetLiquidType(type);
		}

		protected static void InvokeUserCode_CmdSetLiquidType__LiquidType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetLiquidType called on client.");
			}
			else
			{
				((LiquidContainerComponent)obj).UserCode_CmdSetLiquidType__LiquidType(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(reader));
			}
		}

		static LiquidContainerComponent()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidContainerComponent), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdTransferTo(NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent,System.Single)", InvokeUserCode_CmdTransferTo__LiquidContainerComponent__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidContainerComponent), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdAddLiquid(System.Single)", InvokeUserCode_CmdAddLiquid__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidContainerComponent), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdRemoveLiquid(System.Single)", InvokeUserCode_CmdRemoveLiquid__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidContainerComponent), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdSetAmount(System.Single)", InvokeUserCode_CmdSetAmount__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(LiquidContainerComponent), "System.Void NomadDrive.Features.LiquidTransferSystem.LiquidContainerComponent::CmdSetLiquidType(NomadDrive.Features.LiquidTransferSystem.LiquidType)", InvokeUserCode_CmdSetLiquidType__LiquidType, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(writer, currentLiquidType);
				writer.WriteFloat(liquidCapacity);
				writer.WriteFloat(_liquidAmount);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(writer, currentLiquidType);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteFloat(liquidCapacity);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteFloat(_liquidAmount);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref currentLiquidType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(reader));
				GeneratedSyncVarDeserialize(ref liquidCapacity, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _liquidAmount, _Mirror_SyncVarHookDelegate__liquidAmount, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref currentLiquidType, null, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(reader));
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref liquidCapacity, null, reader.ReadFloat());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _liquidAmount, _Mirror_SyncVarHookDelegate__liquidAmount, reader.ReadFloat());
			}
		}
	}
}
