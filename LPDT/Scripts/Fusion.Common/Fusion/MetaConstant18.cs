using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 18)]
	public readonly struct MetaConstant18 : IMetaConstant
	{
		public int ExpectedSize => 18;
	}
}
