using System;

namespace NWH.Common.Utility
{
	public static class UnitConverter
	{
		public static float Inch_To_Meter(float inch)
		{
			return inch * 0.0254f;
		}

		public static float Meter_To_Inch(float meters)
		{
			return meters * 39.3701f;
		}

		public static float KmlToL100km(float kml)
		{
			if (kml != 0f)
			{
				return 100f / kml;
			}
			return 1f / 0f;
		}

		public static float KmlToMpg(float kml)
		{
			return kml * 2.825f;
		}

		public static float L100kmToKml(float l100km)
		{
			if (l100km != 0f)
			{
				return 100f / l100km;
			}
			return 0f;
		}

		public static float L100kmToMpg(float l100km)
		{
			if (l100km != 0f)
			{
				return 282.5f / l100km;
			}
			return 0f;
		}

		public static float AngularVelocityToRPM(float angularVelocity)
		{
			return angularVelocity * (30f / (float)Math.PI);
		}

		public static float RPMToAngularVelocity(float RPM)
		{
			return RPM * ((float)Math.PI / 30f);
		}

		public static float MpgToKml(float mpg)
		{
			return mpg * 0.354f;
		}

		public static float MpgToL100km(float mpg)
		{
			if (mpg != 0f)
			{
				return 282.5f / mpg;
			}
			return 1f / 0f;
		}

		public static float MphToKph(float value)
		{
			return value * 1.60934f;
		}

		public static float MpsToKph(float value)
		{
			return value * 3.6f;
		}

		public static float MpsToMph(float value)
		{
			return value * 2.23694f;
		}

		public static float Speed_kmhToMph(float kmh)
		{
			return kmh * 0.621371f;
		}

		public static float Speed_kmhToMs(float kmh)
		{
			return kmh * 0.277778f;
		}

		public static float Speed_mphToKmh(float mph)
		{
			return mph * 1.60934f;
		}

		public static float Speed_mphToMs(float mph)
		{
			return mph * 0.44704f;
		}

		public static float Speed_msToKph(float ms)
		{
			return ms * 3.6f;
		}

		public static float Speed_msToMph(float ms)
		{
			return ms * 2.23694f;
		}
	}
}
