using System;

namespace Fusion
{
	public interface IReadOnlySpanAssignable
	{
		void Set(ReadOnlySpan<uint> values);
	}
}
