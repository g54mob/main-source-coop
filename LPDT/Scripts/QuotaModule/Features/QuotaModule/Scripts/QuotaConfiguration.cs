using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.QuotaModule.Scripts
{
	[CreateAssetMenu(fileName = "QuotaConfiguration_Default", menuName = "Configurations/QuotaModule/QuotaConfiguration")]
	public class QuotaConfiguration : ScriptableObject
	{
		public float CurrencyQuotaCoefficient;

		[field: SerializeField]
		public SerializableDictionary<LevelType, float> QuotaPercentByLevel { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, float> ExplicitQuotaByLevelOverride { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, float> ThemeBoundaryCarryCap { get; private set; }
	}
}
