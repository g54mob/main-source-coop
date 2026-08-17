using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	public class ObjectRecord
	{
		public string Guid = string.Empty;

		public string AddressableGuid = string.Empty;

		public int SpawnSeed;

		public Vector3 Position;

		public Quaternion Rotation = Quaternion.identity;

		public SaveParentLink ParentLink = SaveParentLink.None;

		public Dictionary<string, byte[]> Contributors = new Dictionary<string, byte[]>();
	}
}
