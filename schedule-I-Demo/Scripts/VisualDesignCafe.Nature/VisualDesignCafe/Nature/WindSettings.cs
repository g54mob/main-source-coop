using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VisualDesignCafe.Nature
{
	[Serializable]
	public struct WindSettings
	{
		[FormerlySerializedAs("GustDirection")]
		public Vector2 WindDirection;

		[FormerlySerializedAs("GustStrength")]
		[Range(0f, 1f)]
		public float WindStrength;

		[FormerlySerializedAs("GustSpeed")]
		[Range(0.5f, 1f)]
		public float WindSpeed;

		[FormerlySerializedAs("ShiverStrength")]
		[Range(0f, 1f)]
		public float Turbulence;

		public static WindSettings None => new WindSettings
		{
			WindDirection = new Vector2(0.7f, 0.3f),
			WindStrength = 0f,
			Turbulence = 0f,
			WindSpeed = 0.5f
		};

		public static WindSettings Calm => new WindSettings
		{
			WindDirection = new Vector2(0.7f, 0.3f),
			WindStrength = 0.05f,
			Turbulence = 0.05f,
			WindSpeed = 0.5f
		};

		public static WindSettings Breeze => new WindSettings
		{
			WindDirection = new Vector2(0.7f, 0.3f),
			WindStrength = 0.2f,
			Turbulence = 0.2f,
			WindSpeed = 0.5f
		};

		public static WindSettings StrongBreeze => new WindSettings
		{
			WindDirection = new Vector2(0.7f, 0.3f),
			WindStrength = 0.5f,
			Turbulence = 0.5f,
			WindSpeed = 0.75f
		};

		public static WindSettings Storm => new WindSettings
		{
			WindDirection = new Vector2(0.7f, 0.3f),
			WindStrength = 1f,
			Turbulence = 1f,
			WindSpeed = 1f
		};

		public static WindSettings FromWindZone(WindZone windZone)
		{
			return new WindSettings
			{
				WindStrength = windZone.windMain * 0.2f,
				WindSpeed = windZone.windPulseFrequency,
				Turbulence = windZone.windTurbulence * 0.2f,
				WindDirection = RotationToDirection(windZone.transform.rotation)
			};
		}

		public static Vector2 RotationToDirection(Quaternion quaternion)
		{
			float y = quaternion.eulerAngles.y;
			return new Vector2(Mathf.Sin(y * (MathF.PI / 180f)), Mathf.Cos(y * (MathF.PI / 180f))).normalized;
		}

		public WindSettings(WindSettings other)
		{
			WindDirection = other.WindDirection;
			WindStrength = other.WindStrength;
			WindSpeed = other.WindSpeed;
			Turbulence = other.Turbulence;
		}

		public WindSettings(Vector2 gustDirection, float windStrength, float windSpeed, float turbulence)
		{
			WindDirection = gustDirection;
			WindStrength = windStrength;
			WindSpeed = windSpeed;
			Turbulence = turbulence;
		}

		public void Apply(Texture2D gustNoise)
		{
			Shader.SetGlobalTexture("g_GustNoise", gustNoise);
			Apply();
		}

		public void Apply()
		{
		}

		public void ApplyToWindZone(WindZone zone)
		{
			zone.windMain = WindStrength * 5f;
			zone.windPulseFrequency = WindSpeed;
			zone.windTurbulence = Turbulence * 5f;
		}
	}
}
