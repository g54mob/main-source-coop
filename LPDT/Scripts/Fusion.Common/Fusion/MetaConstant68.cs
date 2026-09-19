using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 68)]
	public readonly struct MetaConstant68 : IMetaConstant
	{
		public int ExpectedSize => 68;
	}
}
