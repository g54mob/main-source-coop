using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroAuroraQualitySettings
	{
		public bool aurora = true;

		[Range(6f, 32f)]
		public int steps = 32;
	}
}
