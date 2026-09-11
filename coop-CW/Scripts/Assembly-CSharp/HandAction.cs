using UnityEngine;

public class HandAction : MonoBehaviour
{
	private static readonly int AnimatorLeft = Animator.StringToHash("Left");

	[SerializeField]
	private Transform disableOnRigCreator;

	[SerializeField]
	private bool left;

	[SerializeField]
	private HandReference hand;

	[SerializeField]
	private string actionName;

	[SerializeField]
	private bool action;

	private void Start()
	{
		if (disableOnRigCreator.name == "RigCreator")
		{
			base.gameObject.SetActive(value: false);
		}
		UpdateAnimators();
	}

	private void Update()
	{
		UpdateAnimators();
	}

	private void UpdateAnimators()
	{
		if (left)
		{
			hand.handL.SetBool(actionName, action);
			hand.handL.SetBool(AnimatorLeft, value: true);
		}
		else
		{
			hand.handR.SetBool(actionName, action);
		}
	}
}
