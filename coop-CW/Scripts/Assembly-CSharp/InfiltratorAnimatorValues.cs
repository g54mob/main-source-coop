using UnityEngine;

public class InfiltratorAnimatorValues : MonoBehaviour
{
	private Animator anim;

	private void Start()
	{
		if ((bool)GetComponent<Animator>())
		{
			anim = GetComponent<Animator>();
		}
	}

	private void Update()
	{
		if ((bool)anim)
		{
			anim.SetBool("Infiltrator Hold Item", value: true);
		}
	}

	private void LateUpdate()
	{
		if ((bool)anim)
		{
			anim.SetBool("Infiltrator Hold Item", value: true);
		}
	}
}
