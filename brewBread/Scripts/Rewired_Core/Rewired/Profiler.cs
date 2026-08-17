using System.Diagnostics;
using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false)]
	internal static class Profiler
	{
		private const string WeofNNbRCNPRQEQRKpMkPGvdZexG = "ENABLE_PROFILER must be set in Rewired Core to use the profiler.";

		public static bool enableBinaryLog
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return false;
			}
			set
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			}
		}

		public static bool enabled
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return false;
			}
			set
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			}
		}

		public static string logFile
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return string.Empty;
			}
			set
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			}
		}

		public static bool supported
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return false;
			}
		}

		public static uint usedHeapSize
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return 0u;
			}
		}

		public static long usedHeapSizeLong
		{
			get
			{
				QclcJhmaxAdzEHyvWIOQSOyNOKbg();
				return 0L;
			}
		}

		private static void QclcJhmaxAdzEHyvWIOQSOyNOKbg()
		{
			Logger.Log("ENABLE_PROFILER must be set in Rewired Core to use the profiler.");
		}

		[Conditional("ENABLE_PROFILER")]
		public static void AddFramesFromFile(string file)
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name)
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name, Object targetObject)
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void EndSample()
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
		}

		public static uint GetMonoHeapSize()
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			return 0u;
		}

		public static long GetMonoHeapSizeLong()
		{
			return 0L;
		}

		public static uint GetMonoUsedSize()
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			return 0u;
		}

		public static long GetMonoUsedSizeLong()
		{
			return 0L;
		}

		public static int GetRuntimeMemorySize(Object o)
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			return 0;
		}

		public static long GetRuntimeMemorySizeLong(Object o)
		{
			return 0L;
		}

		public static uint GetTotalAllocatedMemory()
		{
			QclcJhmaxAdzEHyvWIOQSOyNOKbg();
			return 0u;
		}

		public static long GetTotalAllocatedMemoryLong()
		{
			return 0L;
		}

		public static uint GetTotalReservedMemory()
		{
			return 0u;
		}

		public static long GetTotalReservedMemoryLong()
		{
			return 0L;
		}

		public static uint GetTotalUnusedReservedMemory()
		{
			return 0u;
		}

		public static long GetTotalUnusedReservedMemoryLong()
		{
			return 0L;
		}
	}
}
