using System;

namespace EvilCore.GraphicsQuality
{
	public interface IGraphicsQualityManager
	{
		GraphicsQualityLevel CurrentLevel { get; }

		event Action<GraphicsQualityLevel> OnQualityChanged;

		void SetQuality(GraphicsQualityLevel level);

		void SetMotionBlurEnabled(bool enabled);

		void ForceApply();
	}
}
