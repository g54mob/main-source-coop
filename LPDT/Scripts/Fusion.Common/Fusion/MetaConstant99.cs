using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 99)]
	public readonly struct MetaConstant99 : IMetaConstant
	{
		public int ExpectedSize => 99;
	}
}
