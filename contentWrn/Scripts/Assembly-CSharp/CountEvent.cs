using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CountEvent : MonoBehaviour
{
	public float seconds = 0.2f;

	public UnityEvent eventToCall;

	private void OnEnable()
	{
		StartCoroutine(IDoEvent());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Go()
	{
		StartCoroutine(IDoEvent());
	}

	private IEnumerator IDoEvent()
	{
		yield return new WaitForSeconds(seconds);
		eventToCall.Invoke();
	}
}
