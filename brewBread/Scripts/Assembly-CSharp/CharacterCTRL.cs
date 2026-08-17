using UnityEngine;

public class CharacterCTRL : MonoBehaviour
{
	private float f_movement;

	private bool b_isGrounded;

	private float f_bufferTime = 0.2f;

	private bool jump;

	public bool b_isGrabbing;

	private Rigidbody2D rb;

	[Header("Movement Values")]
	public float f_acceleration;

	public float f_speed;

	public float f_jumpForce;

	public float f_maxVelocity;

	[Header("Ground Check")]
	public Transform t_groundCheck;

	public float f_radiusCheck;

	public LayerMask l_groundLayer;

	[Header("Inputs")]
	public KeyCode k_up;

	public KeyCode k_down;

	public KeyCode k_left;

	public KeyCode k_right;

	[Header("Companion")]
	public GameObject g_companion;

	public float f_playerDistance;

	public PlayerState _playerState;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		f_playerDistance = (base.transform.position - g_companion.transform.position).magnitude;
		b_isGrounded = IsGrounded();
		SetState();
		InputManagement();
	}

	private void FixedUpdate()
	{
		if (_playerState != PlayerState.HANGING)
		{
			base.transform.position += new Vector3(f_movement, 0f, 0f) * f_speed * Time.deltaTime;
		}
		FixVelocity();
	}

	private void SetState()
	{
		if (_playerState != PlayerState.FALLING)
		{
			if (b_isGrounded)
			{
				if (f_movement == 0f)
				{
					if (Input.GetKey(k_down))
					{
						_playerState = PlayerState.WEIGHT;
					}
					else
					{
						_playerState = PlayerState.IDLE;
					}
				}
				else
				{
					_playerState = PlayerState.WALK;
				}
			}
			else
			{
				if ((g_companion.GetComponent<CharacterCTRL>().GetGrounded() || g_companion.GetComponent<CharacterCTRL>()._playerState == PlayerState.ATTACHED) && base.transform.position.y < g_companion.transform.position.y && f_playerDistance > 2.4f)
				{
					_playerState = PlayerState.HANGING;
				}
				if (_playerState != PlayerState.HANGING || (!g_companion.GetComponent<CharacterCTRL>().GetGrounded() && g_companion.GetComponent<CharacterCTRL>()._playerState != PlayerState.ATTACHED))
				{
					_playerState = PlayerState.JUMPING;
				}
			}
			if (b_isGrabbing)
			{
				_playerState = PlayerState.ATTACHED;
			}
		}
		else
		{
			if (b_isGrounded)
			{
				_playerState = PlayerState.IDLE;
				f_movement = 0f;
			}
			if (g_companion.GetComponent<CharacterCTRL>()._playerState == PlayerState.ATTACHED)
			{
				_playerState = PlayerState.HANGING;
			}
		}
	}

	private void InputManagement()
	{
		if (_playerState != PlayerState.WEIGHT && _playerState != PlayerState.FALLING)
		{
			if (Input.GetKey(k_left))
			{
				if (f_movement > 0f)
				{
					f_movement = 0f;
				}
				if (f_movement > -1f)
				{
					f_movement -= f_acceleration * Time.deltaTime;
				}
				else
				{
					f_movement = -1f;
				}
			}
			else if (Input.GetKey(k_right))
			{
				if (f_movement < 0f)
				{
					f_movement = 0f;
				}
				if (f_movement < 1f)
				{
					f_movement += f_acceleration * Time.deltaTime;
				}
				else
				{
					f_movement = 1f;
				}
			}
			else
			{
				f_movement = 0f;
				if (b_isGrounded && _playerState != PlayerState.ATTACHED)
				{
					_playerState = PlayerState.IDLE;
				}
			}
			if (Input.GetKeyDown(k_up) && b_isGrounded)
			{
				rb.velocity = Vector2.up * f_jumpForce;
			}
		}
		if (_playerState == PlayerState.WEIGHT)
		{
			rb.mass = 100f;
			f_movement = 0f;
		}
		else
		{
			rb.mass = 0.5f;
		}
		JumpInputBuffer();
	}

	private void FixVelocity()
	{
		if (rb.velocity.x > f_maxVelocity)
		{
			rb.velocity = new Vector2(f_maxVelocity, rb.velocity.y);
		}
		if (rb.velocity.x < 0f - f_maxVelocity)
		{
			rb.velocity = new Vector2(0f - f_maxVelocity, rb.velocity.y);
		}
	}

	private bool IsGrounded()
	{
		bool flag = true;
		BoxCollider2D component = GetComponent<BoxCollider2D>();
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(component.bounds.center, component.bounds.size * 0.9f, 0f, Vector2.down, 0.5f, l_groundLayer);
		flag = (bool)raycastHit2D && ((raycastHit2D.collider.tag != "Slope") ? true : false);
		Debug.DrawRay(component.bounds.center + new Vector3(component.bounds.extents.x, 0f), Vector2.down * (component.bounds.extents.y + 0.5f), Color.red);
		Debug.DrawRay(component.bounds.center - new Vector3(component.bounds.extents.x, 0f), Vector2.down * (component.bounds.extents.y + 0.5f), Color.red);
		Debug.DrawRay(component.bounds.center - new Vector3(component.bounds.extents.x, component.bounds.extents.y + 0.5f), Vector2.right * component.bounds.extents.x, Color.red);
		return flag;
	}

	private void JumpInputBuffer()
	{
		if (_playerState == PlayerState.WEIGHT)
		{
			if (Input.GetKeyDown(k_up))
			{
				jump = true;
				f_bufferTime = 0.2f;
			}
			else
			{
				f_bufferTime -= Time.deltaTime;
			}
			if (f_bufferTime <= 0f)
			{
				jump = false;
			}
		}
		else if (jump)
		{
			jump = false;
			rb.velocity = Vector2.up * f_jumpForce;
		}
	}

	public float GetMovement()
	{
		return f_movement;
	}

	public bool GetGrounded()
	{
		return b_isGrounded;
	}
}
