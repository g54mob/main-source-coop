using UnityEngine;

[CreateAssetMenu(fileName = "PenguinPullRope", menuName = "Actions/PullRopeJeff", order = 1)]
public class PullingRopeJeffAction : PullingRopeAction
{
	public override void Enter()
	{
		if (Controller.Companion.IsGrounded)
		{
			Controller.SendPlayerMakerEvent("UnPullRope");
			return;
		}
		base.Enter();
		Controller.Companion.SetAnimationBool("ClimbRope", value: true);
	}

	public override void Exit()
	{
		base.Exit();
		Controller.Companion.SetAnimationBool("ClimbRope", value: false);
	}
}
