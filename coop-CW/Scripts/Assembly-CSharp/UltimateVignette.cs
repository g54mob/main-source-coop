using UnityEngine;
using UnityEngine.Rendering;

[VolumeComponentMenu("Retro Look Pro/Ultimate Vignette")]
public class UltimateVignette : VolumeComponent, IPostProcessComponent
{
	public BoolParameter enable = new BoolParameter(value: false);

	public VignetteModeParameter vignetteShape = new VignetteModeParameter();

	[Tooltip(".")]
	public Vector2Parameter center = new Vector2Parameter(new Vector2(0.5f, 0.5f));

	[Range(0f, 100f)]
	[Tooltip(".")]
	public ClampedFloatParameter vignetteAmount = new ClampedFloatParameter(50f, 0f, 100f);

	[Range(-1f, -100f)]
	[Tooltip(".")]
	public ClampedFloatParameter vignetteFineTune = new ClampedFloatParameter(-10f, -100f, -10f);

	[Range(0f, 100f)]
	[Tooltip("Scanlines width.")]
	public ClampedFloatParameter edgeSoftness = new ClampedFloatParameter(1.5f, 0f, 100f);

	[Range(200f, 0f)]
	[Tooltip("Horizontal/Vertical scanlines.")]
	public ClampedFloatParameter edgeBlend = new ClampedFloatParameter(0f, 0f, 200f);

	[Range(0f, 200f)]
	[Tooltip(".")]
	public ClampedFloatParameter innerColorAlpha = new ClampedFloatParameter(0f, 0f, 200f);

	public ColorParameter innerColor = new ColorParameter(default(Color));

	[Space]
	[Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
	public BoolParameter GlobalPostProcessingSettings = new BoolParameter(value: false);

	public bool IsActive()
	{
		return (bool)enable;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
