using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 140)]
	public readonly struct MetaConstant140 : IMetaConstant
	{
		public int ExpectedSize => 140;
	}
}
