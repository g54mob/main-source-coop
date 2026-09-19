using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 25)]
	public readonly struct MetaConstant25 : IMetaConstant
	{
		public int ExpectedSize => 25;
	}
}
