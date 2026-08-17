using UnityEngine;

namespace EvilCore.GraphicsQuality
{
	[CreateAssetMenu(fileName = "HardwareQualityConfig", menuName = "EvilCore/Hardware Quality Config")]
	public class HardwareQualityConfig : ScriptableObject
	{
		[Header("High tier minimum requirements")]
		[Tooltip("Dedicated video memory (MB) required for the High tier.")]
		[SerializeField]
		private int minVramMbHigh = 6144;

		[Tooltip("System memory (MB) required for the High tier.")]
		[SerializeField]
		private int minRamMbHigh = 16384;

		[Tooltip("Logical CPU cores required for the High tier.")]
		[SerializeField]
		private int minCoresHigh = 8;

		[Header("Medium tier minimum requirements")]
		[Tooltip("Dedicated video memory (MB) required for the Medium tier.")]
		[SerializeField]
		private int minVramMbMedium = 4096;

		[Tooltip("System memory (MB) required for the Medium tier.")]
		[SerializeField]
		private int minRamMbMedium = 12288;

		[Tooltip("Logical CPU cores required for the Medium tier.")]
		[SerializeField]
		private int minCoresMedium = 6;

		[Header("Integrated GPU")]
		[Tooltip("If the GPU name contains any keyword below, force the Low tier regardless of reported VRAM/RAM.")]
		[SerializeField]
		private bool forceLowOnIntegratedGpu = true;

		[Tooltip("Case-insensitive substrings matched against SystemInfo.graphicsDeviceName. Keep this list small and safe.")]
		[SerializeField]
		private string[] integratedGpuNameKeywords = new string[5] { "Intel", "UHD", "HD Graphics", "Iris", "Microsoft Basic" };

		[Header("Fallback")]
		[Tooltip("Used when SystemInfo cannot be trusted (e.g. reported VRAM <= 0).")]
		[SerializeField]
		private GraphicsQualityLevel fallbackLevel = GraphicsQualityLevel.Medium;

		public int MinVramMbHigh => minVramMbHigh;

		public int MinRamMbHigh => minRamMbHigh;

		public int MinCoresHigh => minCoresHigh;

		public int MinVramMbMedium => minVramMbMedium;

		public int MinRamMbMedium => minRamMbMedium;

		public int MinCoresMedium => minCoresMedium;

		public bool ForceLowOnIntegratedGpu => forceLowOnIntegratedGpu;

		public string[] IntegratedGpuNameKeywords => integratedGpuNameKeywords;

		public GraphicsQualityLevel FallbackLevel => fallbackLevel;
	}
}
