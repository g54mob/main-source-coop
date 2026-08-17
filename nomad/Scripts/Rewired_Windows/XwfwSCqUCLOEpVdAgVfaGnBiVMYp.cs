using Rewired.Utils;

internal class XwfwSCqUCLOEpVdAgVfaGnBiVMYp : fHDswGZGOzbHlnetfyGPLEbLJpvs
{
	public readonly int PPVBIUbZJZhhmPeapPZTXxzJBliQA;

	public readonly int iOUFXAEstgHeAtHYjtRyvVamgvqW;

	public readonly int UxzefBfZVrojhnmWSHmaaYgZHLYtA;

	public readonly int zVLgbAfVUgilSRCSvddlxJvNHuggb;

	public readonly uint lEqVbdiCOZzrhxHIzyETQUAafneE;

	public readonly uint QsyyqZZwmbdzqPWucwazhRXrbkdB;

	public readonly int aakBsiAwRgbofnHoTiAvPefkKqLgA;

	private readonly int kcCHLZYdaojUgeZPIJJaTAnZiVoRA;

	public uint fUnDHXRxGymEygdLXSbiTjexADKf;

	public int jCyfwtFcUHvfdeMyeasmtzzNuWvKA
	{
		get
		{
			if (fUnDHXRxGymEygdLXSbiTjexADKf < PPVBIUbZJZhhmPeapPZTXxzJBliQA || fUnDHXRxGymEygdLXSbiTjexADKf > iOUFXAEstgHeAtHYjtRyvVamgvqW)
			{
				return -1;
			}
			int num = (int)((fUnDHXRxGymEygdLXSbiTjexADKf - PPVBIUbZJZhhmPeapPZTXxzJBliQA) / kcCHLZYdaojUgeZPIJJaTAnZiVoRA * 4500);
			if (num >= 36000)
			{
				num = 0;
			}
			return num;
		}
	}

	public XwfwSCqUCLOEpVdAgVfaGnBiVMYp(byte P_0, ushort P_1, ushort P_2, int P_3, int P_4, int P_5, int P_6, int P_7, int P_8, uint P_9, uint P_10, int P_11)
		: base(P_0, P_1, P_2, P_3, P_4)
	{
		PPVBIUbZJZhhmPeapPZTXxzJBliQA = P_5;
		iOUFXAEstgHeAtHYjtRyvVamgvqW = P_6;
		lEqVbdiCOZzrhxHIzyETQUAafneE = P_9;
		QsyyqZZwmbdzqPWucwazhRXrbkdB = P_10;
		aakBsiAwRgbofnHoTiAvPefkKqLgA = P_11;
		UxzefBfZVrojhnmWSHmaaYgZHLYtA = P_5 - 1;
		if (UxzefBfZVrojhnmWSHmaaYgZHLYtA < 0)
		{
			UxzefBfZVrojhnmWSHmaaYgZHLYtA = P_6 + 1;
		}
		zVLgbAfVUgilSRCSvddlxJvNHuggb = -1;
		int num = P_6 - P_5 + 1;
		kcCHLZYdaojUgeZPIJJaTAnZiVoRA = MathTools.Clamp(num / 8, 1, 2147483647);
		gpsKOLTRVFtMWsMaAmbwcnvefEfv();
	}

	public virtual void kWqLJgkUCrapqbcjRBKnCCutiOGzA()
	{
		fUnDHXRxGymEygdLXSbiTjexADKf = (uint)UxzefBfZVrojhnmWSHmaaYgZHLYtA;
	}
}
