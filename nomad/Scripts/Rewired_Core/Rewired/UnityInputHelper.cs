using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal static class UnityInputHelper
	{
		private class XnUUlzTHfQGtZWWIMmBGCOeOIVwZ
		{
			public string[] GEupbqgCGXBYYxjVieTiRjfotijV;

			public string[] dIaLNiQFYWGctQNaFjSMzybBosMK;

			public XnUUlzTHfQGtZWWIMmBGCOeOIVwZ(int P_0)
			{
				GEupbqgCGXBYYxjVieTiRjfotijV = new string[29];
				for (int i = 0; i < GEupbqgCGXBYYxjVieTiRjfotijV.Length; i++)
				{
					GEupbqgCGXBYYxjVieTiRjfotijV[i] = UnityTools.GetUnityInputAxisName(P_0, i);
				}
				if (P_0 + 1 > 16)
				{
					dIaLNiQFYWGctQNaFjSMzybBosMK = new string[20];
					for (int j = 0; j < dIaLNiQFYWGctQNaFjSMzybBosMK.Length; j++)
					{
						dIaLNiQFYWGctQNaFjSMzybBosMK[j] = UnityTools.GetUnityInputButtonName(P_0, j);
					}
				}
			}
		}

		private static XnUUlzTHfQGtZWWIMmBGCOeOIVwZ[] TZJzzFPtVxMPAfRnnBSfHxtbWthNA;

		static UnityInputHelper()
		{
			TZJzzFPtVxMPAfRnnBSfHxtbWthNA = new XnUUlzTHfQGtZWWIMmBGCOeOIVwZ[16];
			for (int i = 0; i < TZJzzFPtVxMPAfRnnBSfHxtbWthNA.Length; i++)
			{
				TZJzzFPtVxMPAfRnnBSfHxtbWthNA[i] = new XnUUlzTHfQGtZWWIMmBGCOeOIVwZ(i);
			}
		}

		public static float GetJoystickAxisValueByJoystickId(int joystickId, int axisIndex)
		{
			if (joystickId <= 0 || joystickId > 16)
			{
				return 0f;
			}
			if (axisIndex >= 29)
			{
				return 0f;
			}
			return Input.GetAxis(TZJzzFPtVxMPAfRnnBSfHxtbWthNA[joystickId - 1].GEupbqgCGXBYYxjVieTiRjfotijV[axisIndex]);
		}

		public static float GetJoystickAxisRawValueByJoystickId(int joystickId, int axisIndex)
		{
			if (joystickId <= 0 || joystickId > 16)
			{
				return 0f;
			}
			if (axisIndex >= 29)
			{
				return 0f;
			}
			return Input.GetAxisRaw(TZJzzFPtVxMPAfRnnBSfHxtbWthNA[joystickId - 1].GEupbqgCGXBYYxjVieTiRjfotijV[axisIndex]);
		}

		public static float GetJoystickAxisValueByJoystickIndex(int joystickIndex, int axisIndex)
		{
			return GetJoystickAxisValueByJoystickId(joystickIndex + 1, axisIndex);
		}

		public static float GetJoystickAxisRawValueByJoystickIndex(int joystickIndex, int axisIndex)
		{
			return GetJoystickAxisRawValueByJoystickId(joystickIndex + 1, axisIndex);
		}

		public static bool GetJoystickButtonValueByJoystickId(int joystickId, int buttonIndex)
		{
			if (joystickId <= 0 || joystickId > 16)
			{
				return false;
			}
			if (buttonIndex >= 20)
			{
				return false;
			}
			int num = joystickId - 1;
			if (joystickId <= 16)
			{
				return Input.GetKey((KeyCode)(350 + 20 * num + buttonIndex));
			}
			return Input.GetButton(TZJzzFPtVxMPAfRnnBSfHxtbWthNA[num].dIaLNiQFYWGctQNaFjSMzybBosMK[buttonIndex]);
		}

		public static bool GetJoystickButtonValueByJoystickIndex(int joystickIndex, int buttonIndex)
		{
			return GetJoystickButtonValueByJoystickId(joystickIndex + 1, buttonIndex);
		}
	}
}
