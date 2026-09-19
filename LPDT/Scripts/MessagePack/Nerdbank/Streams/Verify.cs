using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Nerdbank.Streams
{
	internal static class Verify
	{
		[DebuggerStepThrough]
		internal static void Operation([DoesNotReturnIf(false)] bool condition, string message)
		{
			if (!condition)
			{
				throw new InvalidOperationException(message);
			}
		}
	}
}
