using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.FogModule.Scripts
{
	public class FogRegionsModel : ISessionCleanup
	{
		private readonly List<FogRegion> _fogRegions = new List<FogRegion>();

		public bool DebugFogTransitionEnabled;

		public IReadOnlyList<FogRegion> FogRegions => _fogRegions;

		public FogBlend CurrentBlend { get; private set; }

		public void AddFogRegion(FogRegion fogRegion)
		{
			_fogRegions.Add(fogRegion);
		}

		public void RemoveFogRegion(FogRegion fogRegion)
		{
			_fogRegions.Remove(fogRegion);
		}

		public void SetCurrentBlend(FogBlend fogBlend)
		{
			CurrentBlend = fogBlend;
		}

		public void Cleanup()
		{
			_fogRegions.Clear();
			CurrentBlend = default(FogBlend);
		}
	}
}
