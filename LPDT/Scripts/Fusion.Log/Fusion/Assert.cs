using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Fusion
{
	public static class Assert
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:null=>halt")]
		[HideInCallstack]
		public static void Check<T>(ReadOnlySpan<T> condition, [CallerArgumentExpression("condition")] string error = null)
		{
			if (condition.IsEmpty)
			{
				throw new AssertException(error);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[DoesNotReturn]
		[HideInCallstack]
		public static void Fail()
		{
			throw new AssertException();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[DoesNotReturn]
		[HideInCallstack]
		public static void Fail(string error)
		{
			throw new AssertException(error);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[DoesNotReturn]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Fail(string format, params object[] args)
		{
			throw new AssertException(string.Format(format, args));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:null=>halt")]
		[HideInCallstack]
		public static void Check(object condition, [CallerArgumentExpression("condition")] string error = null)
		{
			if (condition == null)
			{
				throw new AssertException(error);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:null=>halt")]
		[HideInCallstack]
		public unsafe static void Check(void* condition, [CallerArgumentExpression("condition")] string error = null)
		{
			if (condition == null)
			{
				throw new AssertException(error);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression("condition")] string error = null)
		{
			if (!condition)
			{
				throw new AssertException(error);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Check<T0>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Check<T0, T1>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Check<T0, T1, T2>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1, T2 arg2)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1, arg2));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Check<T0, T1, T2, T3>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1, arg2, arg3));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check<T0>([DoesNotReturnIf(false)] bool condition, T0 arg0)
		{
			if (!condition)
			{
				throw new AssertException($"{arg0}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check<T0, T1>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check<T0, T1, T2>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1, T2 arg2)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1} arg2:{arg2}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check<T0, T1, T2, T3>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1} arg2:{arg2} arg3:{arg3}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[AssertionMethod]
		[ContractAnnotation("condition:false=>halt")]
		[HideInCallstack]
		public static void Check<T0, T1, T2, T3, T4>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1} arg2:{arg2} arg3:{arg3} arg4:{arg4}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Obsolete("Use overload with a message instead")]
		[DoesNotReturn]
		[HideInCallstack]
		public static void AlwaysFail()
		{
			throw new AssertException();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DoesNotReturn]
		[HideInCallstack]
		public static void AlwaysFail(string error)
		{
			throw new AssertException(error);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DoesNotReturn]
		[HideInCallstack]
		public static void AlwaysFail(object error)
		{
			throw new AssertException(error?.ToString());
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DoesNotReturn]
		[HideInCallstack]
		public static void AlwaysFail<T>(T error) where T : struct
		{
			throw new AssertException(error.ToString());
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[HideInCallstack]
		public static void Always([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression("condition")] string error = null)
		{
			if (!condition)
			{
				throw new AssertException(error);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Always<T0>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Always<T0, T1>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Always<T0, T1, T2>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1, T2 arg2)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1, arg2));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Always<T0, T1, T2, T3>([DoesNotReturnIf(false)] bool condition, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (!condition)
			{
				throw new AssertException(string.Format(format, arg0, arg1, arg2, arg3));
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[HideInCallstack]
		public static void Always<T0>([DoesNotReturnIf(false)] bool condition, T0 arg0)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[HideInCallstack]
		public static void Always<T0, T1>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[HideInCallstack]
		public static void Always<T0, T1, T2>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1, T2 arg2)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1} arg2:{arg2}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[ContractAnnotation("condition:false=>halt")]
		[AssertionMethod]
		[HideInCallstack]
		public static void Always<T0, T1, T2, T3>([DoesNotReturnIf(false)] bool condition, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (!condition)
			{
				throw new AssertException($"arg0:{arg0} arg1:{arg1} arg2:{arg2} arg3:{arg3}");
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[HideInCallstack]
		public static void Log(bool condition, string message)
		{
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Log<T0>(bool condition, string format, T0 arg0)
		{
			if (!condition)
			{
				string.Format(format, arg0);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Log<T0, T1>(bool condition, string format, T0 arg0, T1 arg1)
		{
			if (!condition)
			{
				string.Format(format, arg0, arg1);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Log<T0, T1, T2>(bool condition, string format, T0 arg0, T1 arg1, T2 arg2)
		{
			if (!condition)
			{
				string.Format(format, arg0, arg1, arg2);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[Conditional("DEBUG")]
		[StringFormatMethod("format")]
		[HideInCallstack]
		public static void Log<T0, T1, T2, T3>(bool condition, string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			if (!condition)
			{
				string.Format(format, arg0, arg1, arg2, arg3);
			}
		}
	}
}
