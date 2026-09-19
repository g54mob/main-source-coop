using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 123)]
	public readonly struct MetaConstant123 : IMetaConstant
	{
		public int ExpectedSize => 123;
	}
}
