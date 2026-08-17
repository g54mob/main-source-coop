using System;
using System.Globalization;

namespace EvilCore.Localization
{
	internal class LocaleFormatter
	{
		private CultureInfo _currentCulture = CultureInfo.InvariantCulture;

		public void SetLocale(string localeCode)
		{
			try
			{
				_currentCulture = new CultureInfo(localeCode);
			}
			catch (CultureNotFoundException)
			{
				_currentCulture = CultureInfo.InvariantCulture;
			}
		}

		public string FormatNumber(double value, int decimals)
		{
			return value.ToString($"N{decimals}", _currentCulture);
		}

		public string FormatPercent(float ratio, int decimals)
		{
			return (ratio * 100f).ToString($"F{decimals}", _currentCulture) + "%";
		}

		public string FormatDate(DateTime date)
		{
			return date.ToString("d", _currentCulture);
		}

		public string FormatTime(int hours, int minutes)
		{
			return new DateTime(1, 1, 1, hours, minutes, 0).ToString("t", _currentCulture);
		}

		public string FormatTemperature(float celsius, LocalizationConfig.TemperatureUnit unit)
		{
			float num = ((unit == LocalizationConfig.TemperatureUnit.Fahrenheit) ? (celsius * 9f / 5f + 32f) : celsius);
			string text = ((unit == LocalizationConfig.TemperatureUnit.Fahrenheit) ? "°F" : "°C");
			return FormatNumber(num, 1) + text;
		}
	}
}
