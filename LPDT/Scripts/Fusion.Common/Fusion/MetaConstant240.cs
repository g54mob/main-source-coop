using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 240)]
	public readonly struct MetaConstant240 : IMetaConstant
	{
		public int ExpectedSize => 240;
	}
}
