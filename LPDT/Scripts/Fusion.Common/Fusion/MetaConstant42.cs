using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 42)]
	public readonly struct MetaConstant42 : IMetaConstant
	{
		public int ExpectedSize => 42;
	}
}
