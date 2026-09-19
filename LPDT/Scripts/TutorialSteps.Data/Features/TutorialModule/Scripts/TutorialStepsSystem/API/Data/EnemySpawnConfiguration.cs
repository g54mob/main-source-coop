using System;
using Features.AIModule.Scripts;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data
{
	[Serializable]
	public class EnemySpawnConfiguration
	{
		public EnemyType EnemyType;

		public int Count;
	}
}
