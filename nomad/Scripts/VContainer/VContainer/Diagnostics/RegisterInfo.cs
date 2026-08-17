using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;

namespace VContainer.Diagnostics
{
	public sealed class RegisterInfo
	{
		private static bool displayFileNames = true;

		private static int idSeed;

		private StackFrame headLineStackFrame;

		internal string formattedStackTrace;

		public int Id { get; }

		public RegistrationBuilder RegistrationBuilder { get; }

		public StackTrace StackTrace { get; }

		public RegisterInfo(RegistrationBuilder registrationBuilder)
		{
			Id = Interlocked.Increment(ref idSeed);
			RegistrationBuilder = registrationBuilder;
			StackTrace = new StackTrace(fNeedFileInfo: true);
			headLineStackFrame = GetHeadlineFrame(StackTrace);
		}

		public string GetFilename()
		{
			if (headLineStackFrame != null && displayFileNames && headLineStackFrame.GetILOffset() != -1)
			{
				try
				{
					return headLineStackFrame.GetFileName();
				}
				catch (NotSupportedException)
				{
					displayFileNames = false;
				}
				catch (SecurityException)
				{
					displayFileNames = false;
				}
			}
			return null;
		}

		public int GetFileLineNumber()
		{
			if (headLineStackFrame != null && displayFileNames && headLineStackFrame.GetILOffset() != -1)
			{
				try
				{
					return headLineStackFrame.GetFileLineNumber();
				}
				catch (NotSupportedException)
				{
					displayFileNames = false;
				}
				catch (SecurityException)
				{
					displayFileNames = false;
				}
			}
			return -1;
		}

		public string GetScriptAssetPath()
		{
			string filename = GetFilename();
			if (filename == null)
			{
				return "";
			}
			int num = filename.LastIndexOf("Assets/");
			if (num <= 0)
			{
				return "";
			}
			return filename.Substring(num);
		}

		public string GetHeadline()
		{
			if (headLineStackFrame == null)
			{
				return "";
			}
			MethodBase method = headLineStackFrame.GetMethod();
			string filename = GetFilename();
			if (filename != null)
			{
				int fileLineNumber = GetFileLineNumber();
				return $"{method.DeclaringType?.FullName}.{method.Name} (at {Path.GetFileName(filename)}:{fileLineNumber})";
			}
			int iLOffset = headLineStackFrame.GetILOffset();
			if (iLOffset != -1)
			{
				return $"{method.DeclaringType?.FullName}.{method.Name}(offset: {iLOffset})";
			}
			return method.DeclaringType?.FullName + "." + method.Name;
		}

		private StackFrame GetHeadlineFrame(StackTrace stackTrace)
		{
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				if (frame != null)
				{
					MethodBase method = frame.GetMethod();
					if (!(method == null) && !(method.DeclaringType == null) && (method.DeclaringType.Namespace == null || !method.DeclaringType.Namespace.StartsWith("VContainer")))
					{
						return frame;
					}
				}
			}
			if (stackTrace.FrameCount <= 0)
			{
				return null;
			}
			return stackTrace.GetFrame(0);
		}
	}
}
