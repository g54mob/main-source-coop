using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public readonly struct MetaConstant16 : IMetaConstant
	{
		public int ExpectedSize => 16;
	}
}
