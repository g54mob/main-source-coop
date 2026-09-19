using Unity.Jobs;

namespace Obi
{
	public interface IBurstConstraintsImpl : IConstraints
	{
		JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft);

		JobHandle Project(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft);

		void Dispose();

		IConstraintsBatchImpl CreateConstraintsBatch();

		void RemoveBatch(IConstraintsBatchImpl batch);
	}
}
