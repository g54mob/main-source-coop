using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 40)]
	public readonly struct MetaConstant40 : IMetaConstant
	{
		public int ExpectedSize => 40;
	}
}
