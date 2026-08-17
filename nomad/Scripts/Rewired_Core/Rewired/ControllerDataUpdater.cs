using System;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class ControllerDataUpdater
	{
		public readonly InputSource source;

		public readonly int axisCount;

		public readonly int buttonCount;

		public readonly float[] axisValues;

		public readonly bool[] buttonValues;

		public readonly float[] buttonPressureValues;

		public readonly bool[] axisHasBeenPressedOSXLinux;

		private readonly UnknownControllerHat[] GdjxqVEaykIeHaBDjLHFGCeYshIX;

		public bool hasReceivedInput;

		public ControllerDataUpdater(InputSource P_0, int P_1, int P_2, UnknownControllerHat[] P_3)
		{
			if (P_1 < 0 || P_2 < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			source = P_0;
			axisCount = P_1;
			buttonCount = P_2;
			GdjxqVEaykIeHaBDjLHFGCeYshIX = P_3;
			axisValues = new float[P_1];
			buttonValues = new bool[P_2];
			buttonPressureValues = new float[P_2];
			axisHasBeenPressedOSXLinux = new bool[P_1];
		}

		public bool IsUnknownHatCardinal(int buttonIndex)
		{
			if (GdjxqVEaykIeHaBDjLHFGCeYshIX == null)
			{
				return false;
			}
			for (int i = 0; i < GdjxqVEaykIeHaBDjLHFGCeYshIX.Length; i++)
			{
				if (GdjxqVEaykIeHaBDjLHFGCeYshIX[i].IsButtonIndexCardinal(buttonIndex))
				{
					return true;
				}
			}
			return false;
		}

		public UnknownControllerHat.HatButtons GetUnknownHatButtons(int buttonIndex)
		{
			if (GdjxqVEaykIeHaBDjLHFGCeYshIX == null)
			{
				return null;
			}
			for (int i = 0; i < GdjxqVEaykIeHaBDjLHFGCeYshIX.Length; i++)
			{
				if (GdjxqVEaykIeHaBDjLHFGCeYshIX[i].ContainsButtonIndex(buttonIndex))
				{
					return GdjxqVEaykIeHaBDjLHFGCeYshIX[i].GetButtons();
				}
			}
			return null;
		}

		public void ClearData()
		{
			Array.Clear(axisValues, 0, axisValues.Length);
			Array.Clear(buttonValues, 0, buttonValues.Length);
			Array.Clear(buttonPressureValues, 0, buttonPressureValues.Length);
			Array.Clear(axisHasBeenPressedOSXLinux, 0, axisHasBeenPressedOSXLinux.Length);
			hasReceivedInput = false;
		}
	}
}
