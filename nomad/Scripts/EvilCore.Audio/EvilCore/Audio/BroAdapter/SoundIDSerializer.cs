using Ami.BroAudio;
using Mirror;

namespace EvilCore.Audio.BroAdapter
{
	public static class SoundIDSerializer
	{
		public static void WriteSoundID(this NetworkWriter writer, SoundID value)
		{
			writer.WriteString(value.IsValid() ? value.ToString() : string.Empty);
		}

		public static SoundID ReadSoundID(this NetworkReader reader)
		{
			string text = reader.ReadString();
			if (string.IsNullOrEmpty(text))
			{
				return default(SoundID);
			}
			BroAudioEntityRegistry activeInstance = BroAudioEntityRegistry.ActiveInstance;
			if (!(activeInstance != null))
			{
				return default(SoundID);
			}
			return activeInstance.GetByName(text);
		}
	}
}
