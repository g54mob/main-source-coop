using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.CLI;
using pworld.Scripts.Extensions;

[CreateAssetMenu(fileName = "NetworkDealDataBase", menuName = "NetworkDealDataBase")]
public class NetworkDealDataBase : SingletonAsset<NetworkDealDataBase>
{
	public List<NetworkDealBase> deals;

	public void LoadDeals()
	{
		deals = new List<NetworkDealBase>();
		Type[] classesThatDeriveFrom = ReflectionUtility.GetClassesThatDeriveFrom(typeof(NetworkDealBase));
		for (int i = 0; i < classesThatDeriveFrom.Length; i++)
		{
			NetworkDealBase networkDealBase = (NetworkDealBase)Activator.CreateInstance(classesThatDeriveFrom[i]);
			if (networkDealBase.UseInGame)
			{
				deals.Add(networkDealBase);
			}
		}
	}

	public List<ParameterAutocomplete> GetAutoCompleteOptions(string parameterText)
	{
		List<ParameterAutocomplete> list = new List<ParameterAutocomplete>();
		foreach (NetworkDealBase deal in deals)
		{
			if (deal != null && deal.GetType().ToString().ToLower()
				.Contains(parameterText.ToLower()))
			{
				list.Add(new ParameterAutocomplete(deal.GetType().ToString().WithoutWhitespace()));
			}
		}
		return list;
	}

	private void Go()
	{
		foreach (NetworkDealBase deal in deals)
		{
			Debug.Log($"Type {deal.GetType()}");
		}
	}

	public NetworkDealBase GetDealFromString(string inString)
	{
		inString = inString.ToLower();
		foreach (NetworkDealBase deal in SingletonAsset<NetworkDealDataBase>.Instance.deals)
		{
			if (deal.GetType().ToString().WithoutWhitespace()
				.ToLower() == inString)
			{
				return deal.CreateNew();
			}
		}
		return null;
	}

	public NetworkDealBase GetDealFromIndex(int index)
	{
		foreach (NetworkDealBase deal in deals)
		{
			if (deal.GetIndex() == index)
			{
				return deal.CreateNew();
			}
		}
		Debug.LogError("coulndt find reward with index " + index);
		return null;
	}

	public List<NetworkDealBase> GetWeightedRandomDeal(List<DIFFICULTY> difficulties, int numbers, bool allowDuplicates)
	{
		HashSet<NetworkDealBase> hashSet = new HashSet<NetworkDealBase>();
		int num = 0;
		int num2 = 0;
		while (num < numbers && num2 < 20)
		{
			NetworkDealBase weightedRandom = deals.GetWeightedRandom((NetworkDealBase o) => o.GetWeight());
			if (allowDuplicates || !hashSet.Contains(weightedRandom))
			{
				hashSet.Add(weightedRandom);
				num++;
				num2++;
			}
		}
		return hashSet.ToList();
	}

	private void PrintChances()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		int times = 1000;
		for (int i = 0; i < times; i++)
		{
			List<NetworkDealBase> weightedRandomDeal = SingletonAsset<NetworkDealDataBase>.Instance.GetWeightedRandomDeal(new List<DIFFICULTY>
			{
				DIFFICULTY.veryEasy,
				DIFFICULTY.easy,
				DIFFICULTY.medium,
				DIFFICULTY.hard,
				DIFFICULTY.veryHard
			}, 3, allowDuplicates: false);
			for (int j = 0; j < weightedRandomDeal.Count; j++)
			{
				NetworkDealBase networkDealBase = weightedRandomDeal[j];
				if (!dictionary.ContainsKey(networkDealBase.GetIndex()))
				{
					dictionary.Add(networkDealBase.GetIndex(), 0);
				}
				dictionary[networkDealBase.GetIndex()]++;
			}
		}
		dictionary.PrintLedger(times, delegate(int index, int count)
		{
			NetworkDealBase dealFromIndex = GetDealFromIndex(index);
			Debug.Log($"{dealFromIndex}: {(float)count / (float)times}");
		});
	}

	private void PrintChancesOnce()
	{
		int times = 10000;
		deals.GetLedgerOfChances((NetworkDealBase deal) => deal.GetWeight(), times).PrintLedger(times);
	}

	public List<NetworkDealBase> GetDeals(DIFFICULTY difficulty)
	{
		return GetDeals(difficulty.PToList());
	}

	public List<NetworkDealBase> GetDeals(List<DIFFICULTY> difficulties)
	{
		List<NetworkDealBase> list = new List<NetworkDealBase>();
		foreach (NetworkDealBase deal in deals)
		{
			foreach (DIFFICULTY difficulty in difficulties)
			{
				if (deal.AllowedDifficulties.Contains(difficulty))
				{
					list.Add(deal);
				}
			}
		}
		return list;
	}
}
