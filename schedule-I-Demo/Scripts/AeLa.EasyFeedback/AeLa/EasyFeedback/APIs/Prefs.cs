using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public struct Prefs
	{
		public string background;

		public object backgroundImage;

		public object backgroundImageScaled;

		public string backgroundBrightness;

		public string backgroundColor;

		public bool? backgroundTile;

		public bool? calendarFeedEnabled;

		public bool? canBePublic;

		public bool? canBeOrg;

		public bool? canBePrivate;

		public bool? canInvite;

		public CardAgeMode? cardAging;

		public Invitations? invitations;

		public PermissionLevel? permissionLevel;

		public bool? selfJoin;

		public bool? cardCovers;

		public AccessibilityLevel? voting;

		public AccessibilityLevel? comments;
	}
}
