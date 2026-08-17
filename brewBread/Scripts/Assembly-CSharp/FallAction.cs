using UnityEngine;

[CreateAssetMenu(fileName = "PenguinFall", menuName = "Actions/Fall", order = 1)]
public class FallAction : PenguinAction
{
	[SerializeField]
	private float _fallTime;

	[SerializeField]
	private float _coyoteTime;

	private float _fallTimeAux;

	private float _coyoteTimeAux;

	public override void Enter()
	{
		base.Enter();
		_fallTimeAux = _fallTime;
		_coyoteTimeAux = _coyoteTime;
		if (StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
		{
			Controller.JumpEnabled = true;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (_fallTimeAux <= 0f)
		{
			Controller.SendPlayerMakerEvent("FallHard");
		}
		if (Controller.JumpEnabled)
		{
			_coyoteTimeAux -= Time.deltaTime;
			if (_coyoteTimeAux < 0f && !StaticInstance<AssistModeManager>.Instance.InfiniteJumps)
			{
				Controller.JumpEnabled = false;
			}
		}
		_fallTimeAux -= Time.deltaTime;
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
