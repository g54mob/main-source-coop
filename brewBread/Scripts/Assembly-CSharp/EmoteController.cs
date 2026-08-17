using UnityEngine;

public class EmoteController : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (base.transform.parent.localScale.x < 1f)
		{
			base.transform.localScale = new Vector3(-1f, 1f);
		}
		else
		{
			base.transform.localScale = new Vector3(1f, 1f);
		}
	}

	public void PlayeAnimation(string animation)
	{
		animator.Play(animation);
	}

	public void HideEmote()
	{
		animator.Play("Normal");
	}

	public void PlayNoVariableSound(AudioClip clip)
	{
		AudioSource component = GetComponent<AudioSource>();
		if (component != null)
		{
			component.PlayOneShot(clip);
		}
	}
}
