using TMPro;
using UnityEngine;

public class DiveBellSFX : MonoBehaviour
{
	private static readonly int Land = Animator.StringToHash("Land");

	public AudioClip loopSound;

	public DivingBell diveBell;

	public TextMeshProUGUI text;

	private string textprev;

	public SFX_Instance readyToSubmerge;

	public SFX_Instance closeDoor;

	public SFX_Instance notAll;

	public SFX_Instance diveBellStartGoingDown;

	public Animator diveBellLand;

	private void Update()
	{
		if (diveBell.onSurface && !diveBell.opened)
		{
			diveBellLand.SetBool(Land, value: true);
		}
		if (!diveBell.onSurface && !diveBell.opened)
		{
			diveBellLand.SetBool(Land, value: true);
		}
		if (textprev != text.text)
		{
			if (text.text == "Ready to submerge!" || text.text == "Ready to return")
			{
				readyToSubmerge.Play(base.transform.position);
			}
			if (text.text == "Not all suits are inside")
			{
				notAll.Play(base.transform.position);
			}
			if (text.text == "Close the door!")
			{
				closeDoor.Play(base.transform.position);
			}
		}
		textprev = text.text;
	}

	public void PlayStartTransition()
	{
		diveBellStartGoingDown.Play(base.transform.position);
	}
}
