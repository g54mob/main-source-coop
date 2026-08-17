internal static class OfsJTgppBgDTpDvZTiIeeOcLSHFpA
{
	public static string IKpcTSZShIqPngUEFfdHqwsaiFMM(string P_0)
	{
		if (P_0 == null || P_0 == string.Empty)
		{
			return string.Empty;
		}
		int num = P_0.LastIndexOf('\\');
		if (num < 0 || num >= P_0.Length - 1)
		{
			return P_0;
		}
		return P_0.Substring(num + 1);
	}

	public static tuNShypmhgUozYlqKBRHDpTzKZMW nEmLDwDTSksoYnKcxcZZhEqaoXEnA(uint P_0)
	{
		return P_0 switch
		{
			8u => tuNShypmhgUozYlqKBRHDpTzKZMW.LostFocus, 
			7u => tuNShypmhgUozYlqKBRHDpTzKZMW.GainedFocus, 
			_ => tuNShypmhgUozYlqKBRHDpTzKZMW.None, 
		};
	}
}
