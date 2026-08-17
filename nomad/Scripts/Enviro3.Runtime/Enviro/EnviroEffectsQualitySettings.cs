using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroEffectsQualitySettings
	{
		[Range(0f, 2f)]
		public float particeEmissionRateModifier = 1f;
	}
}
