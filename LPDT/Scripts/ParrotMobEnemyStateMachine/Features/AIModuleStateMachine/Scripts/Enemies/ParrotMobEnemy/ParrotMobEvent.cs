namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	public enum ParrotMobEvent
	{
		None = 0,
		OnPlayerDetected = 1,
		OnAlertCompleted = 2,
		OnScreamCompleted = 3,
		OnPlayerLost = 4,
		OnRepeatScream = 5,
		OnDamage = 6
	}
}
