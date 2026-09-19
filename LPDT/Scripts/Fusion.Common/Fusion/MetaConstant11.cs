using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 11)]
	public readonly struct MetaConstant11 : IMetaConstant
	{
		public int ExpectedSize => 11;
	}
}
