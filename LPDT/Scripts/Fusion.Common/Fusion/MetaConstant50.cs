using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 50)]
	public readonly struct MetaConstant50 : IMetaConstant
	{
		public int ExpectedSize => 50;
	}
}
