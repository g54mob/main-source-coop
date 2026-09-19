using System.Runtime.InteropServices;
using UnityEngine;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 56)]
	[NetworkStructWeaved(14)]
	public struct NetworkTRSPData : INetworkStruct
	{
		public const int WORDS = 14;

		public const int POSITION_OFFSET = 2;

		[FieldOffset(0)]
		public NetworkBehaviourId Parent;

		internal const int POSITION_X = 2;

		internal const int POSITION_Y = 3;

		internal const int POSITION_Z = 4;

		internal const int ROTATION_X = 5;

		internal const int ROTATION_Y = 6;

		internal const int ROTATION_Z = 7;

		internal const int ROTATION_W = 8;

		[FieldOffset(8)]
		public Vector3 Position;

		[FieldOffset(20)]
		public Quaternion Rotation;

		[FieldOffset(36)]
		public Vector3Compressed Scale;

		[FieldOffset(48)]
		public int TeleportKey;

		[FieldOffset(52)]
		public NetworkId AreaOfInterestOverride;

		public const int SIZE = 56;

		public const int WORD_COUNT = 14;

		internal const int BYTE_OF_PARENT = 0;

		internal const int BYTE_COUNT_OF_PARENT = 8;

		internal const int BYTE_OF_POSITION = 8;

		internal const int BYTE_COUNT_OF_POSITION = 12;

		internal const int BYTE_OF_ROTATION = 20;

		internal const int BYTE_COUNT_OF_ROTATION = 16;

		internal const int BYTE_OF_SCALE = 36;

		internal const int BYTE_COUNT_OF_SCALE = 12;

		internal const int BYTE_OF_TELEPORT_KEY = 48;

		internal const int BYTE_COUNT_OF_TELEPORT_KEY = 4;

		internal const int BYTE_OF_AREA_OF_INTEREST_OVERRIDE = 52;

		internal const int BYTE_COUNT_OF_AREA_OF_INTEREST_OVERRIDE = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static NetworkBehaviourId NonNetworkedParent
		{
			get
			{
				NetworkBehaviourId result = default(NetworkBehaviourId);
				result.Object = default(NetworkId);
				result.Behaviour = 1;
				return result;
			}
		}
	}
}
