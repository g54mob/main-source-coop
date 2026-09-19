using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public readonly struct MetaConstant20 : IMetaConstant
	{
		public int ExpectedSize => 20;
	}
}
