using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit)]
	[NetworkStructWeaved(7)]
	public struct NetworkPhysicsData : INetworkStruct
	{
		public const int WORDS = 7;

		[FieldOffset(0)]
		public Vector3Compressed LinearVelocity;

		[FieldOffset(12)]
		public Vector3Compressed AngularVelocity;

		[FieldOffset(24)]
		public int _flagsEncoded;

		public readonly NetworkRigidbodyFlags Flags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (NetworkRigidbodyFlags)(_flagsEncoded & 0xFF);
			}
		}

		public readonly int Constraints => (_flagsEncoded >> 8) & 0xFF;

		public (NetworkRigidbodyFlags flags, int constraints) FlagsAndConstraints
		{
			readonly get
			{
				NetworkRigidbodyFlags item = (NetworkRigidbodyFlags)(_flagsEncoded & 0xFF);
				int item2 = (_flagsEncoded >> 8) & 0xFF;
				return (flags: item, constraints: item2);
			}
			set
			{
				(NetworkRigidbodyFlags flags, int constraints) tuple = value;
				NetworkRigidbodyFlags item = tuple.flags;
				int item2 = tuple.constraints;
				_flagsEncoded = (int)item;
				_flagsEncoded |= item2 << 8;
			}
		}
	}
}
