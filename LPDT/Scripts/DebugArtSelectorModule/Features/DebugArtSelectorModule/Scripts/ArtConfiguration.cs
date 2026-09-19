using UnityEngine;

namespace Features.DebugArtSelectorModule.Scripts
{
	[CreateAssetMenu(fileName = "ArtConfiguration_Default", menuName = "Configurations/DebugArtSelectorModule/ArtConfiguration")]
	public class ArtConfiguration : ScriptableObject
	{
		[ColorUsage(false, true)]
		public Color AmbientColor;

		public Color RealtimeShadowColor;

		public bool Fog;

		public FogMode FogMode;

		public Color FogColor;

		public float FogStartDistance;

		public float FogEndDistance;

		public bool ReApplyWeather;

		public bool UseDebugAmbientFromLevel;

		public bool FogTransitionEnabled;
	}
}
