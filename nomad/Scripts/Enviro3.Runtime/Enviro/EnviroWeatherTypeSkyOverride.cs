using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeSkyOverride
	{
		public float intensity = 1f;

		public float mieScatteringMultiplier = 1f;

		public float skyColorExponent = 1f;

		public Color skyColorTint = Color.white;
	}
}
