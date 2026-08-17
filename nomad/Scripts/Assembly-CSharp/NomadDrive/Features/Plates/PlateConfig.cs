using System;
using System.Collections.Generic;
using NomadDrive.Features.WorldGeneration.Utilities;
using UnityEngine;

namespace NomadDrive.Features.Plates
{
	[CreateAssetMenu(fileName = "PlateConfig", menuName = "NomadDrive/Items/Plate Config")]
	public class PlateConfig : ScriptableObject
	{
		[Serializable]
		public class CityEntry
		{
			[Tooltip("Display name shown on the plate and panel. Proper noun — resolved via LocalizeName with a literal fallback.")]
			public string cityName = "Los Angeles";

			[Min(0.0001f)]
			[Tooltip("Selection weight. Higher = more common. Odds for this city = 1 / (weight / totalWeight).")]
			public float weight = 100f;
		}

		[Serializable]
		public class PatternEntry
		{
			public PlatePatternType type;

			[Tooltip("Localization key for the pattern name (e.g. @plate.pattern_solid).")]
			public string nameKey;

			[Min(1f)]
			[Tooltip("Designer-tuned display odds: roughly 1 in N that a random code shows this pattern. Feeds the cumulative odds and tier.")]
			public int oddsDenominator = 100;
		}

		[Serializable]
		public class TierThreshold
		{
			[Tooltip("Localization key for the tier name (e.g. @plate.tier_mythic).")]
			public string nameKey;

			[Tooltip("Tier enum — gates the rare-found notification.")]
			public PlateTier tier;

			[Tooltip("Minimum cumulative odds denominator (1 in N) required to reach this tier.")]
			public long minOdds = 1L;

			public Color color = Color.white;
		}

		[Header("Cities (weighted)")]
		[Tooltip("Fictional origin names. Higher weight = more common. Defaults span ~1/2 (Redhollow) to ~1/2165 (Nyx Hollow).")]
		[SerializeField]
		private CityEntry[] cities = new CityEntry[10]
		{
			new CityEntry
			{
				cityName = "Redhollow",
				weight = 500f
			},
			new CityEntry
			{
				cityName = "Marrow Creek",
				weight = 250f
			},
			new CityEntry
			{
				cityName = "Dustwick",
				weight = 150f
			},
			new CityEntry
			{
				cityName = "Pellmont",
				weight = 90f
			},
			new CityEntry
			{
				cityName = "Gravesend Junction",
				weight = 50f
			},
			new CityEntry
			{
				cityName = "Ashbarrow",
				weight = 25f
			},
			new CityEntry
			{
				cityName = "Cinderfell",
				weight = 10f
			},
			new CityEntry
			{
				cityName = "Wraithmoor",
				weight = 5f
			},
			new CityEntry
			{
				cityName = "Halcyon Reach",
				weight = 2f
			},
			new CityEntry
			{
				cityName = "Nyx Hollow",
				weight = 0.5f
			}
		};

		[Header("Code patterns")]
		[Tooltip("Detection order does not matter; a code resolves to the matched pattern with the LARGEST oddsDenominator.")]
		[SerializeField]
		private PatternEntry[] patterns = new PatternEntry[6]
		{
			new PatternEntry
			{
				type = PlatePatternType.RepeatingPair,
				nameKey = "@plate.pattern_repeating_pair",
				oddsDenominator = 3
			},
			new PatternEntry
			{
				type = PlatePatternType.SequentialDigits,
				nameKey = "@plate.pattern_sequence",
				oddsDenominator = 63
			},
			new PatternEntry
			{
				type = PlatePatternType.LowNumber,
				nameKey = "@plate.pattern_low_number",
				oddsDenominator = 100
			},
			new PatternEntry
			{
				type = PlatePatternType.AllDigitsSame,
				nameKey = "@plate.pattern_all_digits",
				oddsDenominator = 100
			},
			new PatternEntry
			{
				type = PlatePatternType.AllLettersSame,
				nameKey = "@plate.pattern_all_letters",
				oddsDenominator = 676
			},
			new PatternEntry
			{
				type = PlatePatternType.Solid,
				nameKey = "@plate.pattern_solid",
				oddsDenominator = 676000
			}
		};

		[Header("Foil / gold variant")]
		[Range(0f, 100f)]
		[SerializeField]
		private float foilChancePercent = 0.5f;

		[Min(1f)]
		[SerializeField]
		private int foilOddsDenominator = 200;

		[SerializeField]
		private Material normalPlateMaterial;

		[SerializeField]
		private Material foilPlateMaterial;

		[Header("Quality (font face dilate)")]
		[Tooltip("Each plate rolls an independent 0-1 quality. It maps to the text's Face > Dilate: low = faded/worn, high = crisp — like a per-plate condition.")]
		[SerializeField]
		private float minFaceDilate = -0.2f;

