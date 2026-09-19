using System.Collections.Generic;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public class MimicInterestPointsModel
	{
		private readonly List<MimicInterestPoint> _navigableInterestPoints = new List<MimicInterestPoint>();

		public IReadOnlyList<MimicInterestPoint> NavigableInterestPoints => _navigableInterestPoints;

		public void RegisterInterestPoint(MimicInterestPoint navigableInterestPoint)
		{
			_navigableInterestPoints.Add(navigableInterestPoint);
		}

		public void UnregisterInterestPoint(MimicInterestPoint navigableInterestPoint)
		{
			_navigableInterestPoints.Remove(navigableInterestPoint);
		}
	}
}
