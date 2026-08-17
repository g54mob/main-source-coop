namespace EvilCore.Audio
{
	public readonly struct AudioHandle
	{
		public readonly uint Id;

		public bool IsValid
		{
			get
			{
				if (Id != 0)
				{
					return AudioHandleResolver.IsActive(this);
				}
				return false;
			}
		}

		public static AudioHandle Invalid => default(AudioHandle);

		public AudioHandle(uint id)
		{
			Id = id;
		}
	}
}
