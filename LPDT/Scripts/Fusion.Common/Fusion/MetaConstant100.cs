using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 100)]
	public readonly struct MetaConstant100 : IMetaConstant
	{
		public int ExpectedSize => 100;
	}
}
