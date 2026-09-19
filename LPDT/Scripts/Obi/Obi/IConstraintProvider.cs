namespace Obi
{
	public interface IConstraintProvider
	{
		int GetConstraintCount();

		int GetParticleCount(int constraintIndex);

		int GetParticle(int constraintIndex, int index);

		void WriteSortedConstraint(int constraintIndex, int sortedIndex);
	}
}
