using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class MonoFunctions : MonoBehaviour
{
	public static MonoFunctions instance;

	private void Awake()
	{
		instance = this;
	}

	public void DelayCall(Action actionToDo, float seconds)
	{
		StartCoroutine(IDelayCall(actionToDo, seconds));
		static IEnumerator IDelayCall(Action action, float seconds2)
		{
			yield return new WaitForSeconds(seconds2);
			action?.Invoke();
		}
	}

	internal void PhotonDestroy(GameObject target, float seconds)
	{
		if (HelperFunctions.IsMine(target))
		{
			StartCoroutine(IDelayDestroy(target, seconds));
		}
		static IEnumerator IDelayDestroy(GameObject targetGo, float seconds2)
		{
			yield return new WaitForSeconds(seconds2);
			PhotonNetwork.Destroy(targetGo);
		}
	}
}
