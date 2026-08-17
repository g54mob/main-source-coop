using UnityEngine;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Stat Setting", fileName = "PlayerStatSetting")]
	public class PlayerStatSetting : ScriptableObject
	{
		public float maxValue;

		public float defaultConsumeSpeed;

		public float criticalLowThreshold;

		public float sleepingConsumeSpeedMultiplier;
	}
}
