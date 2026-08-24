namespace HeathenEngineering.SteamworksIntegration
{
	public static class Currency
	{
		public enum Code
		{
			Unknown = 0,
			AED = 1,
			ARS = 2,
			AUD = 3,
			BRL = 4,
			CAD = 5,
			CHF = 6,
			CLP = 7,
			CNY = 8,
			COP = 9,
			CRC = 10,
			EUR = 11,
			GBP = 12,
			HKD = 13,
			ILS = 14,
			IDR = 15,
			INR = 16,
			JPY = 17,
			KRW = 18,
			KWD = 19,
			KZT = 20,
			MXN = 21,
			MYR = 22,
			NOK = 23,
			NZD = 24,
			PEN = 25,
			PHP = 26,
			PLN = 27,
			QAR = 28,
			RUB = 29,
			SAR = 30,
			SGD = 31,
			THB = 32,
			TRY = 33,
			TWD = 34,
			UAH = 35,
			USD = 36,
			UYU = 37,
			VND = 38,
			ZAR = 39
		}

		public static string GetSymbol(Code code)
		{
			return code switch
			{
				Code.Unknown => string.Empty, 
				Code.AED => "د.إ", 
				Code.BRL => "R$", 
				Code.CHF => "CHF", 
				Code.CNY => "¥", 
				Code.CRC => "₡", 
				Code.EUR => "€", 
				Code.GBP => "£", 
				Code.ILS => "₪", 
				Code.IDR => "Rp", 
				Code.INR => "₹", 
				Code.JPY => "¥", 
				Code.KRW => "₩", 
				Code.KWD => "د.ك", 
				Code.KZT => "лв", 
				Code.MYR => "RM", 
				Code.NOK => "kr", 
				Code.PEN => "S/.", 
				Code.PHP => "₱", 
				Code.PLN => "zł", 
				Code.QAR => "﷼", 
				Code.RUB => "₽", 
				Code.SAR => "﷼", 
				Code.THB => "฿", 
				Code.TRY => "₺", 
				Code.TWD => "NT$", 
				Code.UAH => "₴", 
				Code.UYU => "$U", 
				Code.VND => "₫", 
				Code.ZAR => "R", 
				_ => "$", 
			};
		}
	}
}
