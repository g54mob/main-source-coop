using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.Common
{
	[AddComponentMenu("")]
	public class RandomColor : NetworkBehaviour
	{
		private Material cachedMaterial;

		[SyncVar(hook = "SetColor")]
		public Color32 color = Color.black;

		public Action<Color32, Color32> _Mirror_SyncVarHookDelegate_color;

		public Color32 Networkcolor
		{
			get
			{
				return color;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref color, 1uL, _Mirror_SyncVarHookDelegate_color);
			}
		}

		private void SetColor(Color32 _, Color32 newColor)
		{
			if (cachedMaterial == null)
			{
				cachedMaterial = GetComponentInChildren<Renderer>().material;
			}
			cachedMaterial.color = newColor;
		}

		public override void OnStartServer()
		{
			if (color == Color.black)
			{
				Networkcolor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			}
		}

		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(cachedMaterial);
		}

		public RandomColor()
		{
			_Mirror_SyncVarHookDelegate_color = SetColor;
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
				writer.WriteColor32(color);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteColor32(color);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref color, _Mirror_SyncVarHookDelegate_color, reader.ReadColor32());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref color, _Mirror_SyncVarHookDelegate_color, reader.ReadColor32());
			}
		}
	}
}
