using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroTime
	{
		public enum CalenderType
		{
			Realistic = 0,
			Custom = 1
		}

		public bool simulate;

		public DateTime date = new DateTime(1, 1, 1, 0, 0, 0);

		[SerializeField]
		public int secSerial;

		[SerializeField]
		public int minSerial;

		[SerializeField]
		public int hourSerial;

		[SerializeField]
		public int daySerial;

		[SerializeField]
		public int monthSerial;

		[SerializeField]
		public int yearSerial;

		public float timeOfDay;

		[Range(-90f, 90f)]
		[Tooltip("-90,  90   Horizontal earth lines")]
		public float latitude;

		[Range(-180f, 180f)]
		[Tooltip("-180, 180  Vertical earth line")]
		public float longitude;

		[Range(-13f, 13f)]
		[Tooltip("Time offset for timezones")]
		public int utcOffset;

		[Tooltip("Realtime minutes for a 24h game time cycle.")]
		public float cycleLengthInMinutes = 10f;

		[Tooltip("Day length modifier will increase/decrease time progression speed at daytime.")]
		[Range(0.1f, 10f)]
		public float dayLengthModifier = 1f;

		[Tooltip("Night length modifier will increase/decrease time progression speed at nighttime.")]
		[Range(0.1f, 10f)]
		public float nightLengthModifier = 1f;

		public CalenderType calenderType;

		public int daysInMonth = 10;

		public int monthsInYear = 4;

		public float customSunOffset = -8f;

		[Range(0f, 360f)]
		public float customSunRotation;
	}
}
