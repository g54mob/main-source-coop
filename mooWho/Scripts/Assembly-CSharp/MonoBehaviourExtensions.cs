using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
	public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay)
	{
		mono.StartCoroutine(CallWithDelayRoutine(method, delay));
	}

	private static IEnumerator CallWithDelayRoutine(Action method, float delay)
	{
		yield return new WaitForSeconds(delay);
		method();
	}

	public static GameObject[] ShuffleList(this MonoBehaviour mono, List<GameObject> list)
	{
		GameObject[] array = list.ToArray();
		for (int i = 0; i < array.Length - 1; i++)
		{
			int num = UnityEngine.Random.Range(i, array.Length);
			GameObject gameObject = array[i];
			array[i] = array[num];
			array[num] = gameObject;
		}
		return array;
	}

	public static float RoundValue(float value, int decimalAmount)
	{
		float num = Mathf.Pow(10f, decimalAmount);
		return Mathf.Round(value * num) / num;
	}
}
