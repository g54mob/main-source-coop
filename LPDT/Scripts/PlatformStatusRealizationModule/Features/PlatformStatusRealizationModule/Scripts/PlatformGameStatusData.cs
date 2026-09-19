using System;
using UnityEngine;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	[Serializable]
	public class PlatformGameStatusData
	{
		[field: SerializeField]
		public string Property { get; private set; }

		[field: SerializeField]
		public string Value { get; private set; }
	}
}