		[SerializeField]
		private float maxFaceDilate = 0.12f;

		[Header("Tiers")]
		[Tooltip("Each tier the cumulative odds qualify for; the entry with the highest satisfied minOdds wins.")]
		[SerializeField]
		private TierThreshold[] tierThresholds = new TierThreshold[6]
		{
			new TierThreshold
			{
				tier = PlateTier.Common,
				nameKey = "@plate.tier_common",
				minOdds = 1L,
				color = new Color(0.78f, 0.8f, 0.82f, 1f)
			},
			new TierThreshold
			{
				tier = PlateTier.Uncommon,
				nameKey = "@plate.tier_uncommon",
				minOdds = 50L,
				color = new Color(0.45f, 0.85f, 0.45f, 1f)
			},
			new TierThreshold
			{
				tier = PlateTier.Rare,
				nameKey = "@plate.tier_rare",
				minOdds = 500L,
				color = new Color(0.35f, 0.65f, 1f, 1f)
			},
			new TierThreshold
			{
				tier = PlateTier.Epic,
				nameKey = "@plate.tier_epic",
				minOdds = 10000L,
				color = new Color(0.7f, 0.45f, 1f, 1f)
			},
			new TierThreshold
			{
				tier = PlateTier.Legendary,
				nameKey = "@plate.tier_legendary",
				minOdds = 500000L,
				color = new Color(1f, 0.7f, 0.25f, 1f)
			},
			new TierThreshold
			{
				tier = PlateTier.Mythic,
				nameKey = "@plate.tier_mythic",
				minOdds = 50000000L,
				color = new Color(1f, 0.35f, 0.45f, 1f)
			}
		};

		[Header("Rare notification")]
		[Tooltip("First time the local player hovers a plate at this tier (or rarer), a floating notification is shown.")]
		[SerializeField]
		private PlateTier rareNotificationMinTier = PlateTier.Epic;

		public float FoilChancePercent => foilChancePercent;

		public int FoilOddsDenominator => foilOddsDenominator;

		public Material NormalPlateMaterial => normalPlateMaterial;

		public Material FoilPlateMaterial => foilPlateMaterial;

		public float MinFaceDilate => minFaceDilate;

		public float MaxFaceDilate => maxFaceDilate;

		public PlateTier RareNotificationMinTier => rareNotificationMinTier;

		public bool HasCities
		{
			get
			{
				if (cities != null)
				{
					return cities.Length != 0;
				}
				return false;
			}
		}

		public CityEntry GetCity(int index)
		{
			if (cities == null || index < 0 || index >= cities.Length)
			{
				return null;
			}
			return cities[index];
		}

		public int SelectCityIndex(System.Random random)
		{
			if (!HasCities)
			{
				return 0;
			}
			CityEntry value = new WeightedRandomSelector<CityEntry>(cities, (CityEntry c) => c.weight).Select(random);
			int num = Array.IndexOf(cities, value);
			if (num >= 0)
			{
				return num;
			}
			return 0;
		}

		public long GetCityOddsDenominator(int index)
		{
			CityEntry city = GetCity(index);
			if (city == null || city.weight <= 0f)
			{
				return 1L;
			}
			double num = 0.0;
			CityEntry[] array = cities;
			foreach (CityEntry cityEntry in array)
			{
				num += (double)cityEntry.weight;
			}
			return (long)Math.Max(1.0, Math.Round(num / (double)city.weight));
		}

		public PatternEntry ResolveBestPattern(string code)
		{
			if (patterns == null || patterns.Length == 0 || string.IsNullOrEmpty(code))
			{
				return null;
			}
			List<PlatePatternType> list = new List<PlatePatternType>(4);
			PlatePatternDetector.Detect(code, list);
			if (list.Count == 0)
			{
				return null;
			}
			PatternEntry patternEntry = null;
			PatternEntry[] array = patterns;
			foreach (PatternEntry patternEntry2 in array)
			{
				if (patternEntry2 != null && patternEntry2.type != PlatePatternType.None && list.Contains(patternEntry2.type) && (patternEntry == null || patternEntry2.oddsDenominator > patternEntry.oddsDenominator))
				{
					patternEntry = patternEntry2;
				}
			}
			return patternEntry;
		}

		public TierThreshold ResolveTier(long cumulativeOdds)
		{
			if (tierThresholds == null || tierThresholds.Length == 0)
			{
				return null;
			}
			TierThreshold tierThreshold = null;
			TierThreshold[] array = tierThresholds;
			foreach (TierThreshold tierThreshold2 in array)
			{
				if (tierThreshold2 != null && cumulativeOdds >= tierThreshold2.minOdds && (tierThreshold == null || tierThreshold2.minOdds > tierThreshold.minOdds))
				{
					tierThreshold = tierThreshold2;
				}
			}
			return tierThreshold ?? tierThresholds[0];
		}
	}
}
