using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public readonly struct MetaConstant8 : IMetaConstant
	{
		public int ExpectedSize => 8;
	}
}
