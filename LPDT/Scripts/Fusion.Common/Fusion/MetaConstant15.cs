using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 15)]
	public readonly struct MetaConstant15 : IMetaConstant
	{
		public int ExpectedSize => 15;
	}
}
