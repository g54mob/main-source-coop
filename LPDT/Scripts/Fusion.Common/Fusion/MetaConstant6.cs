using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 6)]
	public readonly struct MetaConstant6 : IMetaConstant
	{
		public int ExpectedSize => 6;
	}
}
