namespace Febucci.UI
{
	public static class TAnimTags
	{
		public const string bh_Shake = "shake";

		public const string bh_Rot = "rot";

		public const string bh_Wiggle = "wiggle";

		public const string bh_Wave = "wave";

		public const string bh_Swing = "swing";

		public const string bh_Incr = "incr";

		public const string bh_Slide = "slide";

		public const string bh_Bounce = "bounce";

		public const string bh_Fade = "fade";

		public const string bh_Rainb = "rainb";

		public const string bh_Dangle = "dangle";

		public const string bh_Pendulum = "pend";

		public const string ap_Size = "size";

		public const string ap_Fade = "fade";

		public const string ap_Offset = "offset";

		public const string ap_RandomDir = "rdir";

		public const string ap_VertExp = "vertexp";

		public const string ap_HoriExp = "horiexp";

		public const string ap_DiagExp = "diagexp";

		public const string ap_Rot = "rot";

		public static readonly string[] defaultBehaviors = new string[12]
		{
			"shake", "rot", "wiggle", "wave", "swing", "incr", "slide", "bounce", "fade", "rainb",
			"dangle", "pend"
		};

		public static readonly string[] defaultAppearances = new string[8] { "size", "fade", "offset", "vertexp", "horiexp", "diagexp", "rot", "rdir" };
	}
}
