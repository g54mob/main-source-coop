using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 75)]
	public readonly struct MetaConstant75 : IMetaConstant
	{
		public int ExpectedSize => 75;
	}
}
