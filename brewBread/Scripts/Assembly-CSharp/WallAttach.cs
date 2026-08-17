using UnityEngine;

public class WallAttach : MonoBehaviour
{
	private Rigidbody2D _rb;

	private PenguinActionsController _penguinActionsController;

	[SerializeField]
	private float _minimumSpeed;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_penguinActionsController = GetComponent<PenguinActionsController>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 10 && !(collision.transform.tag == "Platform") && !(_rb.velocity.y <= -1f) && Mathf.Abs(_rb.velocity.y) >= _minimumSpeed)
		{
			_penguinActionsController.SendPlayerMakerEvent("Grab");
		}
	}
}
