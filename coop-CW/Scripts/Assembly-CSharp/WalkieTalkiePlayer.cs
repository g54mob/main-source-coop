using UnityEngine;

public class WalkieTalkiePlayer : MonoBehaviour
{
	private AudioLoop m_RadioStatic;

	public SFX_Instance sfxOn;

	public SFX_Instance sfxOff;

	public bool on;

	private void Start()
	{
		InitReferences();
	}

	private void InitReferences()
	{
		m_RadioStatic = GetComponent<AudioLoop>();
		KillStatic();
	}

	public void PlayStatic()
	{
		m_RadioStatic.volume = 1f;
		if (!on)
		{
			sfxOn.Play(base.transform.position);
		}
		on = true;
	}

	public void KillStatic()
	{
		m_RadioStatic.volume = 0f;
		if (on)
		{
			sfxOff.Play(base.transform.position);
		}
		on = false;
	}
}
