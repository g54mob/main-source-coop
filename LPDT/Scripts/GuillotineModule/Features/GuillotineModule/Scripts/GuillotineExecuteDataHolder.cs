using System.Collections.Generic;

namespace Features.GuillotineModule.Scripts
{
	public class GuillotineExecuteDataHolder
	{
		public List<IGuillotineExecutable> GuillotineExecutablesInRange { get; private set; } = new List<IGuillotineExecutable>();
	}
}
