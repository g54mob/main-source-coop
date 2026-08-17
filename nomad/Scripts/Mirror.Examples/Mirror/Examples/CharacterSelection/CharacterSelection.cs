using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	public class CharacterSelection : NetworkBehaviour
	{
		public Transform floatingInfo;

		[SyncVar]
		public int characterNumber;

		public TextMesh textMeshName;

		[SyncVar(hook = "HookSetName")]
		public string playerName = "";

		[SyncVar(hook = "HookSetColor")]
		public Color characterColour;

		private Material cachedMaterial;

		public MeshRenderer[] characterRenderers;

		public Action<string, string> _Mirror_SyncVarHookDelegate_playerName;

		public Action<Color, Color> _Mirror_SyncVarHookDelegate_characterColour;

		public int NetworkcharacterNumber
		{
			get
			{
				return characterNumber;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref characterNumber, 1uL, null);
			}
		}

		public string NetworkplayerName
		{
			get
			{
				return playerName;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref playerName, 2uL, _Mirror_SyncVarHookDelegate_playerName);
			}
		}

		public Color NetworkcharacterColour
		{
			get
			{
				return characterColour;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref characterColour, 4uL, _Mirror_SyncVarHookDelegate_characterColour);
			}
		}

		private void HookSetName(string _old, string _new)
		{
			AssignName();
		}

		private void HookSetColor(Color _old, Color _new)
		{
			AssignColours();
		}

		public void AssignColours()
		{
			MeshRenderer[] array = characterRenderers;
			foreach (MeshRenderer meshRenderer in array)
			{
				cachedMaterial = meshRenderer.material;
				cachedMaterial.color = characterColour;
			}
		}

		private void OnDestroy()
		{
			if ((bool)cachedMaterial)
			{
				UnityEngine.Object.Destroy(cachedMaterial);
			}
		}

		public void AssignName()
		{
			textMeshName.text = playerName;
		}

		public CharacterSelection()
		{
			_Mirror_SyncVarHookDelegate_playerName = HookSetName;
			_Mirror_SyncVarHookDelegate_characterColour = HookSetColor;
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
				writer.WriteVarInt(characterNumber);
				writer.WriteString(playerName);
				writer.WriteColor(characterColour);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(characterNumber);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteString(playerName);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteColor(characterColour);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref characterNumber, null, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref playerName, _Mirror_SyncVarHookDelegate_playerName, reader.ReadString());
				GeneratedSyncVarDeserialize(ref characterColour, _Mirror_SyncVarHookDelegate_characterColour, reader.ReadColor());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref characterNumber, null, reader.ReadVarInt());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref playerName, _Mirror_SyncVarHookDelegate_playerName, reader.ReadString());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref characterColour, _Mirror_SyncVarHookDelegate_characterColour, reader.ReadColor());
			}
		}
	}
}
