using System;
using UnityEngine;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	[Serializable]
	public abstract class BoosterSettingsBase
	{
		[field: SerializeField]
		public float BoosterLifeTime { get; private set; }

		[field: SerializeField]
		public float StartedBoosterLifeTime { get; private set; }

		[field: SerializeField]
		public Sprite BoosterIcon { get; private set; }

		public virtual string GetIdentifier()
		{
			return GetType().Name;
		}
	}
}
