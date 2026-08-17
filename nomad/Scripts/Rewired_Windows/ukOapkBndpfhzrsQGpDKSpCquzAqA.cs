internal static class ukOapkBndpfhzrsQGpDKSpCquzAqA
{
	public static string REdCZcEnNdBEDFORweJqIprradFS(string P_0)
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

	public static fBMrrsvbWMcOcdDbjcFUuOyQnpTbb YpfcnAiPGIcplppqFKAqZQDUkNZPA(int P_0)
	{
		switch (P_0)
		{
		case 0:
			return fBMrrsvbWMcOcdDbjcFUuOyQnpTbb.LostFocus;
		case 1:
		case 2:
			return fBMrrsvbWMcOcdDbjcFUuOyQnpTbb.GainedFocus;
		default:
			return fBMrrsvbWMcOcdDbjcFUuOyQnpTbb.None;
		}
	}
}
