using UnityEngine;

public class Emotes : MonoBehaviour
{
	public bool emoting;

	private bool t;

	public int currentEmote;

	public AnimationClip[] emotes;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}
}
