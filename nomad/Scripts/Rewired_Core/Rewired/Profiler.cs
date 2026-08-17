using System.Diagnostics;
using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false)]
	internal static class Profiler
	{
		private const string KqSSwwHlkOMcsRDsWcMRbEswCiPh = "ENABLE_PROFILER must be set in Rewired Core to use the profiler.";

		public static bool enableBinaryLog
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return false;
			}
			set
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			}
		}

		public static bool enabled
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return false;
			}
			set
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			}
		}

		public static string logFile
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return string.Empty;
			}
			set
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			}
		}

		public static bool supported
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return false;
			}
		}

		public static uint usedHeapSize
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return 0u;
			}
		}

		public static long usedHeapSizeLong
		{
			get
			{
				ILkSpzUuapZdKMjtRBsJyHqUwyXS();
				return 0L;
			}
		}

		private static void ILkSpzUuapZdKMjtRBsJyHqUwyXS()
		{
			Logger.Log("ENABLE_PROFILER must be set in Rewired Core to use the profiler.");
		}

		[Conditional("ENABLE_PROFILER")]
		public static void AddFramesFromFile(string file)
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name)
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name, Object targetObject)
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
		}

		[Conditional("ENABLE_PROFILER")]
		public static void EndSample()
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
		}

		public static uint GetMonoHeapSize()
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			return 0u;
		}

		public static long GetMonoHeapSizeLong()
		{
			return 0L;
		}

		public static uint GetMonoUsedSize()
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			return 0u;
		}

		public static long GetMonoUsedSizeLong()
		{
			return 0L;
		}

		public static int GetRuntimeMemorySize(Object o)
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
			return 0;
		}

		public static long GetRuntimeMemorySizeLong(Object o)
		{
			return 0L;
		}

		public static uint GetTotalAllocatedMemory()
		{
			ILkSpzUuapZdKMjtRBsJyHqUwyXS();
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
