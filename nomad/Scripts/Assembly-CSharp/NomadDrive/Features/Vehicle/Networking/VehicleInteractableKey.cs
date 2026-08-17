namespace NomadDrive.Features.Vehicle.Networking
{
	public static class VehicleInteractableKey
	{
		public static byte Compose(VehicleInteractableId id, byte instanceIndex)
		{
			return (byte)((uint)(id & (VehicleInteractableId)31) | (uint)((instanceIndex & 7) << 5));
		}

		public static byte DecodeBaseId(byte key)
		{
			return (byte)(key & 0x1F);
		}

		public static byte DecodeInstance(byte key)
		{
			return (byte)((key >> 5) & 7);
		}
	}
}
