using System;
using UnityEngine;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	[Serializable]
	public class PlatformGameStatusParameterData
	{
		[field: SerializeField]
		public string Property { get; private set; }
	}
}
