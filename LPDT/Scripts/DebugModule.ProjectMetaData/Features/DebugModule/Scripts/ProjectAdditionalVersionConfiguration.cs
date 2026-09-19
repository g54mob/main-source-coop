using UnityEngine;

namespace Features.DebugModule.Scripts
{
	[CreateAssetMenu(fileName = "ProjectAdditionalVersionConfiguration_Default", menuName = "Configurations/DebugModule/ProjectAdditionalVersionConfiguration")]
	public class ProjectAdditionalVersionConfiguration : ScriptableObject
	{
		public string AdditionalVersion;
	}
}
