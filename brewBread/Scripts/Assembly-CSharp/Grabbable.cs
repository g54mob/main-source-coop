using System.Collections;
using PenguinPackage.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
	private bool _grabbed;

	[SerializeField]
	private float _range;

	[SerializeField]
	private float _horizontalForce;

	[SerializeField]
	private float _verticalForce;

	[SerializeField]
	private float _throwTime;

	[SerializeField]
	private Vector2 _objectPosition;

	[SerializeField]
	private PenguinActionsController _penguinController;

	public UnityEvent OnObjectGrabbed = new UnityEvent();

	public UnityEvent OnObjectReleased = new UnityEvent();

	private Rigidbody2D _rb;

	private float _force;

	private PenguinActionsController _actionController;

	private bool _throw;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_force = 0f;
	}

	private void Start()
	{
		PenguinInputController.OnGrabInput?.AddListener(GrabAction);
		PenguinInputController.OnUnGrabInput?.AddListener(ReleaseAction);
	}

	private void OnDestroy()
	{
		PenguinInputController.OnGrabInput?.RemoveListener(GrabAction);
	}

	private void Update()
	{
		if (_grabbed && !_throw)
		{
			base.transform.localPosition = _objectPosition;
		}
	}

	private void GrabAction(PenguinInputController controller, PenguinActionsController actionsController)
	{
		if (GrabChecks() && !(controller.transform == base.transform))
		{
			if (base.transform.parent == controller.transform)
			{
				_throw = true;
				_actionController = actionsController;
				StartCoroutine(ChargeThrow());
			}
			else if (!((controller.transform.position - base.transform.position).magnitude > _range))
			{
				GrabObject(controller);
			}
		}
	}

	private void GrabObject(PenguinInputController controller)
	{
		OnObjectGrabbed?.Invoke();
		_grabbed = true;
		base.transform.parent = controller.transform;
		base.transform.localPosition = _objectPosition;
	}

	private void ReleaseAction(PenguinInputController controller, PenguinActionsController actionController)
	{
		if (!(base.transform.parent != controller.transform))
		{
			_throw = false;
		}
	}

	private void ReleaseObject(PenguinActionsController actionsController)
	{
		_grabbed = false;
		base.transform.parent = null;
		OnObjectReleased?.Invoke();
		Mathf.Clamp(_force, 0f, _throwTime);
		float num = Mathf.Lerp(0f, _horizontalForce, Mathp.Remap(_force, 0f, _throwTime));
		float y = Mathf.Lerp(0f, _verticalForce, Mathp.Remap(_force, 0f, _throwTime));
		_rb.velocity = new Vector2(actionsController.transform.localScale.x * num, y);
		_force = 0f;
		_actionController.SetAnimationBool("ThrowObject", value: false);
	}

	private IEnumerator ChargeThrow()
	{
		Vector2 currentPosition = base.transform.localPosition;
		Vector2 goalPosition = currentPosition;
		goalPosition.y -= 0.5f;
		goalPosition.x -= 0.2f;
		_actionController.SetAnimationBool("ThrowObject", value: true);
		while (_throw)
		{
			_force += Time.deltaTime;
			if (_force > _throwTime)
			{
				_throw = false;
			}
			base.transform.localPosition = Vector2.Lerp(currentPosition, goalPosition, _force);
			yield return null;
		}
		ReleaseObject(_actionController);
	}

	private bool GrabChecks()
	{
		if (_penguinController == null)
		{
			return true;
		}
		if (_penguinController.CurrentState.Type != PenguinActions.PullRope && _penguinController.CurrentState.Type != PenguinActions.LandHard && _penguinController.CurrentState.Type != PenguinActions.Jump && _penguinController.Companion.CurrentState.Type != PenguinActions.PullRope)
		{
			return true;
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, _range);
	}
}
