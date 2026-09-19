using System;
using UnityEngine;

namespace Global.Modules.DatabaseModule.Scripts.Generated
{
	[Serializable]
	public class EntityDataHolder
	{
		[field: SerializeField]
		public Entity ID { get; private set; }

		[field: SerializeField]
		public int HP { get; private set; }

		[field: SerializeField]
		public float Speed { get; private set; }
	}
}
