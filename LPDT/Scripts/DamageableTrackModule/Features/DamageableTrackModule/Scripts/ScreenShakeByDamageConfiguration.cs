using Features.ScreenShakeModule.Scripts;
using UnityEngine;

namespace Features.DamageableTrackModule.Scripts
{
	[CreateAssetMenu(fileName = "ScreenShakeByDamageConfiguration_Default", menuName = "Configurations/DamageableTrackModule/ScreenShakeByDamageConfiguration")]
	public class ScreenShakeByDamageConfiguration : ScriptableObject
	{
		public ScreenShakeData ScreenShakeDataByDamage;
	}
}
