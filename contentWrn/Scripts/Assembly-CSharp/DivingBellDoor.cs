using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Zorro.Core;

public class DivingBellDoor : MonoBehaviour
{
	public AnimationClip openedClip;

	public AnimationClip closedClip;

	public AnimationClip openClip;

	public AnimationClip closeClip;

	private Optionable<PlayableGraph> playableGraph;

	private Animator m_animator;

	private AnimationClip m_currentClip;

	private void Awake()
	{
		m_animator = GetComponent<Animator>();
		m_animator.runtimeAnimatorController = null;
	}

	public void Init(bool isOpen)
	{
		if (isOpen)
		{
			PlayClip(openedClip);
		}
		else
		{
			PlayClip(closedClip);
		}
	}

	public IEnumerator Open()
	{
		PlayClip(openClip);
		yield return new WaitForSeconds(openClip.length);
		PlayClip(openedClip);
	}

	public IEnumerator Close()
	{
		PlayClip(closeClip);
		yield return new WaitForSeconds(closeClip.length);
		PlayClip(closedClip);
	}

	private void PlayClip(AnimationClip clip)
	{
		if (this.playableGraph.IsSome)
		{
			this.playableGraph.Value.Destroy();
		}
		m_currentClip = clip;
		PlayableGraph playableGraph = PlayableGraph.Create("Door Graph: " + clip.name);
		playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
		AnimationPlayableOutput output = AnimationPlayableOutput.Create(playableGraph, "Animation", m_animator);
		AnimationClipPlayable value = AnimationClipPlayable.Create(playableGraph, clip);
		output.SetSourcePlayable(value);
		playableGraph.Play();
		this.playableGraph = Optionable<PlayableGraph>.Some(playableGraph);
	}

	private void OnDestroy()
	{
		if (playableGraph.IsSome)
		{
			playableGraph.Value.Destroy();
		}
	}

	public bool IsFullyClosed()
	{
		return m_currentClip == closedClip;
	}
}
