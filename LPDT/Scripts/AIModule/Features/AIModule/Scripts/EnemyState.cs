namespace Features.AIModule.Scripts
{
	public enum EnemyState
	{
		None = 0,
		Wandering = 1,
		Chasing = 2,
		ChasingStart = 14,
		Attacking = 3,
		RunAway = 4,
		PlayerAttacking = 5,
		Stun = 6,
		RangeAttacking = 7,
		RunAround = 8,
		InteractingWaiting = 9,
		Eating = 10,
		MoveToPoint = 11,
		Stealing = 12,
		Slowed = 13,
		RageStart = 15,
		Rage = 16,
		RageInteracted = 17,
		RageEnd = 18,
		LowAttacking = 19,
		SnappedToPlayer = 20,
		Fear = 21
	}
}
