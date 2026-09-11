using UnityEngine;
using UnityEngine.Rendering;

[VolumeComponentMenu("Retro Look Pro/Phosphor")]
public class Phosphor : VolumeComponent, IPostProcessComponent
{
	public BoolParameter enable = new BoolParameter(value: false);

	public ClampedFloatParameter width = new ClampedFloatParameter(0.4f, 0f, 20f);

	public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0f, 1f);

	[Space]
	[Tooltip("Mask texture")]
	public TextureParameter mask = new TextureParameter(null);

	public maskChannelModeParameter maskChannel = new maskChannelModeParameter();

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
