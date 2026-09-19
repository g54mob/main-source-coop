using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 86)]
	public readonly struct MetaConstant86 : IMetaConstant
	{
		public int ExpectedSize => 86;
	}
}
