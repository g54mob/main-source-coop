namespace Photon.Voice
{
	public static class Utility
	{
		public static bool IsAudio(this Codec c)
		{
			return c == Codec.AudioOpus;
		}

		public static bool IsVideo(this Codec c)
		{
			if ((byte)c >= 20)
			{
				return (byte)c < 100;
			}
			return false;
		}
	}
}
