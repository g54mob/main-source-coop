using System.Runtime.InteropServices;
using EvilCore.EvilSave;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using UnityEngine;

namespace NomadDrive.Features.Furnitures
{
	public class Picture : HeldItem, INetworkSaveable
	{
		[SerializeField]
		private Material[] _pictureMaterials;

		private MeshRenderer _meshRenderer;

		[SyncVar]
		private byte _selectedPictureIndex;

		public string ContributorKey => "picture";

		public byte Network_selectedPictureIndex
		{
			get
			{
				return _selectedPictureIndex;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _selectedPictureIndex, 512uL, null);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_meshRenderer = GetComponentInChildren<MeshRenderer>();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			Network_selectedPictureIndex = (byte)Random.Range(0, _pictureMaterials.Length);
			SetPicture(_selectedPictureIndex);
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SetPicture(_selectedPictureIndex);
		}

		private void SetPicture(byte index)
		{
			Network_selectedPictureIndex = index;
			_meshRenderer.material = _pictureMaterials[index];
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_selectedPictureIndex);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte b = reader.ReadByte();
			if (NetworkServer.active && _pictureMaterials != null && b < _pictureMaterials.Length)
			{
				SetPicture(b);
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
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
				NetworkWriterExtensions.WriteByte(writer, _selectedPictureIndex);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _selectedPictureIndex);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _selectedPictureIndex, null, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _selectedPictureIndex, null, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
