using UnityEngine;

[CreateAssetMenu(fileName = "PenguinFall", menuName = "Actions/FallJeff", order = 1)]
public class FallJeffAction : FallAction
{
	[SerializeField]
	private float _platformCheckRange;

	private Collider2D _platform;

	private Collider2D _penguinCollider;

	private bool _ignored;

	public override void Initialize()
	{
		base.Initialize();
		_penguinCollider = Controller.GetComponent<Collider2D>();
		_platform = null;
	}

	public override void Enter()
	{
		base.Enter();
		_grounded = false;
		_ignored = false;
		_platform = null;
		if (Controller.Companion.CurrentState.Type != PenguinActions.Anchor)
		{
			return;
		}
		RaycastHit2D[] array = Physics2D.RaycastAll(_controllerTransform.position, Vector2.down, _platformCheckRange);
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit2D raycastHit2D = array[i];
			if (raycastHit2D.transform.tag == "Platform")
			{
				_platform = raycastHit2D.collider;
				break;
			}
		}
		if ((bool)_platform)
		{
			Physics2D.IgnoreCollision(_penguinCollider, _platform, ignore: true);
			_ignored = false;
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if ((bool)_platform && !_ignored)
		{
			if (_controllerTransform.position.y < _platform?.transform.position.y)
			{
				_ignored = true;
				Physics2D.IgnoreCollision(_penguinCollider, _platform, ignore: false);
			}
		}
		else if (Controller.Rb.velocity.y <= 0f)
		{
			_grounded = true;
		}
	}

	public override void Exit()
	{
		base.Exit();
		if ((bool)_platform)
		{
			Physics2D.IgnoreCollision(_penguinCollider, _platform, ignore: false);
		}
		_grounded = true;
	}
}
