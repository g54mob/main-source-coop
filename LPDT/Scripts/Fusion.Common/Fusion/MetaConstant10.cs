using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 10)]
	public readonly struct MetaConstant10 : IMetaConstant
	{
		public int ExpectedSize => 10;
	}
}
