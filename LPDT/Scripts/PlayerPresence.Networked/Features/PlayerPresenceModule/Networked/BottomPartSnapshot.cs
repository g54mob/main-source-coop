using UnityEngine;

namespace Features.PlayerPresenceModule.Networked
{
	public struct BottomPartSnapshot
	{
		public bool HasValue;

		public int TypeId;

		public Color Color;

		public int SkinId;

		public int TexturePresetId;

		public int MeshPresetId;

		public int UsageCount;
	}
}
