using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 5)]
	public readonly struct MetaConstant5 : IMetaConstant
	{
		public int ExpectedSize => 5;
	}
}
