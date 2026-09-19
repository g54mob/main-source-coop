using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 120)]
	public readonly struct MetaConstant120 : IMetaConstant
	{
		public int ExpectedSize => 120;
	}
}
