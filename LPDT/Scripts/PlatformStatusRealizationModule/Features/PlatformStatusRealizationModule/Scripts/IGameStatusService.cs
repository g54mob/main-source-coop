namespace Features.PlatformStatusRealizationModule.Scripts
{
	public interface IGameStatusService
	{
		void SetGameStatus(GameStatusId gameStatusId);

		void SetGameStatusParameter(GameStatusParameterId gameStatusParameterId, string parameterValue);
	}
}
