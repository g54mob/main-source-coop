using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.CLI;

public class GameLoopHax : RetrievableSingleton<GameLoopHax>
{
	protected override void OnCreated()
	{
		base.OnCreated();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	[ConsoleCommand]
	public static void DiveAndReturn()
	{
		RetrievableSingleton<GameLoopHax>.Instance.StartCoroutine(RetrievableSingleton<GameLoopHax>.Instance.DiveAndReturnRoutine());
	}

	[ConsoleCommand]
	public static void GoToSleep()
	{
		RetrievableSingleton<GameLoopHax>.Instance.StartCoroutine(RetrievableSingleton<GameLoopHax>.Instance.GoToSleepRoutine());
	}

	[ConsoleCommand]
	public static void OpenComputerDoor()
	{
		SurfaceNetworkHandler.Instance.OpenComputerRoomDoor();
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		List<NetworkDealBase> weightedRandomDeal = SingletonAsset<NetworkDealDataBase>.Instance.GetWeightedRandomDeal(new List<DIFFICULTY>
		{
			DIFFICULTY.veryEasy,
			DIFFICULTY.easy,
			DIFFICULTY.medium,
			DIFFICULTY.hard,
			DIFFICULTY.veryHard
		}, 3, allowDuplicates: false);
		foreach (NetworkDealBase item in weightedRandomDeal)
		{
			item.Init(SingletonAsset<DealRewardDatabase>.Instance.GetRandom(), item.AllowedDifficulties.GetRandom());
		}
		SurfaceNetworkHandler.RoomStats.SetNetworkDealsToSelect(weightedRandomDeal.ToArray());
	}

	[ConsoleCommand]
	public static void NextDay()
	{
		RetrievableSingleton<GameLoopHax>.Instance.StartCoroutine(RetrievableSingleton<GameLoopHax>.Instance.NextDayRoutine());
	}

	private IEnumerator NextDayRoutine()
	{
		yield return DiveAndReturnRoutine();
		yield return GoToSleepRoutine();
	}

	private IEnumerator GoToSleepRoutine()
	{
		Player player = Object.FindObjectOfType<Player>();
		Bed bed = Object.FindObjectOfType<Bed>();
		player.Teleport(bed.transform.position + Vector3.up, Vector3.forward);
		bed.RequestSleep(player);
		yield return new WaitForSeconds(10f);
		Debug.Log("Sleeping complete!");
	}

	private IEnumerator DiveAndReturnRoutine()
	{
		yield return Dive();
		yield return new WaitForSeconds(3f);
		yield return Return();
		static IEnumerator Dive()
		{
			SurfaceNetworkHandler.Instance.RequestStartGame();
			DivingBell divingBell = Object.FindObjectOfType<DivingBell>();
			Object.FindObjectOfType<Player>().Teleport(divingBell.itemSpawns.position + Vector3.up, Vector3.forward);
			divingBell.AttemptSetOpen(open: false);
			Debug.Log("Closing doors");
			yield return new WaitForSeconds(3.5f);
			Debug.Log("Heading underground");
			divingBell.GoUnderground();
			yield return new WaitForSeconds(8f);
		}
	}

	private IEnumerator Return()
	{
		Debug.Log("Returning to surface");
		Object.FindObjectOfType<DivingBell>().GoToSurface();
		yield return new WaitForSeconds(10f);
		Debug.Log("Dive and return complete!");
	}
}
