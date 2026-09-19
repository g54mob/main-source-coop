using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 60)]
	public readonly struct MetaConstant60 : IMetaConstant
	{
		public int ExpectedSize => 60;
	}
}
