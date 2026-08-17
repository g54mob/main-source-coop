using System;

namespace Mirror.Examples.PickupsDropsChilds
{
	[Serializable]
	public struct EquippedItemConfig : IEquatable<EquippedItemConfig>
	{
		public byte usages;

		public byte maxUsages;

		public EquippedItemConfig(byte maxUsages)
		{
			usages = maxUsages;
			this.maxUsages = maxUsages;
		}

		public EquippedItemConfig(byte usages, byte maxUsages)
		{
			this.usages = usages;
			this.maxUsages = maxUsages;
		}

		public void Use()
		{
			ResetUsages(usages);
			if (usages > 0)
			{
				usages--;
			}
		}

		public void AddUsages(byte usages)
		{
			this.usages = (byte)Mathd.Clamp(this.usages + usages, 0.0, (int)maxUsages);
		}

		public void ResetUsages()
		{
			usages = maxUsages;
		}

		public void ResetUsages(byte usages)
		{
			this.usages = (byte)Mathd.Clamp((int)usages, 0.0, (int)maxUsages);
		}

		public bool Equals(EquippedItemConfig other)
		{
			if (usages == other.usages)
			{
				return maxUsages == other.maxUsages;
			}
			return false;
		}

		public override string ToString()
		{
			return $"EquippedItemConfig[{usages}/{maxUsages}]";
		}
	}
}
