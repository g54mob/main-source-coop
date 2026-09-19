using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 180)]
	public readonly struct MetaConstant180 : IMetaConstant
	{
		public int ExpectedSize => 180;
	}
}
