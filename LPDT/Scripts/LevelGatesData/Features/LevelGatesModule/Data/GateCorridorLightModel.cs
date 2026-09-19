using System.Collections.Generic;

namespace Features.LevelGatesModule.Data
{
	public class GateCorridorLightModel
	{
		private readonly HashSet<IGateCorridorLightPresetTarget> _registeredLights = new HashSet<IGateCorridorLightPresetTarget>();

		public IEnumerable<IGateCorridorLightPresetTarget> RegisteredLights => _registeredLights;

		public void Register(IGateCorridorLightPresetTarget light)
		{
			if (light != null)
			{
				_registeredLights.Add(light);
			}
		}

		public void Unregister(IGateCorridorLightPresetTarget light)
		{
			if (light != null)
			{
				_registeredLights.Remove(light);
			}
		}
	}
}
