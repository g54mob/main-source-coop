using System;
using FMODUnity;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	[Serializable]
	public class SoundOcclusionDistanceOverride
	{
		[field: SerializeField]
		public EventReference Sound { get; private set; }

		[field: SerializeField]
		public float AudibleDistance { get; private set; } = 15f;
	}
}
