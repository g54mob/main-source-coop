namespace Features.LevelLightModule.Scripts
{
	public interface ILightObjectsGroupService
	{
		void SetLightObjectGroupState(LightObjectGroup group, bool state);

		void SetLightObjectGroupState(LightObjectGroup group, LightSettingData lightSettingData);
	}
}
