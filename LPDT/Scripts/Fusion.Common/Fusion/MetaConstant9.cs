using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 9)]
	public readonly struct MetaConstant9 : IMetaConstant
	{
		public int ExpectedSize => 9;
	}
}
