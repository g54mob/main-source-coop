using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 72)]
	public readonly struct MetaConstant72 : IMetaConstant
	{
		public int ExpectedSize => 72;
	}
}
