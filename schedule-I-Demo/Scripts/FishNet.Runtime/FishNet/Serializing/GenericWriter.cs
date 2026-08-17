using System;
using FishNet.Documenting;

namespace FishNet.Serializing
{
	[APIExclude]
	public static class GenericWriter<T>
	{
		public static Action<Writer, T> Write { get; set; }

		public static Action<Writer, T, AutoPackType> WriteAutoPack { get; set; }
	}
}
