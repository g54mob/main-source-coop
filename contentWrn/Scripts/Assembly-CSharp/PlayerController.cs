using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementForce = 10f;

	public float sprintMultiplier = 2f;

	public bool canSprintInAnyDirection;

	public float bodyRotationTorque = 1f;

	public float maxStamina = 10f;

	public float staminaRegRate = 2f;

	public float staminaReActivationThreshold = 3f;

	public float jumpImpulse;

	public float jumpForceOverTime;

	public float jumpForceDuration = 0.5f;

	private float jumpForceTime;

	public float standForce = 10f;

	public float standForcePerGroundCol;

	public bool groundRaycast = true;

	public bool alwaysGroundRaycast;

	public bool wallClimb;

	public float wallClimbGravityAdjustSpeed;

	public float gravity = 20f;

	public float constantGravity;

	public bool simplifiedRagdoll;

	private Player player;

	private PlayerRagdoll ragdoll;

	private static Collider[] m_colliderHits = new Collider[3];

	private Vector3 halfUp = Vector3.up * 0.5f;

	private void Start()
	{
		player = GetComponent<Player>();
		ragdoll = GetComponent<PlayerRagdoll>();
		player.data.playerLookValues = HelperFunctions.DirectionToLook(base.transform.forward);
		player.data.currentStamina = maxStamina;
	}

	private void Update()
	{
		if (player.data.dead || player.NoControl())
		{
			return;
		}
		if (player.data.isSprinting)
		{
			player.data.currentStamina = Mathf.MoveTowards(player.data.currentStamina, 0f, Time.deltaTime);
			if (player.data.currentStamina < 0.01f)
			{
				player.data.staminaDepleated = true;
			}
		}
		else
		{
			if (player.data.sinceSprint > 1f)
			{
				player.data.currentStamina = Mathf.MoveTowards(player.data.currentStamina, maxStamina, Time.deltaTime);
			}
			if (player.data.currentStamina >= staminaReActivationThreshold * 0.99f)
			{
				player.data.staminaDepleated = false;
			}
		}
		if (player.data.isLocal)
		{
			Look();
			MovementStateChanges();
			if (player.input.jumpWasPressed)
			{
				TryJump();
			}
		}
		SetRotations();
	}

	public void TryJump()
	{
		if (player.data.sinceGrounded < 0.5f && player.data.sinceJump > 0.6f)
		{
			player.refs.view.RPC("RPCA_Jump", RpcTarget.All);
			player.data.isCrouching = false;
		}
	}

	[PunRPC]
	public void RPCA_Jump()
	{
		player.data.sinceJump = 0f;
		player.data.sinceGrounded = 0f;
		jumpForceTime = jumpForceDuration;
		for (int i = 0; i < player.refs.ragdoll.bodypartList.Count; i++)
		{
			Rigidbody rig = player.refs.ragdoll.bodypartList[i].rig;
			Vector3 linearVelocity = rig.linearVelocity;
			linearVelocity.y = Mathf.Clamp(linearVelocity.y, 0f, 100000f);
			rig.linearVelocity = linearVelocity;
		}
		player.refs.ragdoll.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
	}

	private void ApplyJumpForce()
	{
		jumpForceTime -= Time.fixedDeltaTime;
		player.refs.ragdoll.AddForce(Vector3.up * (jumpForceOverTime * (jumpForceTime / jumpForceDuration)), ForceMode.VelocityChange);
	}

	private void MovementStateChanges()
	{
		if (player.input.sprintIsPressed && !player.data.staminaDepleated && (player.input.movementInput.y > 0.1f || canSprintInAnyDirection))
		{
			player.data.isSprinting = true;
		}
		else
		{
			player.data.isSprinting = false;
		}
		if (player.input.crouchWasPressed)
		{
			player.data.isCrouching = !player.data.isCrouching;
		}
		if (player.data.isSprinting)
		{
			player.data.isCrouching = false;
		}
	}

	private void Look()
	{
		if (player.HangingUpsideDown())
		{
			player.data.playerLookValues.x -= player.input.lookInput.x;
			player.data.playerLookValues.y += player.input.lookInput.y;
		}
		else
		{
			player.data.playerLookValues.x += player.input.lookInput.x;
			player.data.playerLookValues.y += player.input.lookInput.y;
		}
		player.data.playerLookValues.y = Mathf.Clamp(player.data.playerLookValues.y, -80f, 80f);
	}

	private void SetRotations()
	{
		player.data.lookDirection = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.forward);
		player.data.lookDirectionRight = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.right);
		player.data.lookDirectionUp = HelperFunctions.LookToDirection(player.data.playerLookValues, Vector3.up);
	}

	private void FixedUpdate()
	{
		if (player.Ragdoll() || !player.data.physicsAreReady)
		{
			return;
		}
		if (player.data.simplifiedRagdoll && player.refs.view.IsMine)
		{
			SimpleMovement();
			simplifiedRagdoll = true;
			return;
		}
		simplifiedRagdoll = false;
		if (!player.data.carried)
		{
			ConstantGravity();
			if (!player.data.isGrounded)
			{
				Gravity();
			}
			else
			{
				Standing();
			}
		}
		Movement();
		BodyRotation();
		if (jumpForceTime > 0f)
		{
			ApplyJumpForce();
		}
	}

	private void SimpleMovement()
	{
		if (player.data.framesSinceBotTeleport < 10)
		{
			return;
		}
		Transform transform = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.transform;
		RaycastHit hit = player.refs.ragdoll.GroundRaycast(justChecking: false, justTryingToGetTheRaycastHit: true, ignoreAngle: true);
		if (!hit.transform)
		{
			hit = HelperFunctions.LineCheck(player.data.lastSimplifiedPosition, transform.position, HelperFunctions.LayerType.TerrainProp);
		}
		Vector3 zero = Vector3.zero;
		if ((bool)hit.transform)
		{
			player.refs.ragdoll.HandleGroundCollision(player.refs.ragdoll.RaycastHitToBodyPartCollision(hit, player.refs.ragdoll.GetBodypart(BodypartType.Hip)));
			float num = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position.y - hit.point.y;
			zero.y += player.data.targetHeight - num;
			player.data.isGrounded = true;
			float num2 = Vector3.Dot(base.transform.forward, hit.normal);
			if (num2 > 0.05f)
			{
				zero.y += Mathf.Clamp01(num2 * 0.5f);
			}
		}
		else
		{
			player.data.isGrounded = false;
			zero += Time.fixedDeltaTime * player.data.sinceGrounded * 10f * player.data.gravityDirection;
		}
		player.refs.simpleCollider.enabled = true;
		player.refs.simpleCollider.transform.position = transform.position + Vector3.up * player.data.simplifiedColliderHeight;
		int num3 = Physics.OverlapSphereNonAlloc(transform.position + halfUp, 1f, m_colliderHits, HelperFunctions.GetMask(HelperFunctions.LayerType.TerrainProp));
		for (int i = 0; i < num3; i++)
		{
			Physics.ComputePenetration(player.refs.simpleCollider, player.refs.simpleCollider.transform.position, player.refs.simpleCollider.transform.rotation, m_colliderHits[i], m_colliderHits[i].transform.position, m_colliderHits[i].transform.rotation, out var direction, out var distance);
			zero += direction * distance;
		}
		player.refs.simpleCollider.enabled = false;
		float num4 = movementForce * 0.15f * Time.fixedDeltaTime;
		if (player.data.isSprinting)
		{
			num4 *= 2f;
		}
		zero += player.data.lookDirection.Flat().normalized * (num4 * player.input.movementInput.y);
		zero += player.data.lookDirectionRight.Flat().normalized * (num4 * player.input.movementInput.x);
		if (float.IsNaN(zero.x))
		{
			zero.x = 0f;
		}
		transform.position += zero;
		player.data.lastSimplifiedPosition = player.refs.simpleCollider.transform.position;
	}

	private void ConstantGravity()
	{
		float num = constantGravity;
		if (player.data.possession > 0.1f)
		{
			num += player.data.possession * -0.1f;
		}
		if (!Mathf.Approximately(num, 0f))
		{
			Vector3 force = player.data.gravityDirection * constantGravity;
			for (int i = 0; i < ragdoll.rigList.Count; i++)
			{
				ragdoll.rigList[i].AddForce(force, ForceMode.Acceleration);
			}
		}
	}

	private void BodyRotation()
	{
		Bodypart bodypart = player.refs.ragdoll.GetBodypart(BodypartType.Torso);
		if ((bool)bodypart)
		{
			HelperFunctions.PhysicsRotateTowards(bodypart.rig, bodypart.rig.transform.forward, bodypart.animationTarget.transform.forward, bodyRotationTorque);
		}
		Bodypart bodypart2 = player.refs.ragdoll.GetBodypart(BodypartType.Hip);
		if ((bool)bodypart2)
		{
			HelperFunctions.PhysicsRotateTowards(bodypart2.rig, bodypart2.rig.transform.forward, bodypart2.animationTarget.transform.forward, bodyRotationTorque);
			HelperFunctions.PhysicsRotateTowards(bodypart2.rig, bodypart2.rig.transform.up, bodypart2.animationTarget.transform.up, bodyRotationTorque);
		}
	}

	private void Movement()
	{
		if (!(player.data.currentBed != null) && (player.ai || !player.HasLockedMovement()))
		{
			Vector3 lookDirection = player.data.lookDirection;
			Vector3 lookDirectionRight = player.data.lookDirectionRight;
			lookDirection.y = 0f;
			lookDirection.Normalize();
			Vector3 planeNormal = player.data.groundNormal;
			if (player.data.sinceGrounded > 0.2f)
			{
				planeNormal = -player.data.gravityDirection;
			}
			Vector3 vector = HelperFunctions.GroundDirection(planeNormal, -lookDirectionRight);
			Vector3 vector2 = HelperFunctions.GroundDirection(planeNormal, lookDirection);
			Vector3 vector3 = vector * player.input.movementInput.y + vector2 * player.input.movementInput.x;
			vector3 = Vector3.ClampMagnitude(vector3, 1f);
			if (wallClimb)
			{
				vector3 = player.data.lookDirection;
			}
			player.data.movementVector = vector3;
			float num = 1f;
			if (player.data.isSprinting)
			{
				num *= sprintMultiplier;
			}
			else if (player.data.isCrouching)
			{
				num *= 0.6f;
			}
			if ((bool)player.data.currentSeat || player.data.strangledForSeconds > 0f)
			{
				num *= 0.1f;
			}
			num *= player.data.movementSlowFactor;
			ragdoll.AddForce(player.data.movementVector * (num * movementForce), ForceMode.Acceleration);
		}
	}

	private void Standing()
	{
		if (!(player.data.possession > 0.1f) && !(player.data.currentBed != null) && !player.data.currentSeat)
		{
			float num = player.data.targetHeight - player.data.distanceToGround;
			num = Mathf.Clamp(num * 5f, -1f, 1f);
			if (player.data.StandingStill() && player.data.currentBed == null)
			{
				num = Mathf.Clamp01(num);
			}
			float num2 = standForce;
			num2 += (float)player.data.nrOfGroundCols * standForcePerGroundCol;
			ragdoll.AddForce(-player.data.gravityDirection * (num * num2), ForceMode.Acceleration);
		}
	}

	private void Gravity()
	{
		if (!player.data.currentSeat && !(player.data.possession > 0.1f) && !(player.data.currentBed != null))
		{
			Vector3 force = player.data.gravityDirection * (gravity * player.data.sinceGrounded);
			for (int i = 0; i < ragdoll.rigList.Count; i++)
			{
				ragdoll.rigList[i].AddForce(force, ForceMode.Acceleration);
			}
		}
	}
}
