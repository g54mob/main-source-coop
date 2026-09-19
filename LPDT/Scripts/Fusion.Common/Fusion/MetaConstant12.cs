using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public readonly struct MetaConstant12 : IMetaConstant
	{
		public int ExpectedSize => 12;
	}
}
