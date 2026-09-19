using System.Collections.Generic;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachMaterialSwapService
	{
		BeachMaterialSwapHandle Apply(IReadOnlyList<BeachMaterialSwap> swaps);

		void Restore(BeachMaterialSwapHandle handle);
	}
}
