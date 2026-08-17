using System;
using UnityEngine;

namespace NomadDrive.Features.GameTime
{
	[CreateAssetMenu(menuName = "NomadDrive/GameTime/Seeded Time Config", fileName = "SeededTimeConfig")]
	public class SeededTimeConfig : ScriptableObject
	{
		[Tooltip("Earliest possible random starting hour (0-24, e.g. 6 = 06:00).")]
		[Range(0f, 24f)]
		public float minStartHour = 6f;

		[Tooltip("Latest possible random starting hour (0-24, e.g. 15 = 15:00). Inclusive when whole-hours-only.")]
		[Range(0f, 24f)]
		public float maxStartHour = 15f;

		[Tooltip("If true, snap the drawn time to a whole hour (e.g. 14:00 instead of 14:37).")]
		public bool wholeHoursOnly = true;

		[Tooltip("Sub-seed identifier passed to SeedManager.CreateRandom. Keep stable — it defines the draw. Distinct from the weather config's identifier so time and weather draw independently.")]
		public string seedIdentifier = "InitialTimeOfDay";

		public float PickStartHour(System.Random rng)
		{
			float num = Mathf.Clamp(Mathf.Min(minStartHour, maxStartHour), 0f, 24f);
			float num2 = Mathf.Clamp(Mathf.Max(minStartHour, maxStartHour), 0f, 24f);
			if (wholeHoursOnly)
			{
				return Mathf.Clamp(rng.Next(Mathf.FloorToInt(num), Mathf.FloorToInt(num2) + 1), 0, 23);
			}
			return Mathf.Clamp(num + (float)rng.NextDouble() * (num2 - num), 0f, 23.999f);
		}
	}
}
