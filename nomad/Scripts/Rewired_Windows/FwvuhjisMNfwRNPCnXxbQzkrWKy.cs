using System;
using Rewired;
using Rewired.Utils.Classes.Utility;

internal static class FwvuhjisMNfwRNPCnXxbQzkrWKy
{
	private static int cenyvllKXlALFJtFTfJKtaukJNNib;

	private static ThreadHelper tsIzERwWiQjbkqlFvaTxozEudwTb;

	private static ThreadHelper BmclmbDvAJFPgJECChFIoCwxSmIC;

	public static int VjeFpcjaFerIirGHKspqAQDAgjGxA => cenyvllKXlALFJtFTfJKtaukJNNib;

	public static ThreadHelper fEAUVDpMSAiwvnNRVUnogqubQGhf => tsIzERwWiQjbkqlFvaTxozEudwTb;

	public static ThreadHelper apYJZWeiHEkMNcOtWVjXVlAhaguy => BmclmbDvAJFPgJECChFIoCwxSmIC;

	public static void KzanHXbflnAqADtLFigURXhKxzaZ(bool P_0)
	{
		cenyvllKXlALFJtFTfJKtaukJNNib = ReInput.configVars.GetPlatformVar_joystickRefreshRate();
		if (tsIzERwWiQjbkqlFvaTxozEudwTb != null)
		{
			throw new Exception("Input Thread Manager is already initialized.");
		}
		tsIzERwWiQjbkqlFvaTxozEudwTb = ThreadHelper.CreateFixedTimeStep(cenyvllKXlALFJtFTfJKtaukJNNib);
		tsIzERwWiQjbkqlFvaTxozEudwTb.Start(wait: true);
		if (P_0)
		{
			BmclmbDvAJFPgJECChFIoCwxSmIC = ThreadHelper.CreateFixedTimeStep(100);
			BmclmbDvAJFPgJECChFIoCwxSmIC.Start(wait: true);
		}
		ReInput.UpdateStartedEvent += rmhDATIMvIfqmEffVvmxFypdLTqNB;
	}

	private static void rmhDATIMvIfqmEffVvmxFypdLTqNB(UpdateLoopType P_0)
	{
		if (P_0 == UpdateLoopType.Update)
		{
			int platformVar_joystickRefreshRate = ReInput.configVars.GetPlatformVar_joystickRefreshRate();
			if (cenyvllKXlALFJtFTfJKtaukJNNib != platformVar_joystickRefreshRate)
			{
				cenyvllKXlALFJtFTfJKtaukJNNib = platformVar_joystickRefreshRate;
				tsIzERwWiQjbkqlFvaTxozEudwTb.fixedTimeStepFPS = platformVar_joystickRefreshRate;
			}
		}
	}

	public static void wFjbdaEaLsAZFmfjExaiwfzfniukA()
	{
		ReInput.UpdateStartedEvent -= rmhDATIMvIfqmEffVvmxFypdLTqNB;
		if (tsIzERwWiQjbkqlFvaTxozEudwTb != null)
		{
			tsIzERwWiQjbkqlFvaTxozEudwTb.WaitForActionQueueToFinish();
			tsIzERwWiQjbkqlFvaTxozEudwTb.Dispose();
			tsIzERwWiQjbkqlFvaTxozEudwTb = null;
		}
		if (BmclmbDvAJFPgJECChFIoCwxSmIC != null)
		{
			BmclmbDvAJFPgJECChFIoCwxSmIC.WaitForActionQueueToFinish();
			BmclmbDvAJFPgJECChFIoCwxSmIC.Dispose();
			BmclmbDvAJFPgJECChFIoCwxSmIC = null;
		}
	}
}
