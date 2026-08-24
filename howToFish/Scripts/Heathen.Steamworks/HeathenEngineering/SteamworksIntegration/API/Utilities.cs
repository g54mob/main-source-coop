using System;
using System.Net;
using Steamworks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration.API
{
	public static class Utilities
	{
		public static class Client
		{
			private static Callback<AppResumingFromSuspend_t> m_AppResumingFromSuspend_t;

			private static Callback<FloatingGamepadTextInputDismissed_t> m_FloatingGamepadTextInputDismissed_t;

			private static UnityEvent eventAppResumeFromSuspend;

			private static UnityEvent eventKeyboardShown;

			private static UnityEvent eventKeyboardClosed;

			public static UnityEvent EventAppResumFromSuspend
			{
				get
				{
					if (m_AppResumingFromSuspend_t == null)
					{
						m_AppResumingFromSuspend_t = Callback<AppResumingFromSuspend_t>.Create(delegate
						{
							eventAppResumeFromSuspend.Invoke();
						});
					}
					return eventAppResumeFromSuspend;
				}
			}

			public static UnityEvent EventKeyboardShown => eventKeyboardShown;

			public static UnityEvent EventKeyboardClosed
			{
				get
				{
					if (m_FloatingGamepadTextInputDismissed_t == null)
					{
						m_FloatingGamepadTextInputDismissed_t = Callback<FloatingGamepadTextInputDismissed_t>.Create(delegate
						{
							eventKeyboardClosed.Invoke();
						});
					}
					return eventKeyboardClosed;
				}
			}

			public static string IpCountry => SteamUtils.GetIPCountry();

			public static uint SecondsSinceAppActive => SteamUtils.GetSecondsSinceAppActive();

			public static DateTime ServerRealTime => new DateTime(1970, 1, 1).AddSeconds(SteamUtils.GetServerRealTime());

			public static string SteamUILanguage => SteamUtils.GetSteamUILanguage();

			public static bool IsSteamInBigPictureMode => SteamUtils.IsSteamInBigPictureMode();

			public static bool IsSteamRunningInVR => SteamUtils.IsSteamRunningInVR();

			public static bool IsSteamRunningOnSteamDeck => SteamUtils.IsSteamRunningOnSteamDeck();

			public static bool IsVRHeadsetStreamingEnabled
			{
				get
				{
					return SteamUtils.IsVRHeadsetStreamingEnabled();
				}
				set
				{
					SteamUtils.SetVRHeadsetStreamingEnabled(value);
				}
			}

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Init()
			{
				m_AppResumingFromSuspend_t = null;
				m_FloatingGamepadTextInputDismissed_t = null;
				eventKeyboardShown = new UnityEvent();
				eventKeyboardClosed = new UnityEvent();
			}

			public static void SetGameLauncherMode(bool mode)
			{
				SteamUtils.SetGameLauncherMode(mode);
			}

			public static void StartVRDashboard()
			{
				SteamUtils.StartVRDashboard();
			}

			public static bool ShowVirtualKeyboard(EFloatingGamepadTextInputMode mode, int2 fieldPosition, int2 fieldSize)
			{
				if (SteamUtils.ShowFloatingGamepadTextInput(mode, fieldPosition.x, fieldPosition.y, fieldSize.x, fieldSize.y))
				{
					eventKeyboardShown.Invoke();
					return true;
				}
				return false;
			}

			public static bool ShowVirtualKeyboard(EFloatingGamepadTextInputMode mode, RectTransform fieldTransform, Canvas canvas)
			{
				Rect rect = RectTransformUtility.PixelAdjustRect(fieldTransform, canvas);
				if (SteamUtils.ShowFloatingGamepadTextInput(mode, (int)rect.x, (int)rect.y, (int)rect.size.x, (int)rect.size.y))
				{
					eventKeyboardShown.Invoke();
					return true;
				}
				return false;
			}

			public static bool ShowVirtualKeyboard(EFloatingGamepadTextInputMode mode, float2 fieldPosition, float2 fieldSize)
			{
				if (SteamUtils.ShowFloatingGamepadTextInput(mode, Convert.ToInt32(fieldPosition.x), Convert.ToInt32(fieldPosition.y), Convert.ToInt32(fieldSize.x), Convert.ToInt32(fieldSize.y)))
				{
					eventKeyboardShown.Invoke();
					return true;
				}
				return false;
			}
		}

		public static uint IPStringToUint(string address)
		{
			byte[] array = IPStringToBytes(address);
			return (uint)((array[0] << 24) + (array[1] << 16) + (array[2] << 8) + array[3]);
		}

		public static string IPUintToString(uint address)
		{
			byte[] bytes = BitConverter.GetBytes(address);
			return new IPAddress(new byte[4]
			{
				bytes[3],
				bytes[2],
				bytes[1],
				bytes[0]
			}).ToString();
		}

		public static byte[] IPStringToBytes(string address)
		{
			return IPAddress.Parse(address).GetAddressBytes();
		}

		public static byte[] FlipImageBufferVertical(int width, int height, byte[] buffer)
		{
			byte[] array = new byte[buffer.Length];
			int num = width * 4;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < num; j++)
				{
					array[j + (height - 1 - i) * num] = buffer[j + num * i];
				}
			}
			return array;
		}

		public static bool FindToken(string openPattern, string closePattern, string input, out string result)
		{
			if (input.Contains(openPattern) && input.Contains(closePattern))
			{
				int num = input.LastIndexOf(closePattern);
				int num2 = input.IndexOf(closePattern, num);
				if (num2 != -1)
				{
					result = input.Substring(num, num2 - num + 1);
					return true;
				}
				result = string.Empty;
				return false;
			}
			result = string.Empty;
			return false;
		}
	}
}
