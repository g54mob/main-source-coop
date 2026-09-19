using System.Runtime.InteropServices;
using Fusion;

namespace Features.DamageableTrackModule.Scripts
{
	[StructLayout(LayoutKind.Explicit, Size = 208)]
	[NetworkStructWeaved(52)]
	public struct DamageRpcSource : INetworkStruct
	{
		[FieldOffset(0)]
		public byte Category;

		[FieldOffset(4)]
		public byte DamageType;

		[FieldOffset(8)]
		public NetworkString<_16> OwnerTypeName;

		[FieldOffset(76)]
		public NetworkString<_32> PrefabInstanceName;

		public bool IsDefault
		{
			get
			{
				if (Category == 0)
				{
					return DamageType == 0;
				}
				return false;
			}
		}
	}
}
