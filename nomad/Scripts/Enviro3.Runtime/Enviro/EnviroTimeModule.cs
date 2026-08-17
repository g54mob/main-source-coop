using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroTimeModule : EnviroModule
	{
		public EnviroTime Settings;

		public EnviroTimeModule preset;

		public bool showTimeControls;

		public bool showLocationControls;

		public float LST;

		private float internalTimeOverflow;

		public int seconds
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Second;
				}
				return Settings.secSerial;
			}
			set
			{
				SetDateTime(value, Settings.minSerial, Settings.hourSerial, Settings.daySerial, Settings.monthSerial, Settings.yearSerial);
			}
		}

		public int minutes
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Minute;
				}
				return Settings.minSerial;
			}
			set
			{
				SetDateTime(Settings.secSerial, value, Settings.hourSerial, Settings.daySerial, Settings.monthSerial, Settings.yearSerial);
			}
		}

		public int hours
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Hour;
				}
				return Settings.hourSerial;
			}
			set
			{
				SetDateTime(Settings.secSerial, Settings.minSerial, value, Settings.daySerial, Settings.monthSerial, Settings.yearSerial);
			}
		}

		public int days
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Day;
				}
				return Settings.daySerial;
			}
			set
			{
				SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, value, Settings.monthSerial, Settings.yearSerial);
			}
		}

		public int months
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Month;
				}
				return Settings.monthSerial;
			}
			set
			{
				SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial, value, Settings.yearSerial);
			}
		}

		public int years
		{
			get
			{
				if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
				{
					return Settings.date.Year;
				}
				return Settings.yearSerial;
			}
			set
			{
				SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial, Settings.monthSerial, value);
			}
		}

		public void SetDateTime(int sec, int min, int hours, int day, int month, int year)
		{
			if (year == 0)
			{
				year = 1;
			}
			if (month == 0)
			{
				month = 1;
			}
			if (day == 0)
			{
				day = 1;
			}
			Settings.secSerial = sec;
			Settings.minSerial = min;
			Settings.hourSerial = hours;
			Settings.daySerial = day;
			Settings.monthSerial = month;
			Settings.yearSerial = year;
			if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
			{
				DateTime date = new DateTime(1, 1, 1, 0, 0, 0).AddYears(Settings.yearSerial - 1).AddMonths(Settings.monthSerial - 1).AddDays(Settings.daySerial - 1)
					.AddHours(Settings.hourSerial)
					.AddMinutes(Settings.minSerial)
					.AddSeconds(Settings.secSerial);
				if (EnviroManager.instance != null && EnviroManager.instance.Events != null && EnviroManager.instance.notFirstFrame && Application.isPlaying)
				{
					if (Settings.date.Hour != date.Hour)
					{
						EnviroManager.instance.NotifyHourPassed();
					}
					if (Settings.date.Day != date.Day)
					{
						EnviroManager.instance.NotifyDayPassed();
					}
					if (Settings.date.Year != date.Year)
					{
						EnviroManager.instance.NotifyYearPassed();
					}
				}
				Settings.date = date;
				Settings.secSerial = Settings.date.Second;
				Settings.minSerial = Settings.date.Minute;
				Settings.hourSerial = Settings.date.Hour;
				Settings.daySerial = Settings.date.Day;
				Settings.monthSerial = Settings.date.Month;
				Settings.yearSerial = Settings.date.Year;
				Settings.timeOfDay = (float)Settings.date.Hour + (float)Settings.date.Minute * 0.0166667f + (float)Settings.date.Second * 0.000277778f;
				return;
			}
			if (Settings.secSerial >= 60)
			{
				Settings.minSerial++;
				Settings.secSerial = 0;
			}
			if (Settings.minSerial >= 60)
			{
				Settings.hourSerial++;
				Settings.minSerial = 0;
				if (EnviroManager.instance.Events != null && EnviroManager.instance.notFirstFrame && Application.isPlaying)
				{
					EnviroManager.instance.NotifyHourPassed();
				}
			}
			if (Settings.hourSerial >= 24)
			{
				Settings.daySerial++;
				Settings.hourSerial = 0;
				if (EnviroManager.instance.Events != null && EnviroManager.instance.notFirstFrame && Application.isPlaying)
				{
					EnviroManager.instance.NotifyDayPassed();
				}
			}
			if (Settings.daySerial > Settings.daysInMonth)
			{
				Settings.monthSerial++;
				Settings.daySerial = 1;
			}
			if (Settings.monthSerial > Settings.monthsInYear)
			{
				Settings.yearSerial++;
				Settings.monthSerial = 1;
				if (EnviroManager.instance.Events != null && EnviroManager.instance.notFirstFrame && Application.isPlaying)
				{
					EnviroManager.instance.NotifyYearPassed();
				}
			}
			Settings.timeOfDay = (float)hours + (float)minutes * 0.0166667f + (float)seconds * 0.000277778f;
		}

		public override void UpdateModule()
		{
			if (!active)
			{
				return;
			}
			if (Settings.simulate && Application.isPlaying)
			{
				float num = 0f;
				float num2 = 1f;
				num2 = (EnviroManager.instance.isNight ? Settings.nightLengthModifier : Settings.dayLengthModifier);
				num = 0.4f / (Settings.cycleLengthInMinutes * num2);
				num = num * 3600f * Time.deltaTime;
				internalTimeOverflow += num;
				seconds += (int)internalTimeOverflow;
				if (internalTimeOverflow >= 1f)
				{
					internalTimeOverflow -= (int)internalTimeOverflow;
				}
			}
			SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial, Settings.monthSerial, Settings.yearSerial);
			if (Settings.calenderType == EnviroTime.CalenderType.Realistic)
			{
				UpdateSunAndMoonPosition();
			}
			else
			{
				UpdateCustomSunAndMoonPosition();
			}
		}

		public void UpdateSunAndMoonPosition()
		{
			if (EnviroManager.instance == null)
			{
				return;
			}
			float num = 367 * years - 7 * (years + (months + 9) / 12) / 4 + 275 * months / 9 + days - 730530;
			num += GetUniversalTimeOfDay() / 24f;
			float ecl = 23.4393f - 3.563E-07f * num;
			if (EnviroManager.instance.Sky != null)
			{
				if (EnviroManager.instance.Sky.Settings.moonMode == EnviroSky.MoonMode.Simple)
				{
					CalculateSunPosition(num, ecl, simpleMoon: true);
				}
				else
				{
					CalculateSunPosition(num, ecl, simpleMoon: false);
					CalculateMoonPosition(num, ecl);
				}
			}
			else
			{
				CalculateSunPosition(num, ecl, simpleMoon: false);
				CalculateMoonPosition(num, ecl);
			}
			CalculateStarsPosition(LST);
		}

		public void UpdateCustomSunAndMoonPosition()
		{
			if (!(EnviroManager.instance == null))
			{
				EnviroManager.instance.sunRotationX = (Settings.timeOfDay + Settings.customSunOffset) * 15f;
				if (EnviroManager.instance.sunRotationX >= 360f)
				{
					EnviroManager.instance.sunRotationX = 0f;
				}
				if (EnviroManager.instance.sunRotationX < 0f)
				{
					EnviroManager.instance.sunRotationX = 360f + EnviroManager.instance.sunRotationX;
				}
				EnviroManager.instance.sunRotationY = Settings.customSunRotation;
				EnviroManager.instance.moonRotationX = EnviroManager.instance.sunRotationX - 180f;
				if (EnviroManager.instance.moonRotationX >= 360f)
				{
					EnviroManager.instance.moonRotationX = 0f;
				}
				if (EnviroManager.instance.moonRotationX < 0f)
				{
					EnviroManager.instance.moonRotationX = 360f + EnviroManager.instance.moonRotationX;
				}
				EnviroManager.instance.moonRotationY = EnviroManager.instance.sunRotationY;
				EnviroManager.instance.UpdateNonTime();
				EnviroManager.instance.Objects.stars.transform.localRotation = EnviroManager.instance.Objects.sun.transform.localRotation;
				Shader.SetGlobalMatrix("_StarsMatrix", EnviroManager.instance.Objects.stars.transform.worldToLocalMatrix);
			}
		}

		public float GetUniversalTimeOfDay()
		{
			return Settings.timeOfDay - (float)Settings.utcOffset;
		}

		public float GetTimeOfDay()
		{
			return Settings.timeOfDay;
		}

		public double GetDateInHours()
		{
			double num = 0.0;
			if (Settings.calenderType == EnviroTime.CalenderType.Custom)
			{
				return Settings.timeOfDay + (float)days * 24f + (float)(months * Settings.daysInMonth) * 24f + (float)(years * (Settings.monthsInYear * Settings.daysInMonth)) * 24f;
			}
			return Settings.timeOfDay + (float)days * 24f + (float)(years * 365) * 24f;
		}

		public string GetTimeStringWithSeconds()
		{
			return $"{hours:00}:{minutes:00}:{seconds:00}";
		}

		public string GetTimeString()
		{
			return $"{hours:00}:{minutes:00}";
		}

		public void SetTimeOfDay(float tod)
		{
			Settings.timeOfDay = tod;
			hours = (int)tod;
			tod -= (float)hours;
			minutes = (int)(tod * 60f);
			tod -= (float)minutes * 0.0166667f;
			seconds = (int)(tod * 3600f);
		}

		public Vector3 OrbitalToLocal(float theta, float phi)
		{
			float num = Mathf.Sin(theta);
			float y = Mathf.Cos(theta);
			float num2 = Mathf.Sin(phi);
			float num3 = Mathf.Cos(phi);
			Vector3 result = default(Vector3);
			result.z = num * num3;
			result.y = y;
			result.x = num * num2;
			return result;
		}

		public float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public void CalculateSunPosition(float d, float ecl, bool simpleMoon)
		{
			float num = 282.9404f + 4.70935E-05f * d;
			float num2 = 0.016709f - 1.151E-09f * d;
			float num3;
			for (num3 = 356.047f + 0.98560023f * d; num3 > 360f; num3 -= 360f)
			{
			}
			for (; num3 < 0f; num3 += 360f)
			{
			}
			float num4 = num3 + num2 * 57.29578f * Mathf.Sin((float)Math.PI / 180f * num3) * (1f + num2 * Mathf.Cos((float)Math.PI / 180f * num3));
			float num5 = Mathf.Cos((float)Math.PI / 180f * num4) - num2;
			float num6 = Mathf.Sin((float)Math.PI / 180f * num4) * Mathf.Sqrt(1f - num2 * num2);
			float num7 = 57.29578f * Mathf.Atan2(num6, num5);
			float num8 = Mathf.Sqrt(num5 * num5 + num6 * num6);
			float num9 = num7 + num;
			float num10 = num8 * Mathf.Cos((float)Math.PI / 180f * num9);
			float num11 = num8 * Mathf.Sin((float)Math.PI / 180f * num9);
			float num12 = num10;
			float num13 = num11 * Mathf.Cos((float)Math.PI / 180f * ecl);
			float f = Mathf.Atan2(num11 * Mathf.Sin((float)Math.PI / 180f * ecl), Mathf.Sqrt(num12 * num12 + num13 * num13));
			float num14 = Mathf.Sin(f);
			float num15 = Mathf.Cos(f);
			float num16 = num3 + num + 180f + GetUniversalTimeOfDay() * 15f;
			for (LST = num16 + Settings.longitude; LST > 360f; LST -= 360f)
			{
			}
			while (LST < 0f)
			{
				LST += 360f;
			}
			float num17 = LST - 57.29578f * Mathf.Atan2(num13, num12);
			float f2 = (float)Math.PI / 180f * num17;
			float num18 = Mathf.Sin(f2);
			float num19 = Mathf.Cos(f2) * num15;
			float num20 = num18 * num15;
			float num21 = num14;
			float num22 = Mathf.Sin((float)Math.PI / 180f * Settings.latitude);
			float num23 = Mathf.Cos((float)Math.PI / 180f * Settings.latitude);
			float num24 = num19 * num22 - num21 * num23;
			float num25 = num20;
			float y = num19 * num23 + num21 * num22;
			float num26 = Mathf.Atan2(num25, num24) + (float)Math.PI;
			float num27 = Mathf.Atan2(y, Mathf.Sqrt(num24 * num24 + num25 * num25));
			float num28 = (float)Math.PI / 2f - num27;
			float phi = num26;
			EnviroManager.instance.solarTime = Mathf.Clamp01(Remap(num28, -1.5f, 0f, 1.5f, 1f));
			EnviroManager.instance.Objects.sun.transform.localPosition = OrbitalToLocal(num28, phi);
			EnviroManager.instance.Objects.sun.transform.LookAt(EnviroManager.instance.transform);
			if (simpleMoon)
			{
				EnviroManager.instance.Objects.moon.transform.localPosition = OrbitalToLocal(num28 - (float)Math.PI, phi);
				EnviroManager.instance.lunarTime = Mathf.Clamp01(Remap(num28 - (float)Math.PI, -3f, 0f, 0f, 1f));
				EnviroManager.instance.Objects.moon.transform.LookAt(EnviroManager.instance.transform);
			}
		}

		public void CalculateMoonPosition(float d, float ecl)
		{
			float num = 125.1228f - 0.05295381f * d;
			float num2 = 5.1454f;
			float num3 = 318.0634f + 0.16435732f * d;
			float num4 = 0.0549f;
			float num5 = 115.3654f + 13.064993f * d;
			float num6 = (float)Math.PI / 180f * num5;
			float f = num6 + num4 * Mathf.Sin(num6) * (1f + num4 * Mathf.Cos(num6));
			float num7 = 60.2666f * (Mathf.Cos(f) - num4);
			float num8 = 60.2666f * (Mathf.Sqrt(1f - num4 * num4) * Mathf.Sin(f));
			float num9 = 57.29578f * Mathf.Atan2(num8, num7);
			float num10 = Mathf.Sqrt(num7 * num7 + num8 * num8);
			float f2 = (float)Math.PI / 180f * num;
			float num11 = Mathf.Sin(f2);
			float num12 = Mathf.Cos(f2);
			float f3 = (float)Math.PI / 180f * (num9 + num3);
			float num13 = Mathf.Sin(f3);
			float num14 = Mathf.Cos(f3);
			float f4 = (float)Math.PI / 180f * num2;
			float num15 = Mathf.Cos(f4);
			float num16 = num10 * (num12 * num14 - num11 * num13 * num15);
			float num17 = num10 * (num11 * num14 + num12 * num13 * num15);
			float num18 = num10 * (num13 * Mathf.Sin(f4));
			float num19 = Mathf.Cos((float)Math.PI / 180f * ecl);
			float num20 = Mathf.Sin((float)Math.PI / 180f * ecl);
			float num21 = num16;
			float num22 = num17 * num19 - num18 * num20;
			float y = num17 * num20 + num18 * num19;
			float num23 = Mathf.Atan2(num22, num21);
			float f5 = Mathf.Atan2(y, Mathf.Sqrt(num21 * num21 + num22 * num22));
			float f6 = (float)Math.PI / 180f * LST - num23;
			float num24 = Mathf.Cos(f6) * Mathf.Cos(f5);
			float num25 = Mathf.Sin(f6) * Mathf.Cos(f5);
			float num26 = Mathf.Sin(f5);
			float f7 = (float)Math.PI / 180f * Settings.latitude;
			float num27 = Mathf.Sin(f7);
			float num28 = Mathf.Cos(f7);
			float num29 = num24 * num27 - num26 * num28;
			float num30 = num25;
			float y2 = num24 * num28 + num26 * num27;
			float num31 = Mathf.Atan2(num30, num29) + (float)Math.PI;
			float num32 = Mathf.Atan2(y2, Mathf.Sqrt(num29 * num29 + num30 * num30));
			float num33 = (float)Math.PI / 2f - num32;
			float phi = num31;
			EnviroManager.instance.Objects.moon.transform.localPosition = OrbitalToLocal(num33, phi);
			EnviroManager.instance.lunarTime = Mathf.Clamp01(Remap(num33, -1.5f, 0f, 1.5f, 1f));
			EnviroManager.instance.Objects.moon.transform.LookAt(EnviroManager.instance.transform.position);
		}

		public void CalculateStarsPosition(float siderealTime)
		{
			Quaternion localRotation = Quaternion.AngleAxis(90f - Settings.latitude, Vector3.right) * Quaternion.AngleAxis(180f + siderealTime, Vector3.up);
			EnviroManager.instance.Objects.stars.transform.localRotation = localRotation;
			Shader.SetGlobalMatrix("_StarsMatrix", EnviroManager.instance.Objects.stars.transform.worldToLocalMatrix);
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroTime>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroTimeModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroTime>(JsonUtility.ToJson(Settings));
		}
	}
}
