using Features.GameJournalingModule.Scripts.AnalyticsMarkService;
using GameAnalyticsSDK;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts
{
	public class GameAnalyticsMarkUserService : IAnalyticsMarkUserService
	{
		public void MarkAsDebugBuild()
		{
			GameAnalytics.SetCustomDimension01("debug_build");
		}

		public void MarkAsReleaseBuild()
		{
			GameAnalytics.SetCustomDimension01("release_build");
		}

		public void MarkABGroup(string groupName)
		{
			if (!(groupName == "Default"))
			{
				if (groupName == "B")
				{
					GameAnalytics.SetCustomDimension01("b_group");
				}
				else
				{
					Debug.LogError("Cannot mark AB group " + groupName);
				}
			}
			else
			{
				GameAnalytics.SetCustomDimension01("default_group");
			}
		}

		public void MarkAsDeveloper()
		{
			GameAnalytics.SetCustomDimension02("developer");
		}

		public void MarkAsCheater()
		{
			GameAnalytics.SetCustomDimension03("cheater");
		}

		public void MarkAsNeverVisitedInteractiveMine()
		{
			GameAnalytics.SetCustomDimension03("never_visited_interactive_mine");
		}

		public void MarkAsVisitedInteractiveMine()
		{
			GameAnalytics.SetCustomDimension03("visited_interactive_mine");
		}

		public void MarkAsFinishedInteractiveMine()
		{
			GameAnalytics.SetCustomDimension03("finished_interactive_mine");
		}
	}
}
