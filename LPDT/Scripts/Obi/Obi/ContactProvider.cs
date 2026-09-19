using Unity.Collections;

namespace Obi
{
	public struct ContactProvider : IConstraintProvider
	{
		public NativeArray<BurstContact> contacts;

		public NativeArray<BurstContact> sortedContacts;

		public NativeArray<int> simplices;

		public SimplexCounts simplexCounts;

		public int GetConstraintCount()
		{
			return contacts.Length;
		}

		public int GetParticleCount(int constraintIndex)
		{
			simplexCounts.GetSimplexStartAndSize(contacts[constraintIndex].bodyA, out var size);
			simplexCounts.GetSimplexStartAndSize(contacts[constraintIndex].bodyB, out var size2);
			return size + size2;
		}

		public int GetParticle(int constraintIndex, int index)
		{
			int size;
			int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(contacts[constraintIndex].bodyA, out size);
			int size2;
			int simplexStartAndSize2 = simplexCounts.GetSimplexStartAndSize(contacts[constraintIndex].bodyB, out size2);
			if (index < size)
			{
				return simplices[simplexStartAndSize + index];
			}
			return simplices[simplexStartAndSize2 + index - size];
		}

		public void WriteSortedConstraint(int constraintIndex, int sortedIndex)
		{
			sortedContacts[sortedIndex] = contacts[constraintIndex];
		}
	}
}
