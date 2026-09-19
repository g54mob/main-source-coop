#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Fusion
{
	public sealed class TraceLogStream : IDisposable
	{
		public readonly LogStream InfoStream;

		public readonly LogStream WarnStream;

		public readonly LogStream ErrorStream;

		public TraceLogStream(LogStream innerStream, LogStream warnStream, LogStream errorStream)
		{
			InfoStream = innerStream ?? throw new ArgumentNullException("innerStream");
			WarnStream = warnStream ?? throw new ArgumentNullException("warnStream");
			ErrorStream = errorStream ?? throw new ArgumentNullException("errorStream");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Log(ILogSource source, string message)
		{
			InfoStream.Log(source, message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Log(string message)
		{
			InfoStream.Log(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Info(ILogSource source, string message)
		{
			InfoStream.Log(source, message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Info(string message)
		{
			InfoStream.Log(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Error(ILogSource source, string message)
		{
			ErrorStream.Log(source, message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Error(string message)
		{
			ErrorStream.Log(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Error(Exception message)
		{
			ErrorStream.Log(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Exception(Exception message)
		{
			ErrorStream.Log(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Warn(ILogSource source, string message)
		{
			WarnStream.Log(source, message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[HideInCallstack]
		[Conditional("TRACE")]
		public void Warn(string message)
		{
			WarnStream.Log(message);
		}

		[CanBeNull]
		public TraceLogStream If(bool condition)
		{
			if (!condition)
			{
				return null;
			}
			return this;
		}

		[CanBeNull]
		public TraceLogStream Once(ref bool flag)
		{
			if (!flag)
			{
				flag = true;
				return this;
			}
			return null;
		}

		public void Dispose()
		{
			InfoStream.Dispose();
			WarnStream.Dispose();
			ErrorStream.Dispose();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("DEBUG")]
		internal void Log(object message)
		{
			Log($"{message}");
		}
	}
}
