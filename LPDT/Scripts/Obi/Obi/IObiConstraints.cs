namespace Obi
{
	public interface IObiConstraints
	{
		int batchCount { get; }

		Oni.ConstraintType? GetConstraintType();

		IObiConstraintsBatch GetBatch(int i);

		void Clear();

		bool AddToSolver(ObiSolver solver);

		bool RemoveFromSolver();

		int GetConstraintCount();

		int GetActiveConstraintCount();

		void ActivateAllConstraints();

		void DeactivateAllConstraints();

		void Merge(ObiActor actor, IObiConstraints other);
	}
}
