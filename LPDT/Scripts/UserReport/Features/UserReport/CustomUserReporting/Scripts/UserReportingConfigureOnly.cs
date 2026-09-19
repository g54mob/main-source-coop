using Features.UserReport.CustomUserReporting.Scripts.Plugin;
using UnityEngine;

namespace Features.UserReport.CustomUserReporting.Scripts
{
	public class UserReportingConfigureOnly : MonoBehaviour
	{
		private void Start()
		{
			if (UnityUserReporting.CurrentClient == null)
			{
				UnityUserReporting.Configure();
			}
		}
	}
}
