using UnityEngine;

public class SoundOnMesh : MonoBehaviour
{
	public Renderer reference;

	public Light lightRef;

	public SFX_PlayOneShot[] on;

	public SFX_PlayOneShot[] off;

	public GameObject[] onObject;

	public GameObject[] offObject;

	private bool t;

	private void Update()
	{
		bool flag = (bool)reference && reference.enabled;
		if ((bool)lightRef && lightRef.enabled)
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
		if (flag && !t)
		{
			t = true;
			for (int k = 0; k < on.Length; k++)
			{
				on[k].playOnClick = true;
			}
		}
		if (!flag && t)
		{
			t = false;
			for (int l = 0; l < on.Length; l++)
			{
				off[l].playOnClick = true;
			}
		}
		GameObject[] array = onObject;
		for (int m = 0; m < array.Length; m++)
		{
			array[m].SetActive(flag);
		}
		array = offObject;
		for (int m = 0; m < array.Length; m++)
		{
			array[m].SetActive(!flag);
		}
	}
}
