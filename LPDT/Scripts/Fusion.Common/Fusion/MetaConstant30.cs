using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 30)]
	public readonly struct MetaConstant30 : IMetaConstant
	{
		public int ExpectedSize => 30;
	}
}
