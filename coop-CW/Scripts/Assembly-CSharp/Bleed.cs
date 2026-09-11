using UnityEngine;
using UnityEngine.Rendering;

[VolumeComponentMenu("Retro Look Pro/Bleed")]
public class Bleed : VolumeComponent, IPostProcessComponent
{
	public BoolParameter enable = new BoolParameter(value: false);

	[Tooltip("NTSC Bleed modes.")]
	public bleedModeParameter bleedMode = new bleedModeParameter();

	[Tooltip("Bleed Stretch amount.")]
	public FloatParameter bleedAmount = new ClampedFloatParameter(0f, 0f, 15f);

	[Tooltip("Debug bleed curve.")]
	public BoolParameter bleedDebug = new BoolParameter(value: false);

	[Space]
	[Tooltip("Mask texture")]
	public TextureParameter mask = new TextureParameter(null);

	public maskChannelModeParameter maskChannel = new maskChannelModeParameter();

	[Space]
	[Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
	public BoolParameter GlobalPostProcessingSettings = new BoolParameter(value: false);

	public int bleedModeIndex;

	public bool IsActive()
	{
		return (bool)enable;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
