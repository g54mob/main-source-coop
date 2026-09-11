using UnityEngine;

public class RescueHookSFX : MonoBehaviour
{
	public LineRenderer reference;

	public SFX_PlayOneShot[] on;

	public SFX_PlayOneShot[] off;

	public SFX_PlayOneShot[] pull;

	public GameObject[] onObject;

	public GameObject[] offObject;

	private bool t;

	public RescueHook hook;

	private void Update()
	{
		bool flag = (bool)reference && reference.enabled;
		if (reference.enabled)
		{
			flag = true;
		}
		for (int i = 0; i < on.Length; i++)
		{
			on[i].playOnClick = false;
		}
		for (int j = 0; j < off.Length; j++)
		{
			off[j].playOnClick = false;
		}
		if ((bool)hook)
		{
			for (int k = 0; k < pull.Length; k++)
			{
				pull[k].playOnClick = hook.fly;
			}
		}
		if (flag && !t)
		{
			t = true;
			for (int l = 0; l < on.Length; l++)
			{
				on[l].playOnClick = true;
			}
		}
		if (!flag && t)
		{
			t = false;
			for (int m = 0; m < on.Length; m++)
			{
				off[m].playOnClick = true;
			}
		}
		GameObject[] array = onObject;
		for (int n = 0; n < array.Length; n++)
		{
			array[n].SetActive(flag);
		}
		array = offObject;
		for (int n = 0; n < array.Length; n++)
		{
			array[n].SetActive(!flag);
		}
	}
}
