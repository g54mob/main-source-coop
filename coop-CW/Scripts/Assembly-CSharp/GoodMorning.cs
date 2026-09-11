using UnityEngine;

public class GoodMorning : MonoBehaviour
{
	private static readonly int Yes = Animator.StringToHash("Yes");

	public SurfaceNetworkHandler dayCheck;

	public AudioSource audioVol;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if ((bool)dayCheck && (bool)audioVol)
		{
			if (!dayCheck.firstDay || !audioVol.enabled)
			{
				anim.SetBool(Yes, value: true);
			}
			if (audioVol.enabled)
			{
				anim.SetBool(Yes, value: false);
			}
			if (dayCheck.firstDay)
			{
				anim.SetBool(Yes, value: false);
			}
		}
	}
}
