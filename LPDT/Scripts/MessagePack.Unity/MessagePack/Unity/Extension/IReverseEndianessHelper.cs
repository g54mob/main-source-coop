using System;

namespace MessagePack.Unity.Extension
{
	public interface IReverseEndianessHelper
	{
		void ReverseEndianess(Span<byte> span);
	}
}
