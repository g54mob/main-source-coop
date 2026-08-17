using System;

namespace FishNet.Serializing.Helping
{
	public class GeneratedComparer<T>
	{
		public static Func<T, bool> IsDefault { get; set; }

		public static Func<T, T, bool> Compare { get; set; }
	}
}
