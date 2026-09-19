using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 63)]
	public readonly struct MetaConstant63 : IMetaConstant
	{
		public int ExpectedSize => 63;
	}
}
