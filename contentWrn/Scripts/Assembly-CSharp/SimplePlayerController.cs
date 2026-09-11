using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
	private Rigidbody rig;

	private SimplePlayer player;

	public float movementForce;

	public float airControl = 0.2f;

	public float gravity = 20f;

	public float drag = 0.9f;

	private bool inited;

	private Vector3 headVel;

	private Vector3 headForward;

	public void Start()
	{
		if (!inited)
		{
			inited = true;
			player = GetComponent<SimplePlayer>();
			rig = GetComponent<Rigidbody>();
			player.data.playerLookValues = HelperFunctions.DirectionToLook(base.transform.forward);
			headForward = player.refs.head.transform.forward;
			SetRotations();
		}
	}

	private void Update()
	{
		if (player.data.isLocalPlayer)
		{
			Look();
		}
		SetRotations();
	}

	private void Look()
	{
		player.data.playerLookValues.x += player.input.lookInput.x;
		player.data.playerLookValues.y += player.input.lookInput.y;
		player.data.playerLookValues.y = Mathf.Clamp(player.data.playerLookValues.y, -80f, 80f);
	}

	private void SetRotations()
	{
		player.data.playerLookForward = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.forward);
		player.data.playerLookRight = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.right);
		player.data.playerLookUp = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.up);
		FRILerp.DirectionSpring(ref headForward, player.data.playerLookForward, 5f, 10f, ref headVel);
		player.refs.head.transform.rotation = Quaternion.LookRotation(headForward);
	}

	private void FixedUpdate()
	{
		if (player.data.isLocalPlayer)
		{
			player.data.isGrounded = GroundCheck();
			ConfigValues();
			if (!player.data.isGrounded)
			{
				Gravity();
			}
			else
			{
				Standing();
			}
			Movement();
			Drag();
		}
	}

	private void Drag()
	{
		if (player.data.isGrounded)
		{
			rig.linearVelocity *= drag;
		}
		else
		{
			rig.linearVelocity *= Mathf.Lerp(1f, drag, airControl);
		}
	}

	private void Standing()
	{
		float y = rig.linearVelocity.y;
		float num = player.data.groundHeight - rig.position.y;
		y += num * 1f;
		rig.linearVelocity = new Vector3(rig.linearVelocity.x, y, rig.linearVelocity.z);
	}

	private void ConfigValues()
	{
		if (player.data.isGrounded)
		{
			player.data.sinceGrounded = 0f;
		}
		else
		{
			player.data.sinceGrounded += Time.fixedDeltaTime;
		}
		player.data.relativeVel = player.refs.head.transform.InverseTransformDirection(SimplePlayer.localPlayer.refs.rig.linearVelocity);
	}

	private void Gravity()
	{
		rig.AddForce(Vector3.down * (gravity * player.data.sinceGrounded), ForceMode.Acceleration);
	}

	private bool GroundCheck()
	{
		RaycastHit raycastHit = HelperFunctions.LineCheck(player.refs.head.position, player.refs.head.position + Vector3.down * (player.refs.head.localPosition.y + 0.1f), HelperFunctions.LayerType.Terrain);
		if (!raycastHit.transform)
		{
			return false;
		}
		player.data.groundHeight = raycastHit.point.y;
		player.data.groundPos = raycastHit.point;
		player.data.groundNormal = raycastHit.normal;
		return true;
	}

	private void Movement()
	{
		Vector3 playerLookForward = player.data.playerLookForward;
		Vector3 playerLookRight = player.data.playerLookRight;
		Vector3 vector = HelperFunctions.GroundDirection(player.data.groundNormal, -playerLookRight);
		Vector3 vector2 = HelperFunctions.GroundDirection(player.data.groundNormal, playerLookForward);
		Vector3 normalized = (vector * player.input.movementInput.y + vector2 * player.input.movementInput.x).normalized;
		player.data.movementVector = normalized;
		Debug.DrawRay(base.transform.position, player.data.movementVector);
		float num = 1f;
		if (player.input.sprintIsPressed)
		{
			num *= 2f;
		}
		if (!player.data.isGrounded)
		{
			num *= airControl;
		}
		rig.AddForce(player.data.movementVector * num * movementForce, ForceMode.Acceleration);
	}
}
