using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 160)]
	public readonly struct MetaConstant160 : IMetaConstant
	{
		public int ExpectedSize => 160;
	}
}
