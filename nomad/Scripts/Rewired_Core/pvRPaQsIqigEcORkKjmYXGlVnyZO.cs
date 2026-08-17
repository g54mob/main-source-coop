using System;
using Rewired.Platforms.Custom;

internal static class pvRPaQsIqigEcORkKjmYXGlVnyZO
{
	private static CustomPlatformInitOptions wwkrASwUgdANoSUhyGOhRLymDlMdA;

	public static int SOfgKBMShgdzIDTxWoCbImCADEnmb
	{
		get
		{
			if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
			{
				return -1;
			}
			return wwkrASwUgdANoSUhyGOhRLymDlMdA.platformId;
		}
	}

	public static bool GqHFnYqaTUNNdsujzsTuwmmbVupw => SOfgKBMShgdzIDTxWoCbImCADEnmb != -1;

	public static void cBXKtwJGtBRNVxTEewuaskEervpF(CustomPlatformInitOptions P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("options");
		}
		if (GqHFnYqaTUNNdsujzsTuwmmbVupw)
		{
			throw new Exception("Already initialized");
		}
		iutxXJuZBCvRESHowXoMVPaVjwmGA(P_0);
		wwkrASwUgdANoSUhyGOhRLymDlMdA = P_0;
	}

	public static void jGeriDoeSwnLLfJSCNjdJDAAbmJY()
	{
		wwkrASwUgdANoSUhyGOhRLymDlMdA = null;
	}

	internal static int LNjxjxGbyTVeBywFdKknkSLZfJnn()
	{
		if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
		{
			return -1;
		}
		return wwkrASwUgdANoSUhyGOhRLymDlMdA.platformId;
	}

	internal static string NHVnMXpAyAALDdpkOKuGoAwnKIZjA()
	{
		if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
		{
			return null;
		}
		return wwkrASwUgdANoSUhyGOhRLymDlMdA.platformIdentifierString;
	}

	internal static IHardwareJoystickMapCustomPlatformMapProvider erSbfOsDHUvWiaOieWWpWDRaYylv()
	{
		if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
		{
			return null;
		}
		return wwkrASwUgdANoSUhyGOhRLymDlMdA.hardwareJoystickMapCustomPlatformMapProvider;
	}

	internal static CustomInputSource WnzJlECbJGuczDNuGNIGRlfINCnN()
	{
		if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
		{
			return null;
		}
		return wwkrASwUgdANoSUhyGOhRLymDlMdA.inputSource;
	}

	internal static CustomPlatformConfigVars vPlGBaIZhMFrYUFJcbUjSkWdaiNh()
	{
		if (wwkrASwUgdANoSUhyGOhRLymDlMdA == null)
		{
			return null;
		}
		return wwkrASwUgdANoSUhyGOhRLymDlMdA.configVars;
	}

	private static void iutxXJuZBCvRESHowXoMVPaVjwmGA(CustomPlatformInitOptions P_0)
	{
		if (P_0.platformId == -1)
		{
			throw new Exception("customPlatformId is invalid.");
		}
		if (string.IsNullOrEmpty(P_0.platformIdentifierString))
		{
			throw new Exception("platformIdentifierString is invalid.");
		}
		if (P_0.inputSource == null)
		{
			throw new Exception("inputSource cannot be null.");
		}
		if (P_0.hardwareJoystickMapCustomPlatformMapProvider == null)
		{
			throw new Exception("hardwareJoystickMapCustomPlatformMapProvider cannot be null.");
		}
	}
}
