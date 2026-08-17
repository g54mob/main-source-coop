using Rewired.Dev;

namespace RewiredConsts
{
	public static class Action
	{
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "HorizontalMovement")]
		public const int HorizontalMovement = 0;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Swing")]
		public const int Swing = 22;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Jump")]
		public const int Jump = 1;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Anchor")]
		public const int Anchor = 2;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Grab")]
		public const int Grab = 3;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "PullRope")]
		public const int PullRope = 4;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "SaveCheckpoint")]
		public const int SaveCheckpoint = 5;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "LoadCheckpoint")]
		public const int LoadCheckpoint = 6;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "EmoteThumbsUp")]
		public const int EmoteThumbsUp = 7;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "EmoteClap")]
		public const int EmoteClap = 8;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "EmoteRockPaperScisors")]
		public const int EmoteRockPaperScisors = 9;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "EmoteTimer")]
		public const int EmoteTimer = 10;

		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Interact")]
		public const int Interact = 21;

		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Pause")]
		public const int Pause = 13;

		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIHorizontal")]
		public const int UIHorizontal = 14;

		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIVertical")]
		public const int UIVertical = 15;

		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UISubmit")]
		public const int UISubmit = 18;

		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UICancel")]
		public const int UICancel = 19;
	}
}
