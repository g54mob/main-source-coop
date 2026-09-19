using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 65)]
	public readonly struct MetaConstant65 : IMetaConstant
	{
		public int ExpectedSize => 65;
	}
}
