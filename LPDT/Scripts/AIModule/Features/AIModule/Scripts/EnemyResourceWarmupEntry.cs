using System;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class EnemyResourceWarmupEntry
	{
		public EnemyType EnemyType;

		public GameObject WarmupPrefab;
	}
}
