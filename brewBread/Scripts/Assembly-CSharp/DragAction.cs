using UnityEngine;

[CreateAssetMenu(fileName = "PenguinDrag", menuName = "Actions/Drag", order = 1)]
public class DragAction : PenguinAction
{
	private float _initialPosY;

	private float _direction;

	private Vector2 _directionVector;

	[SerializeField]
	private float _dragForce;

	[SerializeField]
	private float _definitiveFallTimer;

	[SerializeField]
	private float _pullForce;

	[SerializeField]
	private float _timeWithoutTouchingWall;

	private float _dragForceAux;

	private float _definitiveFallTimerAux;

	private Countdown _countdown;

	private Vector2 _verticalForce;

	private bool _definitiveFall;

	public override void Initialize()
	{
		base.Initialize();
		_countdown = null;
		_verticalForce = new Vector2(0f, 10f * _pullForce);
		_definitiveFall = false;
		_definitiveFallTimerAux = _definitiveFallTimer;
	}

	public override void Enter()
	{
		base.Enter();
		_initialPosY = Controller.CompanionTransform.position.y;
		_direction = Mathf.Sign(Controller.CompanionTransform.position.x - _controllerTransform.position.x);
		_directionVector = new Vector2(_direction, 0f);
		_dragForceAux = _dragForce;
		_definitiveFall = false;
		Controller.Companion.SetAnimationInt("WalkState", 1);
	}

	public override void LogicUpdate()
	{
		RotateCharacter();
		bool flag = ((_direction > 0f) ? (Controller.CompanionTransform.position.x + 0.3f < _controllerTransform.position.x) : (Controller.CompanionTransform.position.x - 0.3f > _controllerTransform.position.x));
		if (_controllerTransform.position.y > Controller.CompanionTransform.position.y + 0.1f || Controller.CompanionTransform.position.y < _initialPosY - 0.1f || flag)
		{
			Fall();
		}
		if (!Physics2D.Raycast(_controllerTransform.position, _directionVector, 0.6f, Controller.GroundLayer))
		{
			if (!_countdown)
			{
				_countdown = StaticInstance<TwoWayCountdown>.Instance.StartNewTimer(_timeWithoutTouchingWall);
				_countdown.Ended.AddListener(StopDraggin);
			}
		}
		else if (!_countdown)
		{
			StaticInstance<TwoWayCountdown>.Instance.StopTimer(_countdown);
		}
		base.LogicUpdate();
	}

	private void StopDraggin()
	{
		Controller.SendPlayerMakerEvent("Drag");
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (Mathf.Sign(Controller.Companion.Movement) != _direction || Controller.Companion.Movement == 0f || _definitiveFall)
		{
			if (!Controller.Companion.Flags.HasFlag(PenguinFlags.Immobile))
			{
				Controller.CompanionTransform.position -= new Vector3(_dragForceAux * _direction, 0f) * Time.fixedDeltaTime;
				_dragForceAux += 1.5f * Time.fixedDeltaTime;
			}
		}
		else if (!Physics2D.Raycast(Controller.CompanionTransform.position, _directionVector, 0.6f, Controller.GroundLayer))
		{
			Controller.Rb.AddForce(_verticalForce);
		}
		else
		{
			_definitiveFallTimerAux -= Time.deltaTime;
			if (_definitiveFallTimerAux < 0f)
			{
				_definitiveFall = true;
			}
		}
	}

	public override void Exit()
	{
		base.Exit();
		Controller.Sprite.transform.rotation = Quaternion.identity;
		Controller.Companion.SetAnimationInt("WalkState", 0);
	}

	public override void Clean()
	{
		base.Clean();
	}
}
