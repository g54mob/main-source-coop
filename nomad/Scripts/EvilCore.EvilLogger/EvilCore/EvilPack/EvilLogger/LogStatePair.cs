using System;

namespace EvilCore.EvilPack.EvilLogger
{
	[Serializable]
	public class LogStatePair
	{
		public string key;

		public bool value;

		public LogStatePair(string key, bool value)
		{
			this.key = key;
			this.value = value;
		}
	}
}
