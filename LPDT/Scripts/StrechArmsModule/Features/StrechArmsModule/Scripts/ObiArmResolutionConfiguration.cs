using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[CreateAssetMenu(fileName = "ObiArmResolutionConfiguration_Default", menuName = "Configurations/StrechArm/ObiArmResolutionConfiguration")]
	public class ObiArmResolutionConfiguration : ScriptableObject
	{
		[SerializeField]
		private ObiArmResolutionAuthorityConfiguration _authority;

		[SerializeField]
		private ObiArmResolutionAuthorityConfiguration _nonAuthority;

		public ObiArmResolutionAuthorityConfiguration Authority => _authority;

		public ObiArmResolutionAuthorityConfiguration NonAuthority => _nonAuthority;
	}
}
