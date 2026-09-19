using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 101)]
	public readonly struct MetaConstant101 : IMetaConstant
	{
		public int ExpectedSize => 101;
	}
}
