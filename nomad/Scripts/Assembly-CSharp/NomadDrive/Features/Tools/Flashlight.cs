using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NomadDrive.Features.Tools
{
	public class Flashlight : ChargableDirectHeldItem
	{
		[SerializeField]
		private Light lightSource;

		[SerializeField]
		private SoundID turnOnSound;

		[SerializeField]
		private SoundID turnOffSound;

		[SyncVar(hook = "OnLightStateChange")]
		private bool _isLightOn;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isLightOn;

		public override string UseActionPromptId => "Flashlight_Toggle";

		protected override bool IsActivelyConsuming => _isLightOn;

		public bool Network_isLightOn
		{
			get
			{
				return _isLightOn;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isLightOn, 2048uL, _Mirror_SyncVarHookDelegate__isLightOn);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			lightSource = GetComponentInChildren(typeof(Light)) as Light;
			if (lightSource == null)
			{
				EvilLogger.LogError("Flashlight requires a Light", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Flashlight\\Scripts\\Flashlight.cs", 28);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			lightSource.enabled = _isLightOn;
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			if (base.HasCharge)
			{
				Toggle();
			}
		}

		protected override void OnBatteryDepleted()
		{
			Network_isLightOn = false;
		}

		private void OnLightStateChange(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				lightSource.enabled = newValue;
				AudioManager?.PlayOneShotAttached(newValue ? turnOnSound : turnOffSound, base.gameObject);
			}
		}

		[Command]
		private void Toggle()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Tools.Flashlight::Toggle()", 1010145410, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public Flashlight()
		{
			_Mirror_SyncVarHookDelegate__isLightOn = OnLightStateChange;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_Toggle()
		{
			Network_isLightOn = !_isLightOn;
		}

		protected static void InvokeUserCode_Toggle(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command Toggle called on client.");
			}
			else
			{
				((Flashlight)obj).UserCode_Toggle();
			}
		}

		static Flashlight()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Flashlight), "System.Void NomadDrive.Features.Tools.Flashlight::Toggle()", InvokeUserCode_Toggle, requiresAuthority: true);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isLightOn);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteBool(_isLightOn);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isLightOn, _Mirror_SyncVarHookDelegate__isLightOn, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isLightOn, _Mirror_SyncVarHookDelegate__isLightOn, reader.ReadBool());
			}
		}
	}
}
