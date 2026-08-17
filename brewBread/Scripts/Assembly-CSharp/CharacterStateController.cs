using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStateController : MonoBehaviour
{
	private struct myInput
	{
		private InputAction.CallbackContext context;

		private Inputs inputType;

		private float time;

		public InputAction.CallbackContext Context => context;

		public Inputs InputType => inputType;

		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		public myInput(InputAction.CallbackContext context, Inputs inputType)
		{
			this = default(myInput);
			this.context = context;
			this.inputType = inputType;
			time = 0f;
		}
	}

	private StateMachine machineController;

	[Header("General")]
	[Tooltip("Movement speed of the player")]
	public float speed;

	[Tooltip("Force added to the player when jumping")]
	public float jumpForce;

	[Tooltip("Max speed for the penguins")]
	[SerializeField]
	private float maxSpeed;

	[Tooltip("Distance the player must be from the ground to be detected as grounded")]
	[SerializeField]
	private float groundDetectionRange;

	[Tooltip("Unity's layer for terrain")]
	public LayerMask groundLayer;

	[Header("States")]
	public GroundedState grounded;

	public JumpState jump;

	public HangState hang;

	public FallState fall;

	public HardFallState hardFall;

	public WeightState weight;

	public LandState land;

	public GrabState grab;

	public WallFallState wallFall;

	[Header("Emotes")]
	public EmoteController emoteController;

	[SerializeField]
	public Animator _skinAnimator;

	[HideInInspector]
	public Rigidbody2D rb;

	[HideInInspector]
	public float movement;

	[HideInInspector]
	public Animator animator;

	[HideInInspector]
	public GameObject companion;

	[HideInInspector]
	public SpriteRenderer spriteRenderer;

	[HideInInspector]
	public float timerAux;

	[HideInInspector]
	public InputDevice inputDevice;

	[HideInInspector]
	public GameManager manager;

	[HideInInspector]
	public bool enableInput = true;

	[HideInInspector]
	public bool Jump;

	private CharacterStateController _companionStateController;

	private bool isGrounded;

	private bool isWeight;

	private float grabbingTimer;

	private string EmoteName;

	private Coroutine pullRopeCoroutine;

	private Coroutine releaseRopeCoroutine;

	private bool _isPullingRope;

	public bool IsPullingRope => _isPullingRope;

	public CharacterStateController CompanionStateController
	{
		get
		{
			return _companionStateController;
		}
		set
		{
			_companionStateController = value;
		}
	}

	public event Action<Transform> GrabInputEnabled;

	public event Action<Transform> GrabInputDisabled;

	private void Awake()
	{
		enableInput = true;
	}

	private void Start()
	{
		_ = base.gameObject.name;
		machineController = new StateMachine();
		StaticInstance<Pause>.Instance.onGamePaused.AddListener(DisableInput);
		StaticInstance<Pause>.Instance.onGameResumed.AddListener(EnableInput);
		grounded.Initialize(this, machineController);
		jump.Initialize(this, machineController);
		weight.Initialize(this, machineController);
		hardFall.Initialize(this, machineController);
		fall.Initialize(this, machineController);
		hang.Initialize(this, machineController);
		land.Initialize(this, machineController);
		grab.Initialize(this, machineController);
		wallFall.Initialize(this, machineController);
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		machineController.Initialize(grounded);
		manager = UnityEngine.Object.FindObjectOfType<GameManager>();
		CompanionStateController = companion.GetComponent<CharacterStateController>();
	}

	private void DisableInput()
	{
		enableInput = false;
	}

	private void EnableInput()
	{
		enableInput = true;
	}

	private void Update()
	{
		if (enableInput)
		{
			machineController.CurrentState.LogicUpdate();
		}
		if (machineController.CurrentState != grab && machineController.CurrentState != weight)
		{
			if (movement < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (movement > 0f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		if (machineController.CurrentState != grab && timerAux > 0f)
		{
			timerAux -= Time.deltaTime;
		}
		if (machineController.CurrentState == grounded)
		{
			timerAux = 0f;
		}
		if ((double)GetDistanceToCompanion() > 2.49 && (GetCompanionGrounded() || GetCompanionState() == CharacterStates.GRAB) && companion.transform.position.y > base.transform.position.y)
		{
			machineController.CurrentState.HandleInput(Inputs.HANG);
		}
		if (machineController.CurrentState != grab)
		{
			if (grabbingTimer > 0f)
			{
				grabbingTimer -= Time.deltaTime;
				machineController.CurrentState.HandleInput(Inputs.GRAB);
			}
		}
		else
		{
			grabbingTimer = 0f;
		}
		if (spriteRenderer.material.GetFloat("Vector1_ed0278251b2f42e8b7484ceb6b45bd6c") > 0f && machineController.CurrentState != grab)
		{
			float num = timerAux / grab.grabTimer;
			spriteRenderer.material.SetFloat("Vector1_ed0278251b2f42e8b7484ceb6b45bd6c", num);
			if (num <= 0f)
			{
				spriteRenderer.material.SetFloat("Vector1_ed0278251b2f42e8b7484ceb6b45bd6c", 0f);
				StartCoroutine(WhiteFlash());
			}
		}
		DetectGround();
		WeightInput();
	}

	private void FixedUpdate()
	{
		if (enableInput)
		{
			machineController.CurrentState.PhysicsUpdate();
			FixVelocity();
		}
	}

	public void SetMovement(InputAction.CallbackContext context)
	{
		if (enableInput && !IsPullingRope)
		{
			StartCoroutine(MovementAcceleration(movement, context.ReadValue<float>(), GetFriction()));
		}
		else if (!enableInput)
		{
			movement = 0f;
		}
	}

	public void HandleJump(InputAction.CallbackContext context)
	{
		if (context.started && enableInput && !IsPullingRope)
		{
			machineController.CurrentState.HandleInput(Inputs.JUMP);
			Jump = true;
		}
	}

	public void HandleGrab(InputAction.CallbackContext context)
	{
		if (context.started && enableInput)
		{
			this.GrabInputEnabled?.Invoke(base.transform);
			grabbingTimer = 0.2f;
		}
		else if (context.canceled && machineController.CurrentState == grab && enableInput)
		{
			this.GrabInputDisabled?.Invoke(base.transform);
			machineController.CurrentState.HandleInput(Inputs.GRAB);
		}
		else if (context.canceled)
		{
			this.GrabInputDisabled?.Invoke(base.transform);
		}
	}

	public void HandleWeight(InputAction.CallbackContext context)
	{
		if (context.started && enableInput && !IsPullingRope)
		{
			isWeight = true;
		}
		if (context.canceled)
		{
			isWeight = false;
		}
	}

	public void HandleRopePulling(InputAction.CallbackContext context)
	{
		if (!StaticInstance<AssistModeManager>.Instance.PullingRope || !isGrounded || _companionStateController.IsPullingRope)
		{
			return;
		}
		if (context.started && enableInput)
		{
			if (releaseRopeCoroutine != null)
			{
				StopCoroutine(releaseRopeCoroutine);
			}
			pullRopeCoroutine = StartCoroutine(PullRope());
		}
		if (context.canceled)
		{
			if (pullRopeCoroutine != null)
			{
				StopCoroutine(pullRopeCoroutine);
			}
			releaseRopeCoroutine = StartCoroutine(ReleaseRope());
		}
	}

	public void HandleLoad(InputAction.CallbackContext context)
	{
		if (StaticInstance<AssistModeManager>.Instance.CheckPoints && context.started)
		{
			StaticInstance<CheckpointController>.Instance.LoadGame();
		}
	}

	public void HandleSave(InputAction.CallbackContext context)
	{
		if (StaticInstance<AssistModeManager>.Instance.CheckPoints && context.started)
		{
			StaticInstance<CheckpointController>.Instance.SaveGame();
		}
	}

	public void PlayEmote(InputAction.CallbackContext context)
	{
		if (context.started && enableInput && !IsPullingRope)
		{
			StartCoroutine(PlayEmoteAnimation());
		}
	}

	public void SetEmoteString(string name)
	{
		EmoteName = name;
	}

	private void WeightInput()
	{
		if (isWeight)
		{
			if (GetCurrentState() != CharacterStates.WEIGHT)
			{
				machineController.CurrentState.HandleInput(Inputs.WEIGHT);
			}
		}
		else if (GetCurrentState() == CharacterStates.WEIGHT)
		{
			machineController.CurrentState.HandleInput(Inputs.WEIGHT);
		}
	}

	private bool DetectGround()
	{
		Vector2 vector = new Vector2(0f - (GetComponent<BoxCollider2D>().size.x / 2f - GetComponent<BoxCollider2D>().offset.x), 0f - (GetComponent<BoxCollider2D>().size.y / 2f - GetComponent<BoxCollider2D>().offset.y));
		Vector2 vector2 = new Vector2(0f + (GetComponent<BoxCollider2D>().size.x / 2f + GetComponent<BoxCollider2D>().offset.x), 0f - (GetComponent<BoxCollider2D>().size.y / 2f - GetComponent<BoxCollider2D>().offset.y));
		vector = base.transform.TransformPoint(vector);
		vector2 = base.transform.TransformPoint(vector2);
		if ((bool)Physics2D.Raycast(vector, Vector2.down, groundDetectionRange, groundLayer) || (bool)Physics2D.Raycast(vector2, Vector2.down, groundDetectionRange, groundLayer))
		{
			if (!IsInSlope())
			{
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
			}
		}
		else
		{
			isGrounded = false;
		}
		return isGrounded;
	}

	private float GetFriction()
	{
		float result = 1f;
		Vector2 vector = new Vector2(0f, 0f - (GetComponent<BoxCollider2D>().size.y / 2f - GetComponent<BoxCollider2D>().offset.y));
		vector = base.transform.TransformPoint(vector);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.down, groundDetectionRange, groundLayer);
		if ((bool)raycastHit2D)
		{
			raycastHit2D.collider.TryGetComponent<Rigidbody2D>(out var component);
			if (component == null)
			{
				return result;
			}
			result = (component.sharedMaterial ? raycastHit2D.collider.GetComponent<Rigidbody2D>().sharedMaterial.friction : 1f);
		}
		return result;
	}

	private bool IsInSlope()
	{
		Vector2 vector = new Vector2(0f - (GetComponent<BoxCollider2D>().size.x / 2f - GetComponent<BoxCollider2D>().offset.x), 0f - (GetComponent<BoxCollider2D>().size.y / 2f - GetComponent<BoxCollider2D>().offset.y));
		Vector2 vector2 = new Vector2(0f + (GetComponent<BoxCollider2D>().size.x / 2f + GetComponent<BoxCollider2D>().offset.x), 0f - (GetComponent<BoxCollider2D>().size.y / 2f - GetComponent<BoxCollider2D>().offset.y));
		vector = base.transform.TransformPoint(vector);
		vector2 = base.transform.TransformPoint(vector2);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.down, 2f, groundLayer);
		float num = Vector2.Angle(Physics2D.Raycast(vector2, Vector2.down, 2f, groundLayer).normal, Vector2.down);
		if (Vector2.Angle(raycastHit2D.normal, Vector2.down) != 180f)
		{
			return num != 180f;
		}
		return false;
	}

	public void SetPlayerImmobile()
	{
		rb.velocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void SetPlayerMobile()
	{
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void SetAnimation(string name, float normalizedTime = 0f)
	{
		animator.Play(name, 0, normalizedTime);
		_skinAnimator.Play(name, 0, normalizedTime);
	}

	public void SetState(CharacterStates newState)
	{
		switch (newState)
		{
		case CharacterStates.GROUNDED:
			machineController.ChangeState(grounded, newState);
			break;
		case CharacterStates.JUMP:
			machineController.ChangeState(jump, newState);
			break;
		case CharacterStates.HANG:
			machineController.ChangeState(hang, newState);
			break;
		case CharacterStates.FALL:
			machineController.ChangeState(fall, newState);
			break;
		case CharacterStates.HARDFALL:
			machineController.ChangeState(hardFall, newState);
			break;
		case CharacterStates.WEIGHT:
			machineController.ChangeState(weight, newState);
			break;
		case CharacterStates.LAND:
			machineController.ChangeState(land, newState);
			break;
		case CharacterStates.GRAB:
			machineController.ChangeState(grab, newState);
			break;
		case CharacterStates.WALLFALL:
			machineController.ChangeState(wallFall, newState);
			break;
		}
	}

	public bool GetGrounded()
	{
		return isGrounded;
	}

	public float GetDistanceToCompanion()
	{
		return (base.transform.position - companion.transform.position).magnitude;
	}

	public bool GetCompanionGrounded()
	{
		return _companionStateController.GetGrounded();
	}

	public CharacterStates GetCurrentState()
	{
		return machineController.CurrentState._state;
	}

	public CharacterStates GetCompanionState()
	{
		return _companionStateController.GetCurrentState();
	}

	private void FixVelocity()
	{
		if (rb.velocity.x > maxSpeed)
		{
			rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
		}
		if (rb.velocity.x < 0f - maxSpeed)
		{
			rb.velocity = new Vector2(0f - maxSpeed, rb.velocity.y);
		}
	}

	public bool IsTouchingWall()
	{
		return Physics2D.OverlapCircle(grab.t_wallCheck_Front.position, grab.radiusCheck, grab.chainLayer);
	}

	public void GamepadRumble()
	{
	}

	public void CameraShake()
	{
	}

	private IEnumerator MovementAcceleration(float movement, float direction, float time)
	{
		time += Time.deltaTime;
		this.movement = Mathf.Lerp(movement, direction, time);
		yield return new WaitForEndOfFrame();
		if (time < 1f)
		{
			StartCoroutine(MovementAcceleration(movement, direction, time));
		}
	}

	private IEnumerator WhiteFlash()
	{
		GetComponent<AudioSource>().Play();
		spriteRenderer.material.SetFloat("Boolean_ab5becf3b9e94907bcf4a056c6d488d9", 1f);
		yield return new WaitForSeconds(0.1f);
		spriteRenderer.material.SetFloat("Boolean_ab5becf3b9e94907bcf4a056c6d488d9", 0f);
	}

	private IEnumerator PlayEmoteAnimation()
	{
		yield return new WaitForEndOfFrame();
		if (EmoteName == "Thumbs Up")
		{
			animator.SetInteger("IdleState", 1);
			_skinAnimator.SetInteger("IdleState", 1);
		}
		else if (EmoteName == "Clap")
		{
			animator.SetInteger("IdleState", 2);
			_skinAnimator.SetInteger("IdleState", 2);
		}
		else
		{
			emoteController.PlayeAnimation(EmoteName);
		}
	}

	private IEnumerator PullRope()
	{
		animator.SetInteger("IdleState", 3);
		_skinAnimator.SetInteger("IdleState", 3);
		SetPlayerImmobile();
		_isPullingRope = true;
		movement = 0f;
		while (true)
		{
			DistanceJoint2D component = GetComponent<DistanceJoint2D>();
			component.distance -= Time.deltaTime;
			rb.velocity = Vector2.zero;
			if ((double)component.distance <= 0.005)
			{
				break;
			}
			if (companion.transform.position.y - base.transform.position.y < 0.5f)
			{
				companion.transform.position += new Vector3(0f, 0.5f * Time.deltaTime, 0f);
			}
			yield return new WaitForEndOfFrame();
		}
		Vector3 position = companion.transform.position;
		position.y += 0.1f;
		companion.transform.position = position;
		yield return null;
	}

	private IEnumerator ReleaseRope()
	{
		animator.SetInteger("IdleState", 0);
		_isPullingRope = false;
		SetPlayerMobile();
		DistanceJoint2D component;
		while (true)
		{
			component = GetComponent<DistanceJoint2D>();
			component.distance += Time.deltaTime * 5f;
			if (component.distance >= 2.5f)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		component.distance = 2.5f;
		yield return null;
	}
}
