using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 7)]
	public readonly struct MetaConstant7 : IMetaConstant
	{
		public int ExpectedSize => 7;
	}
}
