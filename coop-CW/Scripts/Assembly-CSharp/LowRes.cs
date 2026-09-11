using UnityEngine;
using UnityEngine.Rendering;

[VolumeComponentMenu("Retro Look Pro/LowRes")]
public class LowRes : VolumeComponent, IPostProcessComponent
{
	public BoolParameter enable = new BoolParameter(value: false);

	public IntParameter height = new IntParameter(180);

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
