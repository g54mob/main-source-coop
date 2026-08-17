using UnityEngine;

public class HangingCharaceter : MonoBehaviour
{
	private CharacterCTRL controller;

	private Rigidbody2D rb;

	[Header("Companion")]
	public GameObject g_companion;

	public float f_playerDistance;

	[Header("Hanging")]
	public float f_swingForce;

	public float f_swingTime;

	public float f_additiveForce;

	public float f_restTime;

	public float f_restTimeAux;

	private float f_swingForceAux;

	private void Start()
	{
		f_restTimeAux = f_restTime;
		f_swingForceAux = f_swingForce;
		controller = GetComponent<CharacterCTRL>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		f_playerDistance = (base.transform.position - g_companion.transform.position).magnitude;
	}

	private void FixedUpdate()
	{
		if (controller._playerState != PlayerState.HANGING)
		{
			return;
		}
		Vector2 vector = (g_companion.transform.position - base.transform.position).normalized;
		Vector2 vector2 = Vector2.zero;
		if (controller.GetMovement() > 0f)
		{
			vector2 = new Vector2(vector.y, 0f - vector.x);
		}
		if (controller.GetMovement() < 0f)
		{
			vector2 = new Vector2(0f - vector.y, vector.x);
		}
		Vector2 vector3 = vector2.normalized * f_swingForce;
		if (Mathf.Sign(vector3.x) == Mathf.Sign(rb.velocity.x))
		{
			if (controller.GetMovement() != 0f)
			{
				f_swingForce += f_additiveForce;
			}
			rb.AddForce(vector3, ForceMode2D.Force);
			Debug.DrawLine(base.transform.position, base.transform.position + (Vector3)vector3, Color.green);
		}
		else if (f_swingTime <= 0f)
		{
			f_swingForce = f_swingForceAux;
			f_swingTime = 0.2f;
		}
		else
		{
			f_swingTime -= Time.deltaTime;
		}
		float num = 0f;
		if (controller.GetMovement() != 0f)
		{
			num = 23f;
			f_restTime = f_restTimeAux;
		}
		else if (f_restTime <= 0f)
		{
			f_restTime = f_restTimeAux;
			if (g_companion.transform.position.y < base.transform.position.y)
			{
				controller._playerState = PlayerState.JUMPING;
			}
		}
		else
		{
			num = 23f;
			f_restTime -= Time.deltaTime;
		}
		Vector2 vector4 = (base.transform.position - g_companion.transform.position).normalized;
		Debug.DrawLine(base.transform.position, base.transform.position + (Vector3)vector4, Color.red);
		rb.AddForce(vector4 * num, ForceMode2D.Force);
	}
}
