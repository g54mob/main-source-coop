using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 17)]
	public readonly struct MetaConstant17 : IMetaConstant
	{
		public int ExpectedSize => 17;
	}
}
