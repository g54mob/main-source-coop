using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
	[AddComponentMenu("")]
	public class PlayerScore : NetworkBehaviour
	{
		[SyncVar]
		public int index;

		[SyncVar]
		public uint score;

		public int Networkindex
		{
			get
			{
				return index;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref index, 1uL, null);
			}
		}

		public uint Networkscore
		{
			get
			{
				return score;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref score, 2uL, null);
			}
		}

		private void OnGUI()
		{
			GUI.Box(new Rect(10f + (float)(index * 110), 10f, 100f, 25f), $"P{index}: {score:0000000}");
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
				writer.WriteVarInt(index);
				writer.WriteVarUInt(score);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(index);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteVarUInt(score);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref index, null, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref score, null, reader.ReadVarUInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref index, null, reader.ReadVarInt());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref score, null, reader.ReadVarUInt());
			}
		}
	}
}
