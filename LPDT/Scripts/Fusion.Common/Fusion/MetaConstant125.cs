using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 125)]
	public readonly struct MetaConstant125 : IMetaConstant
	{
		public int ExpectedSize => 125;
	}
}
