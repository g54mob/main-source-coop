using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 127)]
	public readonly struct MetaConstant127 : IMetaConstant
	{
		public int ExpectedSize => 127;
	}
}
