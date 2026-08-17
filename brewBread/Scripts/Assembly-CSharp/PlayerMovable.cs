using UnityEngine;

public class PlayerMovable : MonoBehaviour
{
	private bool _inCart;

	private PenguinActionsController _penguinController;

	private BoxCollider2D _collider;

	private Transform _previousParent;

	[SerializeField]
	private Grabbable _grabbable;

	private bool _isGrabbed;

	[SerializeField]
	private LayerMask _groundLayer;

	private void Start()
	{
		_collider = GetComponent<BoxCollider2D>();
		_penguinController = GetComponent<PenguinActionsController>();
		if (_grabbable != null)
		{
			_grabbable.OnObjectGrabbed.AddListener(ObjectGrabbed);
			_grabbable.OnObjectReleased.AddListener(ObjectReleased);
		}
	}

	private void ObjectReleased()
	{
		_isGrabbed = false;
	}

	private void ObjectGrabbed()
	{
		_isGrabbed = true;
	}

	private void FixedUpdate()
	{
		if (!_inCart || _penguinController.CurrentState.Type != PenguinActions.Anchor)
		{
			return;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.right, _collider.bounds.extents.x, _groundLayer);
		if ((bool)raycastHit2D)
		{
			float x = raycastHit2D.point.x - (base.transform.position.x + _collider.bounds.extents.x);
			base.transform.position += new Vector3(x, 0f, 0f);
			return;
		}
		raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.left, _collider.bounds.extents.x, _groundLayer);
		if ((bool)raycastHit2D)
		{
			float x2 = raycastHit2D.point.x - (base.transform.position.x - _collider.bounds.extents.x);
			base.transform.position += new Vector3(x2, 0f, 0f);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer != 10 || !(collision.transform.tag == "Movable"))
		{
			return;
		}
		RaycastHit2D[] array = Physics2D.RaycastAll(base.transform.position, Vector2.down, 1f);
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i] && array[i].collider.tag == "Movable")
			{
				_inCart = true;
				_previousParent = base.transform.parent;
				base.transform.parent = array[i].transform;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 10 && collision.transform.tag == "Movable")
		{
			if (!_isGrabbed)
			{
				base.transform.parent = _previousParent;
			}
			_inCart = false;
		}
	}
}
