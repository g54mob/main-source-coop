using UnityEngine;
using UnityEngine.Profiling;

namespace Tayx.Graphy.Ram
{
	public class G_RamMonitor : MonoBehaviour
	{
		public float AllocatedRam { get; private set; }

		public float ReservedRam { get; private set; }

		public float MonoRam { get; private set; }

		private void Update()
		{
			AllocatedRam = (float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
			ReservedRam = (float)Profiler.GetTotalReservedMemoryLong() / 1048576f;
			MonoRam = (float)Profiler.GetMonoUsedSizeLong() / 1048576f;
		}
	}
}
