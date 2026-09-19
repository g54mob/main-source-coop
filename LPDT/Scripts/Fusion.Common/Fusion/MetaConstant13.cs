using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 13)]
	public readonly struct MetaConstant13 : IMetaConstant
	{
		public int ExpectedSize => 13;
	}
}
