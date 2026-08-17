using System.Runtime.InteropServices;
using Mirror;
using NomadDrive.Features.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Tools
{
	public class ConsumableBattery : HeldItem
	{
		[Header("Battery")]
		[FormerlySerializedAs("chargeAmount")]
		[SerializeField]
		private float maxCharge = 100f;

		[SyncVar]
		private float _currentCharge;

		public float CurrentCharge => _currentCharge;

		public float MaxCharge => maxCharge;

		public float ChargeRatio
		{
			get
			{
				if (!(maxCharge > 0f))
				{
					return 0f;
				}
				return Mathf.Clamp01(_currentCharge / maxCharge);
			}
		}

		public float Network_currentCharge
		{
			get
			{
				return _currentCharge;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _currentCharge, 512uL, null);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			Network_currentCharge = maxCharge;
		}

		public void Destroy()
		{
			NetworkServer.Destroy(base.gameObject);
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
				writer.WriteFloat(_currentCharge);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteFloat(_currentCharge);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
			}
		}
	}
}
