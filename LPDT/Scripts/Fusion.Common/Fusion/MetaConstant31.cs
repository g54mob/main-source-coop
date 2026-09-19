using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 31)]
	public readonly struct MetaConstant31 : IMetaConstant
	{
		public int ExpectedSize => 31;
	}
}
