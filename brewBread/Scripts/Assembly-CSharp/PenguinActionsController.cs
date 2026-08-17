using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PenguinActionsController : MonoBehaviour
{
	[SerializeField]
	private PlayMakerFSM _playerMaker;

	[SerializeField]
	private PenguinAction[] _actions;

	[SerializeField]
	private PenguinInputController _penguinInput;

	[Space(10f)]
	[SerializeField]
	private Animator _emoteAnimator;

	private Animator _animator;

	[HideInInspector]
	public UnityEvent OnGrounded;

	[HideInInspector]
	public UnityEvent<PenguinActions, PenguinActionsController> OnStateChanged;

	[SerializeField]
	private float _groundDetectionRange;

	[SerializeField]
	private float _speed;

	private float _movement;

	private float _swingMovement;

	private BoxCollider2D _boxCollider;

	private float _maxPenguinDistance;

	private float _currentPenguinDistance;

	private bool _jumpEnabled;

	private bool _enableToJump;

	[field: SerializeField]
	public LayerMask GroundLayer { get; private set; }

	public float Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
		}
	}

	[field: SerializeField]
	public PenguinActionsController Companion { get; private set; }

	[field: SerializeField]
	public SpriteRenderer Sprite { get; private set; }

	public float Movement
	{
		get
		{
			return _movement;
		}
		set
		{
			SetMovement(value);
		}
	}

	public float SwingMovement
	{
		get
		{
			return _swingMovement;
		}
		set
		{
			_swingMovement = value;
		}
	}

	public bool IsGrounded { get; private set; }

	public Rigidbody2D Rb { get; private set; }

	public PenguinAction CurrentState { get; private set; }

	public Transform CompanionTransform { get; private set; }

	public bool JumpEnabled
	{
		get
		{
			return _jumpEnabled;
		}
		set
		{
			_jumpEnabled = value;
		}
	}

	public DistanceJoint2D DistanceJoint2D { get; private set; }

	public float CurrentPenguinDistance => _currentPenguinDistance;

	public float MaxPenguinDistance => _maxPenguinDistance;

	public PenguinFlags Flags { get; private set; }

	public bool EnableToJump
	{
		get
		{
			return _enableToJump;
		}
		set
		{
			_enableToJump = value;
		}
	}

	public BoxCollider2D BoxCollider2D => _boxCollider;

	private void Awake()
	{
		OnGrounded = new UnityEvent();
		Rb = GetComponent<Rigidbody2D>();
		_boxCollider = GetComponent<BoxCollider2D>();
		DistanceJoint2D = GetComponent<DistanceJoint2D>();
		_animator = GetComponent<Animator>();
		CompanionTransform = Companion.transform;
		OnStateChanged = new UnityEvent<PenguinActions, PenguinActionsController>();
		CurrentState = _actions[0];
	}

	private void Start()
	{
		PenguinAction[] actions = _actions;
		foreach (PenguinAction obj in actions)
		{
			obj.Controller = this;
			obj.Initialize();
		}
		CurrentState.Enter();
		_maxPenguinDistance = DistanceJoint2D.distance;
		WindController.OnPenguinEnter?.AddListener(EnteredWind);
		WindController.OnPenguinExit?.AddListener(ExitWind);
		EnableToJump = true;
	}

	public void Update()
	{
		IsGrounded = DetectGround();
		_currentPenguinDistance = Mathf.Abs((base.transform.position - Companion.transform.position).magnitude);
		if (Companion.SwingConditions() && _currentPenguinDistance >= _maxPenguinDistance - _maxPenguinDistance * 0.05f && base.transform.position.y < CompanionTransform.position.y)
		{
			SendPlayerMakerEvent("Swing");
		}
		CurrentState?.LogicUpdate();
		if (Sprite.material.GetFloat("_redValue") > 0f && CurrentState.Type != PenguinActions.Grab)
		{
			ColorCorrection();
		}
	}

	public void FixedUpdate()
	{
		CurrentState?.PhysicsUpdate();
	}

	public void OnDestroy()
	{
		PenguinAction[] actions = _actions;
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].Clean();
		}
		WindController.OnPenguinEnter?.RemoveListener(EnteredWind);
		WindController.OnPenguinExit?.RemoveListener(ExitWind);
	}

	public void SendPlayerMakerEvent(string eventName)
	{
		_playerMaker.SendEvent(eventName);
	}

	public void EnableInput(bool value)
	{
		_penguinInput.enabled = value;
		SendPlayerMakerEvent("Idle");
	}

	private void SetMovement(float value)
	{
		_movement = value;
	}

	public void SetPlayerMobility(bool value)
	{
		if (!value)
		{
			Rb.velocity = Vector2.zero;
			Rb.bodyType = RigidbodyType2D.Static;
			Flags |= PenguinFlags.Immobile;
		}
		else
		{
			Rb.bodyType = RigidbodyType2D.Dynamic;
			Flags &= ~PenguinFlags.Immobile;
		}
	}

	public void AddFlag(PenguinFlags newFlag)
	{
		Flags |= newFlag;
	}

	public void DeleteFlag(PenguinFlags newFlag)
	{
		Flags &= ~newFlag;
	}

	private void WindBlow(bool blowing)
	{
		SetAnimationInt("WalkState", blowing ? 2 : 0);
	}

	public bool SwingConditions()
	{
		if (!IsGrounded)
		{
			return CurrentState.Type == PenguinActions.Grab;
		}
		return true;
	}

	private bool DetectGround()
	{
		Vector2 vector = new Vector2(0f - (_boxCollider.size.x / 2f - _boxCollider.offset.x), 0f - (_boxCollider.size.y / 2f - _boxCollider.offset.y));
		Vector2 vector2 = new Vector2(_boxCollider.size.x / 2f + _boxCollider.offset.x, 0f - (_boxCollider.size.y / 2f - _boxCollider.offset.y));
		vector = base.transform.TransformPoint(vector);
		vector2 = base.transform.TransformPoint(vector2);
		if (base.name == "Fred")
		{
			Debug.Log(vector);
			Debug.Log(vector2);
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.down, _groundDetectionRange, GroundLayer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.down, _groundDetectionRange, GroundLayer);
		if (((bool)raycastHit2D || (bool)raycastHit2D2) && !IsInSlope(raycastHit2D, raycastHit2D2))
		{
			_jumpEnabled = true;
			OnGrounded?.Invoke();
			return true;
		}
		return false;
	}

	private bool IsInSlope(RaycastHit2D leftRay, RaycastHit2D rightRay)
	{
		float num = Vector2.Angle(rightRay.normal, Vector2.down);
		if (Vector2.Angle(leftRay.normal, Vector2.down) != 180f)
		{
			return num != 180f;
		}
		return false;
	}

	public bool CheckGrab()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.left, 0.8f, GroundLayer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(base.transform.position, Vector2.right, 0.8f, GroundLayer);
		bool flag = false;
		if (CurrentState.Type == PenguinActions.Swing)
		{
			flag = true;
		}
		else if (((bool)raycastHit2D && base.transform.localScale.x == -1f) || ((bool)raycastHit2D2 && base.transform.localScale.x == 1f))
		{
			flag = true;
		}
		if (!flag)
		{
			return false;
		}
		if ((bool)raycastHit2D && raycastHit2D.transform.tag != "Movable")
		{
			return raycastHit2D.transform.tag != "Ice";
		}
		if ((bool)raycastHit2D2 && raycastHit2D2.transform.tag != "Movable")
		{
			return raycastHit2D2.transform.tag != "Ice";
		}
		return false;
	}

	public void ChangeState(PenguinAction newState)
	{
		if (!(newState == CurrentState))
		{
			CurrentState?.Exit();
			CurrentState = newState;
			OnStateChanged?.Invoke(newState.Type, this);
			CurrentState?.Enter();
		}
	}

	public float GetFloat(string name, PenguinAction action)
	{
		if (name != null && name == "GrabTimer")
		{
			return ((GrabAction)action).GetPercentileAnimation();
		}
		return 0f;
	}

	private void EnteredWind(GameObject obj, WindController controller)
	{
		if (obj == base.gameObject)
		{
			Flags |= PenguinFlags.OnWind;
			controller.WindBlow.AddListener(WindBlow);
		}
	}

	private void ExitWind(GameObject obj, WindController controller)
	{
		if (obj == base.gameObject)
		{
			Flags &= ~PenguinFlags.OnWind;
			controller.WindBlow.RemoveListener(WindBlow);
			WindBlow(blowing: false);
		}
	}

	public void AnimatorEnabled(bool value)
	{
		_animator.enabled = value;
	}

	public void SetAnimation(string animationName)
	{
		_animator.Play(animationName);
	}

	public void SetAnimationBool(string boolName, bool value)
	{
		_animator.SetBool(boolName, value);
	}

	public void SetAnimationTrigger(string triggerName)
	{
		_animator.SetTrigger(triggerName);
	}

	public void SetAnimationInt(string intName, int value)
	{
		_animator.SetInteger(intName, value);
	}

	public void SetSpriteEnabled(bool value)
	{
		Sprite.enabled = value;
	}

	public void SetMaterialFloatProperty(string name, float value)
	{
		Sprite.material.SetFloat(name, value);
	}

	public void PlayEmote(string name)
	{
		_emoteAnimator.Play(name);
	}

	private void ColorCorrection()
	{
		float num = Sprite.material.GetFloat("_redValue");
		Sprite.material.SetFloat("_redValue", num -= Time.deltaTime / 15f);
		if (num <= 0f || IsGrounded)
		{
			Sprite.material.SetFloat("_redValue", 0f);
			StartCoroutine(WhiteFlash());
		}
	}

	private IEnumerator WhiteFlash()
	{
		GetComponent<AudioSource>().Play();
		Sprite.material.SetFloat("_whiteFlash", 1f);
		yield return new WaitForSeconds(0.1f);
		Sprite.material.SetFloat("_whiteFlash", 0f);
	}
}
