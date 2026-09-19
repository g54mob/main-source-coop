using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public readonly struct MetaConstant24 : IMetaConstant
	{
		public int ExpectedSize => 24;
	}
}
