namespace Photon.Voice
{
	public class DeviceFeatures
	{
		internal static DeviceFeatures Default = new DeviceFeatures();

		public CameraFacing CameraFacing { get; private set; }

		public DeviceFeatures()
		{
		}

		public DeviceFeatures(CameraFacing facing)
		{
			CameraFacing = facing;
		}
	}
}
