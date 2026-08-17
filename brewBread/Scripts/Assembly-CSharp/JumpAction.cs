using UnityEngine;

[CreateAssetMenu(fileName = "PenguinJump", menuName = "Actions/Jump", order = 1)]
public class JumpAction : PenguinAction
{
	private float _lastPosition;

	private float _detectGroundTimer;

	[SerializeField]
	private float _jumpForce;

	public override void Enter()
	{
		base.Enter();
		Controller.Rb.velocity = Vector2.up * _jumpForce;
		_lastPosition = _controllerTransform.position.y;
		_detectGroundTimer = 0f;
		_grounded = false;
		Controller.JumpEnabled = false;
	}

	public override void LogicUpdate()
	{
		_detectGroundTimer += Time.deltaTime;
		if (_detectGroundTimer >= 0.5f)
		{
			_grounded = true;
		}
		if (_lastPosition > _controllerTransform.position.y)
		{
			Controller.SendPlayerMakerEvent("Fall");
		}
		else
		{
			_lastPosition = _controllerTransform.position.y;
		}
		base.LogicUpdate();
	}

	public override void Exit()
	{
		base.Exit();
		Controller.JumpEnabled = false;
		_grounded = false;
	}
}
