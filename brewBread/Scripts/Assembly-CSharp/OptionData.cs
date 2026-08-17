using System;

[Serializable]
public struct OptionData
{
	public bool speedRunMode;

	public int Language;

	public float fxVolume;

	public float musicVolume;

	public int resolution;

	public bool fullscreen;

	public int vsync;

	public OptionData(bool speedRunMode, int language, float fxVolume, float musicVolume, int resolution, bool fullscreen, int vsync)
	{
		this.speedRunMode = speedRunMode;
		Language = language;
		this.fxVolume = fxVolume;
		this.musicVolume = musicVolume;
		this.resolution = resolution;
		this.fullscreen = fullscreen;
		this.vsync = vsync;
	}
}
