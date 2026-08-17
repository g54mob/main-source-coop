namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal static class ControllerElementRole
	{
		public const string stick1X = "stick1/x";

		public const string stick1Y = "stick1/y";

		public const string stick1Z = "stick1/z";

		public const string stick1Press = "stick1/press";

		public const string stick1Trigger = "stick1/trigger";

		public const string stick1TriggerStage1 = "stick1/trigger/stage1";

		public const string stick2X = "stick2/x";

		public const string stick2Y = "stick2/y";

		public const string stick2Z = "stick2/z";

		public const string stick2Press = "stick2/press";

		public const string stick2Trigger = "stick2/trigger";

		public const string stick2TriggerStage1 = "stick2/trigger/stage1";

		public const string gamepadA = "gamepad/a";

		public const string gamepadB = "gamepad/b";

		public const string gamepadC = "gamepad/c";

		public const string gamepadX = "gamepad/x";

		public const string gamepadY = "gamepad/y";

		public const string gamepadZ = "gamepad/z";

		public const string gamepadLB = "gamepad/lb";

		public const string gamepadRB = "gamepad/rb";

		public const string gamepadLT = "gamepad/lt";

		public const string gamepadRT = "gamepad/rt";

		public const string dpadU = "dpad/up";

		public const string dpadR = "dpad/right";

		public const string dpadD = "dpad/down";

		public const string dpadL = "dpad/left";

		public const string gamepadSelect = "gamepad/select";

		public const string gamepadStart = "gamepad/start";

		public const string gamepadLogoButton = "gamepad/logo_button";

		public const string home = "home";

		public const string capture = "capture";

		public const string chat = "chat";

		public const string mute = "mute";

		public const string gamepadTouchpadPress = "gamepad/touchpad/press";

		public const string wheel = "racing/wheel";

		public const string accelerator = "racing/accelerator";

		public const string brake = "racing/brake";

		public const string clutch = "racing/clutch";

		public const string gearShiftUp = "racing/gear/up";

		public const string gearShiftDown = "racing/gear/down";

		public const string gearReverse = "racing/gear/reverse";

		public const string gearShifter1 = "racing/gear/1";

		public const string gearShifter2 = "racing/gear/2";

		public const string gearShifter3 = "racing/gear/3";

		public const string gearShifter4 = "racing/gear/4";

		public const string gearShifter5 = "racing/gear/5";

		public const string gearShifter6 = "racing/gear/6";

		public const string gearShifter7 = "racing/gear/7";

		public const string gearShifter8 = "racing/gear/8";

		public const string gearShifter9 = "racing/gear/9";

		public const string gearShifter10 = "racing/gear/10";

		public const string flightThrottle = "flight/throttle";

		public const string flightYokeRotate = "flight/yoke/rotate";

		public const string flightYokeZ = "flight/yoke/z";

		public const string flightLeftPedal = "flight/left_pedal";

		public const string flightRightPedal = "flight/right_pedal";

		public const string flightSlidePedals = "flight/slide_pedals";

		public const string joyMouse1X = "joy_mouse1/x";

		public const string joyMouse1Y = "joy_mouse1/y";

		public const string joyMouse2X = "joy_mouse2/x";

		public const string joyMouse2Y = "joy_mouse2/y";

		public const string joyConLeftSL = "left_joycon/sl";

		public const string joyConLeftSR = "left_joycon/sr";

		public const string joyConRightSL = "right_joycon/sl";

		public const string joyConRightSR = "right_joycon/sr";

		public const string joyConSL = "joycon/sl";

		public const string joyConSR = "joycon/sr";

		private static string[] __s_names;

		private static string[] s_names
		{
			get
			{
				if (__s_names == null)
				{
					__s_names = new string[57]
					{
						"stick1/x", "stick1/y", "stick1/z", "stick1/press", "stick1/trigger", "stick1/trigger/stage1", "stick2/x", "stick2/y", "stick2/z", "stick2/press",
						"stick2/trigger", "stick2/trigger/stage1", "gamepad/a", "gamepad/b", "gamepad/c", "gamepad/x", "gamepad/y", "gamepad/z", "gamepad/lb", "gamepad/rb",
						"gamepad/lt", "gamepad/rt", "dpad/up", "dpad/right", "dpad/down", "dpad/left", "gamepad/select", "gamepad/start", "gamepad/logo_button", "home",
						"capture", "chat", "mute", "gamepad/touchpad/press", "racing/wheel", "racing/accelerator", "racing/brake", "racing/clutch", "racing/gear/up", "racing/gear/down",
						"racing/gear/reverse", "racing/gear/1", "racing/gear/2", "racing/gear/3", "racing/gear/4", "racing/gear/5", "racing/gear/6", "racing/gear/7", "racing/gear/8", "racing/gear/9",
						"racing/gear/10", "flight/throttle", "flight/yoke/rotate", "flight/yoke/z", "flight/left_pedal", "flight/right_pedal", "flight/slide_pedals"
					};
				}
				return __s_names;
			}
		}

		public static string[] GetNames()
		{
			return (string[])s_names.Clone();
		}
	}
}
