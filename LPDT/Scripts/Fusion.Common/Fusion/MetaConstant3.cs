using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 3)]
	public readonly struct MetaConstant3 : IMetaConstant
	{
		public int ExpectedSize => 3;
	}
}
