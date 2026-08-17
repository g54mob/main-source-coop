namespace NomadDrive.Features.OutdoorLighting
{
	public interface IOutdoorLightingManager
	{
		void Register(OutdoorLightController controller);

		void Unregister(OutdoorLightController controller);
	}
}
