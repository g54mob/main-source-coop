using System.Collections;
using UnityEngine;

public class SC_Vefects_Delete_After_Time : MonoBehaviour
{
	public float countdownSeconds = 10f;

	private void Awake()
	{
		StartCoroutine(waiter());
	}

	private IEnumerator waiter()
	{
		yield return new WaitForSeconds(countdownSeconds);
		Object.Destroy(base.gameObject);
	}
}
