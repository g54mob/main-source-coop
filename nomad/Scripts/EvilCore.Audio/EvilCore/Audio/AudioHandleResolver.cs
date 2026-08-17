namespace EvilCore.Audio
{
	public static class AudioHandleResolver
	{
		private static IAudioHandleOwner _owner;

		public static void Register(IAudioHandleOwner owner)
		{
			_owner = owner;
		}

		public static void Unregister(IAudioHandleOwner owner)
		{
			if (_owner == owner)
			{
				_owner = null;
			}
		}

		public static bool IsActive(AudioHandle handle)
		{
			if (_owner != null)
			{
				return _owner.IsHandleActive(handle);
			}
			return false;
		}
	}
}
