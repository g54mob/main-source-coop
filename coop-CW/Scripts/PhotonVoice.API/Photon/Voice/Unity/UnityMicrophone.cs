using System.Linq;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public static class UnityMicrophone
	{
		public static string[] devices => Microphone.devices;

		public static void End(string deviceName)
		{
			Microphone.End(deviceName);
		}

		public static void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
		{
			Microphone.GetDeviceCaps(deviceName, out minFreq, out maxFreq);
		}

		public static int GetPosition(string deviceName)
		{
			return Microphone.GetPosition(deviceName);
		}

		public static bool IsRecording(string deviceName)
		{
			return Microphone.IsRecording(deviceName);
		}

		public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
		{
			return Microphone.Start(deviceName, loop, lengthSec, frequency);
		}

		public static string CheckDevice(ILogger logger, string logPref, string device, int suggestedFrequency, out int frequency)
		{
			if (Microphone.devices.Length < 1)
			{
				string text = "No microphones found (Microphone.devices is empty)";
				logger.LogError(logPref + text);
				frequency = 0;
				return text;
			}
			if (!string.IsNullOrEmpty(device) && !Microphone.devices.Contains(device))
			{
				string text2 = $"[PV] MicWrapper: \"{device}\" is not a valid Unity microphone device, falling back to default one";
				logger.LogError(logPref + text2);
				frequency = 0;
				return text2;
			}
			logger.LogInfo("[PV] MicWrapper: initializing microphone '{0}', suggested frequency = {1}).", device, suggestedFrequency);
			Microphone.GetDeviceCaps(device, out var minFreq, out var maxFreq);
			frequency = suggestedFrequency;
			if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.PS5)
			{
				if (suggestedFrequency != minFreq && suggestedFrequency != maxFreq)
				{
					int num = ((suggestedFrequency <= minFreq) ? minFreq : maxFreq);
					logger.LogWarning(logPref + "microphone does not support suggested frequency {0} (supported frequencies are: {1} and {2}). Setting to {3}", suggestedFrequency, minFreq, maxFreq, num);
					frequency = num;
				}
			}
			else if (suggestedFrequency < minFreq || (maxFreq != 0 && suggestedFrequency > maxFreq))
			{
				logger.LogWarning(logPref + "microphone does not support suggested frequency {0} (min: {1}, max: {2}). Setting to {2}", suggestedFrequency, minFreq, maxFreq);
				frequency = maxFreq;
			}
			return null;
		}
	}
}
