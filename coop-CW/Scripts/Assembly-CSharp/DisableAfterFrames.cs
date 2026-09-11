using System.Collections;
using UnityEngine;

public class DisableAfterFrames : MonoBehaviour
{
	private IEnumerator Start()
	{
		for (int i = 0; i < 10; i++)
		{
			Debug.Log(Time.frameCount);
			yield return null;
		}
		base.gameObject.SetActive(value: false);
	}
}
