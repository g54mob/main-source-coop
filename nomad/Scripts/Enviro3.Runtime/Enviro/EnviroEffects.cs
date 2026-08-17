using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroEffects
	{
		public enum EnviroEffectSystemType
		{
			ParticleSystem = 0,
			VFXGraph = 1,
			Both = 2
		}

		public EnviroEffectSystemType enviroEffectSystemType = EnviroEffectSystemType.VFXGraph;

		public List<EnviroEffectTypes> effectTypes = new List<EnviroEffectTypes>();

		[Range(0f, 2f)]
		public float particeEmissionRateModifier = 1f;
	}
}
