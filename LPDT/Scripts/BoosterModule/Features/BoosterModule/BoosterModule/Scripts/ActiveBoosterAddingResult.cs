namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class ActiveBoosterAddingResult
	{
		public bool IsSuccess { get; }

		public ActiveBoosterAddingResultType ResultType { get; }

		public ActiveBoosterAddingResult(bool isSuccess, ActiveBoosterAddingResultType resultType)
		{
			IsSuccess = isSuccess;
			ResultType = resultType;
		}
	}
}
