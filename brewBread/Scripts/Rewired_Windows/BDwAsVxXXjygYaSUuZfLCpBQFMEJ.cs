using System;
using Rewired;
using Rewired.Platforms;
using Rewired.Utils.Classes.Utility;

internal static class BDwAsVxXXjygYaSUuZfLCpBQFMEJ
{
	private const bool pWPtSdsZlyuutIjgfNoPUFvNIlAR = false;

	private static int PquFwGhWAQKtIdskApoIeglseMjab;

	private static ThreadHelper EKZIXgJmmVUaCYPfCdjzKTDWVgPu;

	private static ThreadHelper sQeeaNmkXobQthVZUeqCgJDnIzvbA;

	public static int NombDecpdaRJnwimtgaeUHHBCkQO => PquFwGhWAQKtIdskApoIeglseMjab;

	public static ThreadHelper FYMGCkBBTvgPUrkQJoeEksiLqflfb => EKZIXgJmmVUaCYPfCdjzKTDWVgPu;

	public static ThreadHelper XsLVFvausmxPmDDAcJfhUiFpBuXl => sQeeaNmkXobQthVZUeqCgJDnIzvbA;

	public static ThreadHelper LrMvxKgLebxAruEpHRhBkzmuwWOF => EKZIXgJmmVUaCYPfCdjzKTDWVgPu;

	public static ThreadHelper yTVbPyUIUIUOrvAjtqUooaqsSzuO => sQeeaNmkXobQthVZUeqCgJDnIzvbA;

	public static ThreadHelper KRBFGXsiieIueiWWfqDOFQVKersY => EKZIXgJmmVUaCYPfCdjzKTDWVgPu;

	public static ThreadHelper LYfdDipAfVVfvKJWhGuxLsxOMYnQ => EKZIXgJmmVUaCYPfCdjzKTDWVgPu;

	public static bool iDoQYXPTqrRjcmcGEUbjYYYBIDod
	{
		get
		{
			if (EKZIXgJmmVUaCYPfCdjzKTDWVgPu != null)
			{
				return EKZIXgJmmVUaCYPfCdjzKTDWVgPu.isRunning;
			}
			return false;
		}
	}

	public static void iNjZxEWgGYMxgNiUyUmpWefCBkbC()
	{
		PquFwGhWAQKtIdskApoIeglseMjab = ReInput.configVars.GetPlatformVar_joystickRefreshRate();
		if (EKZIXgJmmVUaCYPfCdjzKTDWVgPu != null)
		{
			throw new Exception("Input Thread Manager is already initialized.");
		}
		EKZIXgJmmVUaCYPfCdjzKTDWVgPu = ThreadHelper.CreateFixedTimeStep(PquFwGhWAQKtIdskApoIeglseMjab);
		EKZIXgJmmVUaCYPfCdjzKTDWVgPu.Start(wait: true);
		if (ReInput.configVars.useXInput || ReInput.configuration.windowsStandalonePrimaryInputSource == WindowsStandalonePrimaryInputSource.XInput)
		{
			sQeeaNmkXobQthVZUeqCgJDnIzvbA = ThreadHelper.CreateFixedTimeStep(100);
			sQeeaNmkXobQthVZUeqCgJDnIzvbA.Start(wait: true);
		}
		ReInput.UpdateStartedEvent += GyjzacVgWaMnFhItqiFYNDebsiFy;
	}

	private static void GyjzacVgWaMnFhItqiFYNDebsiFy(UpdateLoopType P_0)
	{
		if (P_0 == UpdateLoopType.Update)
		{
			int platformVar_joystickRefreshRate = ReInput.configVars.GetPlatformVar_joystickRefreshRate();
			if (PquFwGhWAQKtIdskApoIeglseMjab != platformVar_joystickRefreshRate)
			{
				PquFwGhWAQKtIdskApoIeglseMjab = platformVar_joystickRefreshRate;
				EKZIXgJmmVUaCYPfCdjzKTDWVgPu.fixedTimeStepFPS = platformVar_joystickRefreshRate;
			}
		}
	}

	public static void lDxnsjCDTQrmresvWgbliNUVruIc()
	{
		ReInput.UpdateStartedEvent -= GyjzacVgWaMnFhItqiFYNDebsiFy;
		if (EKZIXgJmmVUaCYPfCdjzKTDWVgPu != null)
		{
			EKZIXgJmmVUaCYPfCdjzKTDWVgPu.WaitForActionQueueToFinish();
			EKZIXgJmmVUaCYPfCdjzKTDWVgPu.Dispose();
			EKZIXgJmmVUaCYPfCdjzKTDWVgPu = null;
		}
		if (sQeeaNmkXobQthVZUeqCgJDnIzvbA != null)
		{
			sQeeaNmkXobQthVZUeqCgJDnIzvbA.WaitForActionQueueToFinish();
			sQeeaNmkXobQthVZUeqCgJDnIzvbA.Dispose();
			sQeeaNmkXobQthVZUeqCgJDnIzvbA = null;
		}
	}
}
