namespace Features.StruggleBarModule.Scripts
{
	public interface IStruggleBarService
	{
		bool IsActive { get; }

		void Start();

		void Start(float drainPerSecond, float boostPerPress, float startNormalized);

		void Cancel();
	}
}
