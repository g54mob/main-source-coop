namespace Obi
{
	public interface IComputeConstraintsImpl : IConstraints
	{
		void Initialize(float stepTime, float substepTime, int steps, float timeLeft);

		void Project(float stepTime, float substepTime, int substeps, float timeLeft);

		void Dispose();

		IConstraintsBatchImpl CreateConstraintsBatch();

		void RemoveBatch(IConstraintsBatchImpl batch);
	}
}
