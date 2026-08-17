using System;

namespace EvilCore.Particles
{
	[Serializable]
	public class ParticleEntry
	{
		public string key;

		public string assetGuid;

		public string address;

		public string displayName;

		public int defaultPoolSize = 3;

		public override string ToString()
		{
			return $"{displayName} (pool:{defaultPoolSize})";
		}

		public override bool Equals(object obj)
		{
			if (obj is ParticleEntry particleEntry)
			{
				return assetGuid == particleEntry.assetGuid;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return assetGuid?.GetHashCode() ?? 0;
		}
	}
}
