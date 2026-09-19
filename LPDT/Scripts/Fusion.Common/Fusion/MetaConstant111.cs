using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 111)]
	public readonly struct MetaConstant111 : IMetaConstant
	{
		public int ExpectedSize => 111;
	}
}
