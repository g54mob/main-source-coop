using System;
using System.Collections.Generic;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialTargetTrackerDataHolder
	{
		private readonly Dictionary<BaseTutorialTargetType, BaseTutorialTargetReachTracker> _targetReachTrackers = new Dictionary<BaseTutorialTargetType, BaseTutorialTargetReachTracker>();

		public IReadOnlyDictionary<BaseTutorialTargetType, BaseTutorialTargetReachTracker> TargetReachTrackers => _targetReachTrackers;

		public event Action<BaseTutorialTargetReachTracker> OnTargetReachTrackerAdded;

		public void AddTargetReachTracker(BaseTutorialTargetReachTracker tracker)
		{
			_targetReachTrackers.Add(tracker.TargetType, tracker);
			this.OnTargetReachTrackerAdded?.Invoke(tracker);
		}

		public void RemoveTargetReachTracker(BaseTutorialTargetReachTracker tracker)
		{
			_targetReachTrackers.Remove(tracker.TargetType);
		}
	}
}
