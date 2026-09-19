using System;
using System.Diagnostics;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	[Conditional("FUSION_ANALYZER")]
	public sealed class StructExplicitAttribute : Attribute
	{
		public int Size { get; }

		public int Pack { get; set; }

		public bool AllowOversizing { get; set; }

		public bool EmitWordOffsets { get; set; }

		public string WordCountName { get; set; }

		public StructExplicitAttribute(int Size)
		{
			this.Size = Size;
			WordCountName = "WORD_COUNT";
			base._002Ector();
		}
	}
}
