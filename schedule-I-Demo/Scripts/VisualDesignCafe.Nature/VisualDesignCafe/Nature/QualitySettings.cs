using System;
using UnityEngine;

namespace VisualDesignCafe.Nature
{
	public static class QualitySettings
	{
		public class InteractionQuality
		{
			public const int DefaultResolution = 1024;

			public bool Enabled { get; set; } = true;

			public int Resolution { get; set; } = 1024;

			public float Accuracy { get; set; } = 5f;

			public float TrailPopupSpeed { get; set; } = 1f;

			public event Action<InteractionQuality> Changed;

			public void Apply()
			{
				if (Enabled)
				{
					Shader.DisableKeyword("_INTERACTION_OFF");
				}
				else
				{
					Shader.EnableKeyword("_INTERACTION_OFF");
				}
				this.Changed?.Invoke(this);
			}
		}

		public enum MaterialQuality
		{
			High = 0,
			Medium = 1,
			Low = 2
		}

		public class GraphicsQuality
		{
			public MaterialQuality MaterialQuality { get; set; } = MaterialQuality.High;

			public event Action<GraphicsQuality> Changed;

			public void Apply()
			{
				switch (MaterialQuality)
				{
				case MaterialQuality.High:
					Shader.DisableKeyword("MATERIAL_QUALITY_LOW");
					Shader.DisableKeyword("MATERIAL_QUALITY_MEDIUM");
					Shader.EnableKeyword("MATERIAL_QUALITY_HIGH");
					break;
				case MaterialQuality.Medium:
					Shader.DisableKeyword("MATERIAL_QUALITY_LOW");
					Shader.DisableKeyword("MATERIAL_QUALITY_HIGH");
					Shader.EnableKeyword("MATERIAL_QUALITY_MEDIUM");
					break;
				case MaterialQuality.Low:
					Shader.DisableKeyword("MATERIAL_QUALITY_MEDIUM");
					Shader.DisableKeyword("MATERIAL_QUALITY_HIGH");
					Shader.EnableKeyword("MATERIAL_QUALITY_LOW");
					break;
				}
				this.Changed?.Invoke(this);
			}
		}

		public class OverlayQuality
		{
			public const int DefaultResolution = 2048;

			public bool Enabled { get; set; } = true;

			public int Resolution { get; set; } = 2048;

			public float Accuracy { get; set; } = 4f;

			public event Action<OverlayQuality> Changed;

			public void Apply()
			{
				if (Enabled)
				{
					Shader.EnableKeyword("_OVERLAY");
				}
				else
				{
					Shader.DisableKeyword("_OVERLAY");
				}
				this.Changed?.Invoke(this);
			}
		}

		public static readonly InteractionQuality Interaction = new InteractionQuality();

		public static readonly GraphicsQuality Graphics = new GraphicsQuality();

		public static readonly OverlayQuality Overlay = new OverlayQuality();

		public static void Apply()
		{
			Interaction.Apply();
			Graphics.Apply();
			Overlay.Apply();
		}
	}
}
