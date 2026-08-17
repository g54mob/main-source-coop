namespace EvilCore.Networking
{
	public readonly struct VoiceAudioDevice
	{
		public readonly string Id;

		public readonly string Name;

		public readonly bool IsDefault;

		public bool IsValid => !string.IsNullOrEmpty(Id);

		public VoiceAudioDevice(string id, string name, bool isDefault)
		{
			Id = id;
			Name = name;
			IsDefault = isDefault;
		}
	}
}
