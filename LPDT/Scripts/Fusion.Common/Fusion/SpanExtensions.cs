using System;

namespace Fusion
{
	internal static class SpanExtensions
	{
		internal static void RepeatingCopyTo(this ReadOnlySpan<byte> src, Span<byte> dst)
		{
			if (!src.IsEmpty)
			{
				while (dst.Length >= src.Length)
				{
					src.CopyTo(dst);
					int length = src.Length;
					dst = dst.Slice(length, dst.Length - length);
				}
				if (dst.Length > 0)
				{
					src.Slice(0, dst.Length).CopyTo(dst);
				}
			}
		}

		internal static bool RepeatingSequenceEqualTo(this ReadOnlySpan<byte> span, ReadOnlySpan<byte> other)
		{
			while (span.Length >= other.Length)
			{
				if (!span.Slice(0, other.Length).SequenceEqual(other))
				{
					return false;
				}
				int length = other.Length;
				span = span.Slice(length, span.Length - length);
			}
			if (span.Length > 0 && !span.SequenceEqual(other.Slice(0, span.Length)))
			{
				return false;
			}
			return true;
		}
	}
}
