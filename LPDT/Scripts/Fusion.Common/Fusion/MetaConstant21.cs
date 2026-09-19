using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 21)]
	public readonly struct MetaConstant21 : IMetaConstant
	{
		public int ExpectedSize => 21;
	}
}
