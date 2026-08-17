using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextAnimationBase
{
	[SerializeField]
	private bool enabled;

	public bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			enabled = value;
		}
	}

	public virtual void Enter(TMP_Text text, string animatedText)
	{
	}

	public virtual void AnimationUpdate(TMP_Text text, string animatedText)
	{
	}

	public virtual void Exit()
	{
	}
}
