using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fusion
{
	internal struct NetworkObjectPacketData
	{
		public NetworkId Id;

		public Tick ResetTick;

		public NetworkObjectPacketFlags Flags;

		[CompilerGenerated]
		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("NetworkObjectPacketData");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private readonly bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Id = ");
			builder.Append(Id.ToString());
			builder.Append(", ResetTick = ");
			builder.Append(ResetTick.ToString());
			builder.Append(", Flags = ");
			builder.Append(Flags.ToString());
			return true;
		}

		[CompilerGenerated]
		public static bool operator !=(NetworkObjectPacketData left, NetworkObjectPacketData right)
		{
			return !(left == right);
		}

		[CompilerGenerated]
		public static bool operator ==(NetworkObjectPacketData left, NetworkObjectPacketData right)
		{
			return left.Equals(right);
		}

		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return (EqualityComparer<NetworkId>.Default.GetHashCode(Id) * -1521134295 + EqualityComparer<Tick>.Default.GetHashCode(ResetTick)) * -1521134295 + EqualityComparer<NetworkObjectPacketFlags>.Default.GetHashCode(Flags);
		}

		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is NetworkObjectPacketData && Equals((NetworkObjectPacketData)obj);
		}

		[CompilerGenerated]
		public readonly bool Equals(NetworkObjectPacketData other)
		{
			return EqualityComparer<NetworkId>.Default.Equals(Id, other.Id) && EqualityComparer<Tick>.Default.Equals(ResetTick, other.ResetTick) && EqualityComparer<NetworkObjectPacketFlags>.Default.Equals(Flags, other.Flags);
		}
	}
}
