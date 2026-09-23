using System;
using System.Collections.Generic;

namespace Mimicraft.Customization
{
	public static class PortraitStudios
	{
		private static readonly List<Action> closers = new List<Action>();

		public static void Register(Action close)
		{
			if (close != null && !closers.Contains(close))
			{
				closers.Add(close);
			}
		}

		public static void CloseAll()
		{
			foreach (Action closer in closers)
			{
				closer();
			}
		}
	}
}
