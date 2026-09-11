using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TakeDamagePost : MonoBehaviour
{
	public static TakeDamagePost instance;

	private Volume volume;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		volume = GetComponent<Volume>();
	}

	public void TakeDamageFeedback()
	{
		StopAllCoroutines();
		StartCoroutine(IBlink());
		IEnumerator IBlink()
		{
			volume.enabled = true;
			if (volume.profile.TryGet<LimitlessGlitch3>(out var glitch3))
			{
				glitch3.active = true;
			}
			volume.weight = 1f;
			yield return new WaitForSeconds(0.1f);
			while (volume.weight > 0.001f)
			{
				volume.weight = Mathf.MoveTowards(volume.weight, 0f, Time.deltaTime * 3f);
			}
			if ((bool)glitch3)
			{
				glitch3.active = false;
			}
			volume.enabled = false;
		}
	}
}
