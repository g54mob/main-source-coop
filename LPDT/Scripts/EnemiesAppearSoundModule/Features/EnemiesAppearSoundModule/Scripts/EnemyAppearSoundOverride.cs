using System;
using FMODUnity;
using Features.AIModule.Scripts;
using UnityEngine;

namespace Features.EnemiesAppearSoundModule.Scripts
{
	[Serializable]
	public class EnemyAppearSoundOverride
	{
		[field: SerializeField]
		public EnemyType EnemyType { get; private set; }

		[field: SerializeField]
		public EventReference Sound { get; private set; }
	}
}
