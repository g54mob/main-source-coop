using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion.Sockets;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	internal struct SimulationMessageHeader
	{
		[FieldOffset(0)]
		public SimulationMessageHeaderFlags Flags;

		[FieldOffset(4)]
		public int PayloadNumBytes;

		[FieldOffset(8)]
		public PlayerRef SourcePlayer;

		[FieldOffset(12)]
		public PlayerRef TargetPlayer;

		[FieldOffset(16)]
		public uint MessageType;

		[FieldOffset(20)]
		public NetworkBehaviourId TargetObject;

		[FieldOffset(16)]
		public ReliableKey ReliableKey;

		public const int SIZE = 32;

		public const int WORD_COUNT = 8;

		internal const int BYTE_OF_FLAGS = 0;

		internal const int BYTE_COUNT_OF_FLAGS = 1;

		internal const int BYTE_OF_PAYLOAD_NUM_BYTES = 4;

		internal const int BYTE_COUNT_OF_PAYLOAD_NUM_BYTES = 4;

		internal const int BYTE_OF_SOURCE_PLAYER = 8;

		internal const int BYTE_COUNT_OF_SOURCE_PLAYER = 4;

		internal const int BYTE_OF_TARGET_PLAYER = 12;

		internal const int BYTE_COUNT_OF_TARGET_PLAYER = 4;

		internal const int BYTE_OF_MESSAGE_TYPE = 16;

		internal const int BYTE_COUNT_OF_MESSAGE_TYPE = 4;

		internal const int BYTE_OF_TARGET_OBJECT = 20;

		internal const int BYTE_COUNT_OF_TARGET_OBJECT = 8;

		internal const int BYTE_OF_RELIABLE_KEY = 16;

		internal const int BYTE_COUNT_OF_RELIABLE_KEY = 16;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public override readonly string ToString()
		{
			return "[SimulationMessageHeader " + string.Format("{0}: {1}, ", "Flags", Flags) + string.Format("{0}: {1}, ", "MessageType", MessageType) + string.Format("{0}: {1}, ", "SourcePlayer", SourcePlayer) + string.Format("{0}: {1}, ", "TargetPlayer", TargetPlayer) + string.Format("{0}: {1}, ", "TargetObject", TargetObject) + string.Format("{0}: {1}]", "PayloadNumBytes", PayloadNumBytes);
		}

		public static bool TryRead(ReadBuffer buffer, out SimulationMessageHeader header)
		{
			header.Flags = (SimulationMessageHeaderFlags)buffer.Byte();
			header.PayloadNumBytes = buffer.IntVar();
			if (!PlayerRef.TryRead(buffer, out header.SourcePlayer))
			{
				goto IL_013a;
			}
			if (header.Flags.Has(SimulationMessageHeaderFlags.HasTargetPlayer))
			{
				if (!PlayerRef.TryRead(buffer, out header.TargetPlayer))
				{
					goto IL_013a;
				}
			}
			else
			{
				header.TargetPlayer = PlayerRef.Invalid;
			}
			if (header.Flags.Has(SimulationMessageHeaderFlags.ReliableData))
			{
				Unsafe.SkipInit<uint>(out header.MessageType);
				Unsafe.SkipInit<NetworkBehaviourId>(out header.TargetObject);
				int key = buffer.Int();
				int key2 = buffer.Int();
				int key3 = buffer.Int();
				int key4 = buffer.Int();
				header.ReliableKey = ReliableKey.FromInts(key, key2, key3, key4);
			}
			else
			{
				Unsafe.SkipInit<ReliableKey>(out header.ReliableKey);
				header.MessageType = buffer.UInt();
				if (header.Flags.Has(SimulationMessageHeaderFlags.HasTargetObject))
				{
					header.TargetObject.Object.Raw = buffer.UIntVar();
					header.TargetObject.Behaviour = buffer.IntVar();
				}
				else
				{
					header.TargetObject = default(NetworkBehaviourId);
				}
			}
			return true;
			IL_013a:
			header = default(SimulationMessageHeader);
			return false;
		}

		public static void Write(WriteBuffer buffer, in SimulationMessageHeader header)
		{
			buffer.Byte((byte)header.Flags);
			buffer.IntVar(header.PayloadNumBytes);
			PlayerRef.Write(buffer, header.SourcePlayer);
			if (header.Flags.Has(SimulationMessageHeaderFlags.HasTargetPlayer))
			{
				PlayerRef.Write(buffer, header.TargetPlayer);
			}
			if (header.Flags.Has(SimulationMessageHeaderFlags.ReliableData))
			{
				header.ReliableKey.GetInts(out var key, out var key2, out var key3, out var key4);
				buffer.Int(key);
				buffer.Int(key2);
				buffer.Int(key3);
				buffer.Int(key4);
			}
			else
			{
				buffer.UInt(header.MessageType);
				if (header.Flags.Has(SimulationMessageHeaderFlags.HasTargetObject))
				{
					buffer.UIntVar(header.TargetObject.Object.Raw);
					buffer.IntVar(header.TargetObject.Behaviour);
				}
			}
		}
	}
}
