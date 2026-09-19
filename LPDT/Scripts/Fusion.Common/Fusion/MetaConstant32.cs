using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	public readonly struct MetaConstant32 : IMetaConstant
	{
		public int ExpectedSize => 32;
	}
}
