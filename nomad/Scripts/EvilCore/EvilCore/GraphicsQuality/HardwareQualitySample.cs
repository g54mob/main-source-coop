using UnityEngine.Rendering;

namespace EvilCore.GraphicsQuality
{
	public readonly struct HardwareQualitySample
	{
		public readonly int VramMB;

		public readonly int SystemRamMB;

		public readonly int LogicalCores;

		public readonly string GraphicsDeviceName;

		public readonly GraphicsDeviceType GraphicsDeviceType;

		public HardwareQualitySample(int vramMB, int systemRamMB, int logicalCores, string graphicsDeviceName, GraphicsDeviceType graphicsDeviceType)
		{
			VramMB = vramMB;
			SystemRamMB = systemRamMB;
			LogicalCores = logicalCores;
			GraphicsDeviceName = graphicsDeviceName;
			GraphicsDeviceType = graphicsDeviceType;
		}
	}
}
