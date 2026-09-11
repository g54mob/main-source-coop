using System;
using UnityEngine;

public class SFX_PlayOneShot : MonoBehaviour
{
	public Action beforePlayAction;

	public Action afterPlayAction;

	public bool playOnStart;

	public bool playOnEnable;

	public bool playOnClick;

	public float cd;

	private float lastTimePlayed;

	private bool t;

	public SFX_Instance sfx;

	public SFX_Instance[] sfxs;

	private void Update()
	{
		if (playOnClick && !t)
		{
			Play();
			t = true;
		}
		if (!playOnClick)
		{
			t = false;
		}
	}

	public void Start()
	{
		if (playOnStart)
		{
			Play();
		}
	}

	public void OnEnable()
	{
		if (playOnEnable)
		{
			Play();
		}
	}

	public void Play()
	{
		if ((bool)SFX_Player.instance && !(Time.time < lastTimePlayed + cd))
		{
			lastTimePlayed = Time.time;
			beforePlayAction?.Invoke();
			if ((bool)sfx)
			{
				SFX_Player.instance.PlaySFX(sfx, base.transform.position);
			}
			for (int i = 0; i < sfxs.Length; i++)
			{
				SFX_Player.instance.PlaySFX(sfxs[i], base.transform.position);
			}
			afterPlayAction?.Invoke();
		}
	}
}
