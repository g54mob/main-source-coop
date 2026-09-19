using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 2)]
	public readonly struct MetaConstant2 : IMetaConstant
	{
		public int ExpectedSize => 2;
	}
}
