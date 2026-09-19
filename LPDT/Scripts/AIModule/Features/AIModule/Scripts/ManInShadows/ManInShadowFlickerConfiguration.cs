using UnityEngine;

namespace Features.AIModule.Scripts.ManInShadows
{
	[CreateAssetMenu(fileName = "ManInShadowFlickerConfiguration_Default", menuName = "Configurations/PostProcessing/ManInShadowFlickerConfiguration")]
	public class ManInShadowFlickerConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float MaxDistanceForBlend { get; private set; }
	}
}
