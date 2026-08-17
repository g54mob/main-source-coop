using System;
using System.Runtime.InteropServices;
using EvilCore.EvilSave;
using Mirror;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Engine
{
	[RequireComponent(typeof(ConditionComponent))]
	public class EngineHeatComponent : NetworkBehaviour, INetworkSaveable
	{
		public bool IsLateJoinCompleted;

		[SyncVar(hook = "OnHeatLevelValueChanged")]
		[SerializeField]
		private float _heatLevel;

		[SyncVar(hook = "OnOverheatedValueChanged")]
		[SerializeField]
		private bool _isOverheated;

		public readonly UnityEvent<float, float> OnHeatLevelChanged = new UnityEvent<float, float>();

		public readonly UnityEvent<bool> OnOverheatedChanged = new UnityEvent<bool>();

		private Engine _engine;

		private ConditionComponent _condition;

		private bool _isControlledByModule;

		public Action<float, float> _Mirror_SyncVarHookDelegate__heatLevel;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isOverheated;

		public float HeatLevel => _heatLevel;

		public float HeatRatio => Mathf.Clamp01(_heatLevel / 100f);

		public bool IsOverheated => _isOverheated;

		private EngineConfig Config
		{
			get
			{
				if (!(_engine != null))
				{
					return null;
				}
				return _engine.engineConfig;
			}
		}

		public string ContributorKey => "engineheat";

		public float Network_heatLevel
		{
			get
			{
				return _heatLevel;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _heatLevel, 1uL, _Mirror_SyncVarHookDelegate__heatLevel);
			}
		}

		public bool Network_isOverheated
		{
			get
			{
				return _isOverheated;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isOverheated, 2uL, _Mirror_SyncVarHookDelegate__isOverheated);
			}
		}

		private void Awake()
		{
			_engine = GetComponent<Engine>();
			_condition = GetComponent<ConditionComponent>();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			IsLateJoinCompleted = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			IsLateJoinCompleted = true;
			OnHeatLevelChanged.Invoke(_heatLevel, _heatLevel);
		}

		private void OnHeatLevelValueChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				OnHeatLevelChanged.Invoke(oldValue, newValue);
			}
		}

		private void OnOverheatedValueChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				OnOverheatedChanged.Invoke(newValue);
			}
		}

		[Server]
		public void ServerSetControlled(bool controlled)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Parts.Engine.EngineHeatComponent::ServerSetControlled(System.Boolean)' called when server was not active");
			}
			else
			{
				_isControlledByModule = controlled;
			}
		}

		private void Update()
		{
			if (base.isServer && !_isControlledByModule && !(_heatLevel <= 0f))
			{
				ServerTick(EngineHeatInputs.Off);
			}
		}

		[Server]
		public void ServerTick(in EngineHeatInputs inputs)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Parts.Engine.EngineHeatComponent::ServerTick(NomadDrive.Features.Vehicle.Parts.Engine.EngineHeatInputs&)' called when server was not active");
				return;
			}
			EngineConfig config = Config;
			if (!(config == null))
			{
				float t = ((_condition != null) ? _condition.ConditionRatio : 1f);
				float num = Mathf.Lerp(1f + config.damageHeatPenalty, 1f, t);
				float num5;
				if (inputs.EngineRunning)
				{
					float num2 = Mathf.Lerp(config.idleHeat, config.throttleHeat, Mathf.Clamp01(inputs.LoadFactor)) * num;
					float num3 = (inputs.HasRadiator ? (config.radiatorCoolRate * Mathf.Clamp01(inputs.CoolantRatio)) : 0f);
					float num4 = config.tempCoolFactor * (_heatLevel * 0.01f);
					num5 = num2 - config.runningCoolRate - num3 - num4;
				}
				else
				{
					num5 = 0f - config.engineOffCoolRate;
				}
				Network_heatLevel = Mathf.Clamp(_heatLevel + num5 * Time.deltaTime, 0f, 100f);
				if (!_isOverheated && _heatLevel >= config.overheatThreshold)
				{
					Network_isOverheated = true;
				}
				else if (_isOverheated && _heatLevel <= config.restartThreshold)
				{
					Network_isOverheated = false;
				}
				if (_heatLevel > config.overheatDamageThreshold && _condition != null && _condition.Condition > 0f)
				{
					float num6 = Mathf.InverseLerp(config.overheatDamageThreshold, 100f, _heatLevel);
					_condition.ServerSetCondition(_condition.Condition - config.overheatDamageRate * num6 * Time.deltaTime);
				}
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_heatLevel);
			writer.Write(_isOverheated);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			float value = reader.ReadFloat();
			bool network_isOverheated = reader.ReadBool();
			if (NetworkServer.active)
			{
				Network_heatLevel = Mathf.Clamp(value, 0f, 100f);
				Network_isOverheated = network_isOverheated;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public EngineHeatComponent()
		{
			_Mirror_SyncVarHookDelegate__heatLevel = OnHeatLevelValueChanged;
			_Mirror_SyncVarHookDelegate__isOverheated = OnOverheatedValueChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(_heatLevel);
				writer.WriteBool(_isOverheated);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteFloat(_heatLevel);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteBool(_isOverheated);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _heatLevel, _Mirror_SyncVarHookDelegate__heatLevel, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _isOverheated, _Mirror_SyncVarHookDelegate__isOverheated, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _heatLevel, _Mirror_SyncVarHookDelegate__heatLevel, reader.ReadFloat());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isOverheated, _Mirror_SyncVarHookDelegate__isOverheated, reader.ReadBool());
			}
		}
	}
}
