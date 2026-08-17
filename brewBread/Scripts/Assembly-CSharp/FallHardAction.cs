using UnityEngine;

[CreateAssetMenu(fileName = "PenguinFallHard", menuName = "Actions/FallHard", order = 1)]
public class FallHardAction : PenguinAction
{
	public override void LogicUpdate()
	{
		if (Controller.IsGrounded)
		{
			Controller.SendPlayerMakerEvent("LandHard");
		}
		if (StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
		{
			Controller.JumpEnabled = true;
		}
		else
		{
			Controller.JumpEnabled = false;
		}
	}
}
