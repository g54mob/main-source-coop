using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 48)]
	public readonly struct MetaConstant48 : IMetaConstant
	{
		public int ExpectedSize => 48;
	}
}
