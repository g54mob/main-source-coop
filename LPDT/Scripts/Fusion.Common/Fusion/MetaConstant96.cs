using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 96)]
	public readonly struct MetaConstant96 : IMetaConstant
	{
		public int ExpectedSize => 96;
	}
}
