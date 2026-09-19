using System.Collections.Generic;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachGateCorridorLightService
	{
		void Apply(IReadOnlyList<BeachGateCorridorLightSetting> settings);
	}
}
