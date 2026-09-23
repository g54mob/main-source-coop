using UnityEngine;

namespace Mimicraft
{
	public static class Build
	{
		public static BuildKind Kind => BuildKind.Demo;

		public static string Label => Kind switch
		{
			BuildKind.Demo => "DEMO", 
			BuildKind.Playtest => "PLAYTEST", 
			_ => "", 
		};

		public static string VersionLabel
		{
			get
			{
				if (!string.IsNullOrEmpty(Label))
				{
					return Application.version + " - " + Label;
				}
				return Application.version;
			}
		}
	}
}
