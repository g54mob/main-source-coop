using UnityEngine;

[CreateAssetMenu(fileName = "PenguinSwing", menuName = "Actions/Swing", order = 1)]
public class SwingAction : PenguinAction
{
	[SerializeField]
	private float _swingForce;

	[SerializeField]
	private float _additiveForce;

	[SerializeField]
	private float _swingTime;

	[SerializeField]
	private float _restTime;

	[SerializeField]
	private float _timeBeforeFalling;

	private float _swingForceAux;

	private float _restTimeAux;

	private float _swingTimeAux;

	private float _timeBeforeFallingAux;

	private Rigidbody2D _rb;

	private float _prevDirection;

	public override void Initialize()
	{
		base.Initialize();
		_rb = Controller.Rb;
	}

	public override void Enter()
	{
		base.Enter();
		_swingForceAux = _swingForce;
		_restTimeAux = _restTime;
		_swingTimeAux = _swingTime;
		_timeBeforeFallingAux = 0f;
		Controller.JumpEnabled = false;
	}

	public override void LogicUpdate()
	{
		RotateCharacter();
		if (!Controller.Companion.SwingConditions())
		{
			Fall();
		}
		if (Controller.CompanionTransform.position.y > Controller.transform.position.y)
		{
			if ((bool)Physics2D.Raycast(Controller.transform.position, Vector2.left, 0.6f, Controller.GroundLayer) || (bool)Physics2D.Raycast(Controller.transform.position, Vector2.right, 0.6f, Controller.GroundLayer))
			{
				_timeBeforeFallingAux += Time.deltaTime;
			}
			else if (_timeBeforeFallingAux > 0f)
			{
				_timeBeforeFallingAux -= Time.deltaTime;
			}
			if (_timeBeforeFallingAux > _timeBeforeFalling && Controller.Companion.IsGrounded)
			{
				Controller.SendPlayerMakerEvent("Drag");
			}
		}
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (_prevDirection != Controller.SwingMovement && Controller.SwingMovement != 0f)
		{
			Controller.SetAnimationBool("ChangeSwingDirection", value: true);
			_prevDirection = Controller.SwingMovement;
		}
		else
		{
			Controller.SetAnimationBool("ChangeSwingDirection", value: false);
		}
		if (Controller.SwingMovement == 0f)
		{
			_rb.AddForce(-_rb.velocity / 4f);
		}
		Vector2 vector = (Controller.CompanionTransform.position - Controller.transform.position).normalized;
		Vector2 vector2 = Vector2.zero;
		if (Controller.SwingMovement > 0f)
		{
			vector2 = new Vector2(vector.y, 0f - vector.x);
		}
		if (Controller.SwingMovement < 0f)
		{
			vector2 = new Vector2(0f - vector.y, vector.x);
		}
		Vector2 force = vector2.normalized * _swingForceAux;
		if (Mathf.Sign(force.x) == Mathf.Sign(_rb.velocity.x))
		{
			if (Controller.SwingMovement != 0f)
			{
				Controller.SetAnimationBool("Swing", value: true);
				_swingForceAux += _additiveForce;
			}
			_rb.AddForce(force, ForceMode2D.Force);
		}
		else if (_swingTimeAux <= 0f)
		{
			_swingForceAux = _swingForce;
			_swingTimeAux = 0.5f;
		}
		else
		{
			_swingTimeAux -= Time.fixedDeltaTime;
		}
		float num = 0f;
		if (Controller.SwingMovement != 0f)
		{
			num = 23f;
			_restTimeAux = _restTime;
		}
		else if (_restTimeAux <= 0f)
		{
			_restTimeAux = _restTime;
			Controller.SetAnimationBool("Swing", value: false);
			if (Controller.CompanionTransform.position.y < Controller.transform.position.y)
			{
				Fall();
			}
		}
		else
		{
			num = 23f;
			_restTimeAux -= Time.fixedDeltaTime;
		}
		Vector2 vector3 = (Controller.transform.position - Controller.CompanionTransform.position).normalized;
		_rb.AddForce(vector3 * num, ForceMode2D.Force);
	}

	public override void Exit()
	{
		base.Exit();
		Controller.Sprite.transform.rotation = Quaternion.identity;
	}
}
