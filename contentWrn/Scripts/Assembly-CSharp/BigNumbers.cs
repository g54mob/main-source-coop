using System;
using CurvedUI;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.Core;
using pworld.Scripts.Extensions;

[CreateAssetMenu(fileName = "BigNumbers", menuName = "BigNumbers")]
public class BigNumbers : SingletonAsset<BigNumbers>
{
	[Header("Rounds")]
	public AnimationCurve scoreRequiredPerWeek;

	public AnimationCurve scoreToViewConversionPerRun;

	public AnimationCurve ScoreToMoneyPerWeek;

	public int StartMoney = 100;

	public int MoneyPerRound;

	[Header("Monsters")]
	public float BarnacleBallScore = 130f;

	public float BigSlapAgro = 200f;

	public float BigSlapPeaceful = 100f;

	public float EarScore = 55f;

	public float FlickerScore = 350f;

	public float JelloScore = 50f;

	public float KnifoScore = 55f;

	public float bombsScore = 55f;

	public float spiderScore = 55f;

	public float larvaScore = 55f;

	public float walloScore = 55f;

	public float mimeScore = 40f;

	public float wormScore = 30f;

	public float fireMonsterScore = 100f;

	public float blackHoleBotScore = 25f;

	public float snailSpawnerScore = 55f;

	public float streamerScore = 100f;

	public float harpoonerScore = 55f;

	public float robotButton = 55f;

	public float dogScore = 55f;

	public float camCreepScore = 55f;

	public float eyeGuyScore = 55f;

	public float MouthScore = 35f;

	public float SlurperScore = 55f;

	public float SnatchoScore = 80f;

	public float ToolkitWhiskScore = 40f;

	public float WeepingScoreIdle = 50f;

	public float WeepingScoreSuccuess = 20f;

	public float WeepingScoreFail;

	public float WeepingScoreCaptured = 30f;

	public float ZombieScore = 25f;

	public float puffoScore = 35f;

	[Header("Players")]
	public float PlayerScore = 10f;

	public float playerRagdollScore = 20f;

	public float playerSmallFallScore = 10f;

	public float playerBigFallScore = 10f;

	public float playerDeadScore = 10f;

	public float PlayerFacingAwayFactor = 1f;

	public float PlayerFacingTowardsFactor = 2f;

	public float PlayerMicrophoneBounus = 3f;

	public float playerGoodCatchScore = 1f;

	public float playerTookDamageScore = 1f;

	[Header("Camera")]
	public AnimationCurve streakCurve;

	public AnimationCurve percentageToScreenToFactorCurve;

	public int MaxSameEventType = 3;

	[Header("Monster Budget")]
	public AnimationCurve monsterBudgetPerDay;

	public int day1ExtraMoBudget = 100;

	public int day2ExtraMoBudget = 200;

	public int day3ExtraMoBudget = 300;

	public Vector2 minMaxSecoundRoundSpawnTime = new Vector2(0.2f, 0.8f);

	public Vector2 firstSpawnAmount = new Vector2(0.7f, 1f);

	[FormerlySerializedAs("biggestPurchasePerDay")]
	public AnimationCurve biggestMonsterPurchasePerDay;

	[Header("Artifact")]
	public AnimationCurve artifactBudgetPerDay;

	public AnimationCurve rarestArtifactPerDay;

	[Header("ToolSpawner")]
	public AnimationCurve toolBudgetPerDay;

	public AnimationCurve rarestToolPerDay;

	public Vector2Int toolPerDayAddMinMax = new Vector2Int(1, 3);

	private float firstWaveSizeWas;

	public float playerHoldingMicScore;

	public static int GetTempDiveBellDifficulty(int day)
	{
		return day;
	}

	public static int GetDiveBellDifficultyHarbour(int day)
	{
		if (day < 4)
		{
			throw new Exception("shits wonky");
		}
		day -= 3;
		if (day > 3)
		{
			day -= 3;
		}
		return day;
	}

	public static int GetGiveDiveBellDifficultyFactory(int day)
	{
		if (day > 3)
		{
			day -= 3;
		}
		return day;
	}

	public static string ViewsToString(int numberIn)
	{
		string text = "";
		if (numberIn < 1000)
		{
			return numberIn.ToString();
		}
		if (numberIn < 1000000)
		{
			if (numberIn < 10000)
			{
				int num = numberIn / 1000;
				int num2 = numberIn - num * 1000;
				num2 /= 100;
				return num + "." + num2 + "K";
			}
			return numberIn / 1000 + "K";
		}
		if (numberIn < 10000000)
		{
			int num3 = numberIn / 1000000;
			int num4 = numberIn - num3 * 1000000;
			num4 /= 100000;
			return num3 + "." + num4 + "M";
		}
		return numberIn / 1000000 + "M";
	}

