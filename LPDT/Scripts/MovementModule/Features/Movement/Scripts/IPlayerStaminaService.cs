namespace Features.Movement.Scripts
{
	public interface IPlayerStaminaService
	{
		void SubtractStaminaLogic(float subtractTime, float staminaDrainRate);
	}
}
