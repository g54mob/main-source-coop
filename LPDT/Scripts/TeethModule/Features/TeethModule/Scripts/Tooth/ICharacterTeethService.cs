using System.Collections.Generic;

namespace Features.TeethModule.Scripts.Tooth
{
	public interface ICharacterTeethService
	{
		void RefreshAllTooth();

		void RefreshTooth(int toothId);

		void RemoveTooth(List<int> toothId, bool spawnObject);

		void RemoveRandomTooth(int count, bool spawnObject);

		void RemoveTooth(int toothId, bool spawnObject);

		void RemoveAllTooth(bool spawnObject);
	}
}