	public static int GetScoreToViews(float score, int day)
	{
		int run = Mathf.FloorToInt(((float)day - 1f) / 3f);
		return GetScoreToViewsFromRun(score, run);
	}

	public static int GetScoreToViewsFromRun(float score, int run)
	{
		return Mathf.RoundToInt(score * SingletonAsset<BigNumbers>.Instance.scoreToViewConversionPerRun.Evaluate(run));
	}

	public static float GetSecondWaveWaitTime()
	{
		return SingletonAsset<BigNumbers>.Instance.minMaxSecoundRoundSpawnTime.PRndRange() * Player.localPlayer.data.maxOxygen;
	}

	public static int GetMetaScoreFromRun(int run)
	{
		return 50 * run;
	}

	public static int GetMetaBaseScoreFromQuest(DIFFICULTY difficulty)
	{
		return difficulty switch
		{
			DIFFICULTY.veryEasy => GetMetaScoreFromRun(1) * 2 - 20, 
			DIFFICULTY.easy => GetMetaScoreFromRun(2) * 2, 
			DIFFICULTY.medium => GetMetaScoreFromRun(3) * 2 + 50, 
			DIFFICULTY.hard => GetMetaScoreFromRun(4) * 2 + 100, 
			DIFFICULTY.veryHard => GetMetaScoreFromRun(5) * 2 + 200, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public static int GetMoneyFromScore(float score, int day)
	{
		int run = Mathf.FloorToInt(((float)day - 1f) / 3f);
		return GetMoneyFromScoreByRun(score, run);
	}

	public static int GetMoneyFromScoreByRun(float score, int run)
	{
		float num = SingletonAsset<BigNumbers>.Instance.ScoreToMoneyPerWeek.Evaluate(run);
		return Mathf.RoundToInt(score * num);
	}

	public static int GetQuota(int runIndex)
	{
		return SingletonAsset<BigNumbers>.Instance.scoreRequiredPerWeek.Evaluate(runIndex).ToInt();
	}

	public static int GetMonsterBudgetForDayFirstWave(int day)
	{
		if (SingletonAsset<BigNumbers>.Instance == null)
		{
			Debug.LogError("Bignumbers is null");
		}
		int num = (day - 1) % 3 + 1;
		int num2 = Mathf.RoundToInt(SingletonAsset<BigNumbers>.Instance.monsterBudgetPerDay.Evaluate(day) + (float)GetExtraBudgetForDay(num));
		float num3 = SingletonAsset<BigNumbers>.Instance.firstSpawnAmount.PRndRange();
		SingletonAsset<BigNumbers>.Instance.firstWaveSizeWas = num3;
		num2 = (SingletonAsset<BigNumbers>.Instance.firstWaveSizeWas * (float)num2).ToInt();
		Debug.LogWarning($"1st wave Day: {day} quotaDay: {num} budget {num2}");
		return num2;
	}

	public static int GetMonsterBudgetForSecondWave(int day)
	{
		int num = (day - 1) % 3 + 1;
		int num2 = Mathf.RoundToInt(SingletonAsset<BigNumbers>.Instance.monsterBudgetPerDay.Evaluate(day) + (float)GetExtraBudgetForDay(num));
		num2 = ((1f - SingletonAsset<BigNumbers>.Instance.firstWaveSizeWas) * (float)num2).ToInt();
		Debug.LogWarning($"2nd wave Day: {day} quotaDay: {num} budget {num2}");
		return num2;
	}

	public static int GetBiggestMonsterPurchaseForDay(int day)
	{
		return SingletonAsset<BigNumbers>.Instance.biggestMonsterPurchasePerDay.Evaluate(day).ToInt();
	}

	public static int GetArtifactBudgetForDay(int day)
	{
		return SingletonAsset<BigNumbers>.Instance.artifactBudgetPerDay.Evaluate(day).ToInt();
	}

	public static float GetRarestArtifactForDay(int day)
	{
		return SingletonAsset<BigNumbers>.Instance.rarestArtifactPerDay.Evaluate(day);
	}

	public static int GetToolBudgetForDay(int day)
	{
		return SingletonAsset<BigNumbers>.Instance.toolBudgetPerDay.Evaluate(day).ToInt() + SingletonAsset<BigNumbers>.Instance.toolPerDayAddMinMax.PRndRange();
	}

	public static float GetRarestToolForDay(int day)
	{
		return SingletonAsset<BigNumbers>.Instance.rarestToolPerDay.Evaluate(day);
	}

	public static int GetExtraBudgetForDay(int day)
	{
		return day switch
		{
			1 => SingletonAsset<BigNumbers>.Instance.day1ExtraMoBudget, 
			2 => SingletonAsset<BigNumbers>.Instance.day2ExtraMoBudget, 
			3 => SingletonAsset<BigNumbers>.Instance.day3ExtraMoBudget, 
			_ => throw new Exception("Day not found"), 
		};
	}
}
