namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public enum HeadmanVisualState : byte
	{
		None = 0,
		Wandering = 1,
		ChasingStart = 2,
		Chasing = 3,
		AttackingBase = 4,
		AttackingLow = 5,
		Rage = 6,
		RageInteracted = 7,
		RageEnd = 8,
		Fear = 9,
		Slowed = 10,
		Dead = 11
	}
}
