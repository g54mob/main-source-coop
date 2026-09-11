using UnityEngine;

public class DimensionSound : MonoBehaviour
{
	public float anticipation;

	public float possess;

	public Bot_Skinny reference;

	public SFX_PlayOneShot[] on;

	public SFX_PlayOneShot[] off;

	private bool t;

	private bool t2;

	public AudioLoop possesAudio;

	public AudioLoop possesAudio2;

	public SFX_PlayOneShot anticipationAudio2;

	private void Update()
	{
		if (reference.nextWillBeFull)
		{
			anticipation = Mathf.InverseLerp(4f, 0f, reference.untilNextSwitch);
		}
		else
		{
			anticipation = 0f;
		}
		if ((bool)anticipationAudio2)
		{
			anticipationAudio2.playOnClick = false;
			if (anticipation > 0.01f && !t2)
			{
				anticipationAudio2.playOnClick = true;
				t2 = true;
			}
			if (anticipation < 0.01f)
			{
				t2 = false;
			}
		}
		for (int i = 0; i < on.Length; i++)
		{
			on[i].playOnClick = false;
		}
		for (int j = 0; j < off.Length; j++)
		{
			off[j].playOnClick = false;
		}
		if (!possesAudio)
		{
			if (reference.fullyInDimention && !t)
			{
				t = true;
				for (int k = 0; k < on.Length; k++)
				{
					on[k].playOnClick = true;
				}
			}
			if (!reference.fullyInDimention && t)
			{
				t = false;
				for (int l = 0; l < on.Length; l++)
				{
					off[l].playOnClick = true;
				}
			}
		}
		if (!possesAudio)
		{
			return;
		}
		possess = Mathf.Lerp(possess, Player.localPlayer.data.possession, 300f * Time.deltaTime);
		if (!reference.fullyInDimention)
		{
			possess = 0f;
		}
		possesAudio.volume = possess / 5f;
		if (possess > 0.001f && !t)
		{
			possesAudio2.enabled = true;
			t = true;
			for (int m = 0; m < on.Length; m++)
			{
				on[m].playOnClick = true;
			}
		}
		if (possess < 0.001f && t)
		{
			possesAudio2.enabled = false;
			t = false;
			for (int n = 0; n < on.Length; n++)
			{
				off[n].playOnClick = true;
			}
		}
	}
}
