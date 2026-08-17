using UnityEngine;

[CreateAssetMenu(fileName = "PenguinGrab", menuName = "Actions/GrabJeff", order = 1)]
public class GrabJeffAction : GrabAction
{
	[Space(10f)]
	[SerializeField]
	private float _bottomMargin;

	[SerializeField]
	private float _fallSpeed;

	[SerializeField]
	private float _precision;

	private float _targetYPosition;

	private Vector2 _targetPosition;

	private bool _goDown;

	private float _t;

	public override void Enter()
	{
		base.Enter();
		if (Controller.Companion.CurrentState.Type == PenguinActions.PullRope)
		{
			Fall();
			return;
		}
		_t = 0f;
		Vector2 origin = _controllerTransform.position;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(_controllerTransform.position, new Vector2(_controllerTransform.localScale.x, 0f), _grabRange, Controller.GroundLayer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(_controllerTransform.position, Vector2.down, _grabRange, Controller.GroundLayer);
		while ((bool)raycastHit2D && !raycastHit2D2)
		{
			_targetYPosition = origin.y;
			origin.y -= _precision;
			raycastHit2D = Physics2D.Raycast(origin, new Vector2(_controllerTransform.localScale.x, 0f), _grabRange, Controller.GroundLayer);
			raycastHit2D2 = Physics2D.Raycast(origin, Vector2.down, _grabRange, Controller.GroundLayer);
		}
		_targetYPosition += _bottomMargin;
		if (_targetYPosition < _controllerTransform.position.y)
		{
			_goDown = true;
			_targetPosition = new Vector2(_controllerTransform.position.x, _targetYPosition);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (Controller.CompanionTransform.position.y > _controllerTransform.position.y && Controller.CurrentPenguinDistance >= Controller.MaxPenguinDistance && Controller.Companion.IsGrounded)
		{
			Fall();
		}
		if (Controller.Companion.CurrentState.Type == PenguinActions.PullRope)
		{
			Fall();
		}
		if (_goDown)
		{
			if (_t >= 1f)
			{
				_goDown = false;
				_controllerTransform.position = Vector2.Lerp(_controllerTransform.position, _targetPosition, 1f);
			}
			_t += Time.deltaTime / _fallSpeed;
			_controllerTransform.position = Vector2.Lerp(_controllerTransform.position, _targetPosition, _t);
		}
	}
}
