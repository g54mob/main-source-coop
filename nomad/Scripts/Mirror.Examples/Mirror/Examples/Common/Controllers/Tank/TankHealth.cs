using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class TankHealth : NetworkBehaviour
	{
		[Header("Components")]
		public TextMesh healthBar;

		[Header("Stats")]
		public byte maxHealth = 5;

		[SyncVar(hook = "OnHealthChanged")]
		public byte health = 5;

		[Header("Respawn")]
		public bool respawn = true;

		public byte respawnTime = 3;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate_health;

		public byte Networkhealth
		{
			get
			{
				return health;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref health, 1uL, _Mirror_SyncVarHookDelegate_health);
			}
		}

		private void OnHealthChanged(byte oldHealth, byte newHealth)
		{
			healthBar.text = new string('-', newHealth);
			if (newHealth >= maxHealth)
			{
				healthBar.color = Color.green;
			}
			if (newHealth < 4)
			{
				healthBar.color = Color.yellow;
			}
			if (newHealth < 2)
			{
				healthBar.color = Color.red;
			}
			if (newHealth < 1)
			{
				healthBar.color = Color.black;
			}
		}

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		public void Reset()
		{
			if (healthBar == null)
			{
				healthBar = base.transform.Find("HealthBar").GetComponent<TextMesh>();
			}
		}

		public override void OnStartServer()
		{
			Networkhealth = maxHealth;
		}

		[ServerCallback]
		public void TakeDamage(byte damage)
		{
			if (!NetworkServer.active || health == 0)
			{
				return;
			}
			if (damage > health)
			{
				Networkhealth = 0;
			}
			else
			{
				Networkhealth = (byte)(health - damage);
			}
			if (health == 0)
			{
				if (base.connectionToClient != null)
				{
					Respawn.RespawnPlayer(respawn, respawnTime, base.connectionToClient);
				}
				else if (base.netIdentity.sceneId != 0L)
				{
					NetworkServer.UnSpawn(base.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		public TankHealth()
		{
			_Mirror_SyncVarHookDelegate_health = OnHealthChanged;
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
				NetworkWriterExtensions.WriteByte(writer, health);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, health);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref health, _Mirror_SyncVarHookDelegate_health, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref health, _Mirror_SyncVarHookDelegate_health, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
