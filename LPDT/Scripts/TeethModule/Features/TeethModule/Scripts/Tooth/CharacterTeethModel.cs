using System.Collections.Generic;

namespace Features.TeethModule.Scripts.Tooth
{
	public class CharacterTeethModel
	{
		public readonly Dictionary<int, Dictionary<int, PlayerTooth>> CharacterToToothCustomizations = new Dictionary<int, Dictionary<int, PlayerTooth>>();

		public void RegisterCharacterTooth(int owner, int toothIndex, PlayerTooth tooth)
		{
			if (!CharacterToToothCustomizations.TryGetValue(owner, out var value))
			{
				value = new Dictionary<int, PlayerTooth>();
				CharacterToToothCustomizations.Add(owner, value);
			}
			value[toothIndex] = tooth;
		}
	}
}
