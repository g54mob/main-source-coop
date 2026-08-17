using UnityEngine;
using UnityEngine.UI;

public class WallGrabCharacter : MonoBehaviour
{
	private bool b_wallAttached;

	private bool b_ableToAttach = true;

	private GameObject g_companion;

	private Rigidbody2D rb;

	private CharacterCTRL controller;

	private float grabTimerAux;

	[Header("General")]
	public KeyCode k_hang;

	public float f_radiusCheck;

	public LayerMask l_groundLayer;

	public SpriteRenderer renderer;

	[Header("Grab Timers")]
	public float grabTimer;

	public float increasedValue;

	[Header("Image")]
	public Image staminaMeter;

	[Header("Wall Check")]
	public Transform t_wallCheckL;

	public Transform t_wallCheckR;

	private void Start()
	{
		grabTimerAux = grabTimer;
		rb = GetComponent<Rigidbody2D>();
		controller = GetComponent<CharacterCTRL>();
		g_companion = controller.g_companion;
	}

	private void Update()
	{
		b_wallAttached = false;
		Vector2 vector = Vector2.zero;
		if (controller.GetGrounded())
		{
			if (!b_ableToAttach)
			{
				b_ableToAttach = controller.GetGrounded();
			}
			grabTimer = grabTimerAux;
		}
		if (Input.GetKey(k_hang))
		{
			if ((bool)Physics2D.OverlapCircle(t_wallCheckL.position, f_radiusCheck, l_groundLayer) && renderer.flipX)
			{
				if (b_ableToAttach)
				{
					b_wallAttached = true;
				}
				vector = new Vector2(Physics2D.Raycast(base.transform.position, Vector2.left, 10f, l_groundLayer).point.x + 0.4f, base.transform.position.y);
			}
			if ((bool)Physics2D.OverlapCircle(t_wallCheckR.position, f_radiusCheck, l_groundLayer) && !renderer.flipX)
			{
				if (b_ableToAttach)
				{
					b_wallAttached = true;
				}
				vector = new Vector2(Physics2D.Raycast(base.transform.position, Vector2.right, 10f, l_groundLayer).point.x - 0.4f, base.transform.position.y);
			}
		}
		if (b_wallAttached)
		{
			base.transform.position = vector;
			rb.velocity = Vector2.zero;
			rb.gravityScale = 0f;
			rb.constraints = RigidbodyConstraints2D.FreezePosition;
			rb.bodyType = RigidbodyType2D.Kinematic;
			controller.b_isGrabbing = true;
			if (!g_companion.GetComponent<CharacterCTRL>().GetGrounded() && g_companion.transform.position.y < base.transform.position.y && controller.f_playerDistance >= 2.49f)
			{
				grabTimer -= Time.deltaTime * increasedValue;
			}
			else
			{
				grabTimer -= Time.deltaTime;
			}
			if (grabTimer <= 0f)
			{
				b_wallAttached = false;
				b_ableToAttach = false;
			}
		}
		else
		{
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.constraints = RigidbodyConstraints2D.None;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			rb.gravityScale = 5f;
			controller.b_isGrabbing = false;
		}
		StaminaController();
	}

	private void StaminaController()
	{
		if (b_wallAttached)
		{
			staminaMeter.enabled = true;
			float fillAmount = grabTimer / grabTimerAux;
			staminaMeter.fillAmount = fillAmount;
		}
		else
		{
			staminaMeter.enabled = false;
		}
	}
}
