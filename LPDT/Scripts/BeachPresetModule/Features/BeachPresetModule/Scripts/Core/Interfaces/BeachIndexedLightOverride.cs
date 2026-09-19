using Features.LevelLightModule.Scripts;
using Features.LevelModule.Scripts.RoomVariations;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachIndexedLightOverride
	{
		public int LightIndex { get; set; }

		public LightSettingData Settings { get; set; }

		public RoomType RoomOverride { get; set; }
	}
}
