using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 192)]
	public readonly struct MetaConstant192 : IMetaConstant
	{
		public int ExpectedSize => 192;
	}
}
