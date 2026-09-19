using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public readonly struct MetaConstant36 : IMetaConstant
	{
		public int ExpectedSize => 36;
	}
}
