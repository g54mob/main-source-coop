using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.QuotaModule.Scripts
{
	[CreateAssetMenu(fileName = "ChapterResumeRewardConfiguration_Default", menuName = "Configurations/QuotaModule/ChapterResumeRewardConfiguration")]
	public class ChapterResumeRewardConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<LevelType, int> CoinsByLevel { get; private set; }
	}
}
