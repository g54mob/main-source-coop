using UnityEngine;

public class EnableAnimators : MonoBehaviour
{
	public EnableAnimators refAnimator;

	public bool on;

	public string actName;

	public Animator[] animators;

	private void Update()
	{
		if ((bool)base.transform.parent && base.transform.parent.name == "RigCreator")
		{
			base.gameObject.SetActive(value: false);
		}
		if ((bool)refAnimator)
		{
			refAnimator.on = on;
		}
		if (animators.Length != 0)
		{
			for (int i = 0; i < animators.Length; i++)
			{
				animators[i].SetBool(actName, on);
			}
		}
	}
}
