using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleAnimalBot : MonoBehaviour
{
	private enum State
	{
		Idle = 0,
		Walk = 1
	}

	[Header("Hareket")]
	public float walkSpeed = 2f;

	public float rotSpeed = 10f;

	[Header("Gezinme Alanı")]
	[Tooltip("Başlangıç noktasından sağa/sola max mesafe")]
	public float wanderRange = 8f;

	[Tooltip("Hedefe varınca durma süresi (min-max)")]
	public float minIdle = 1f;

	public float maxIdle = 3f;

	[Header("Animator")]
	public float animDampTime = 0.12f;

	[Tooltip("Yürürken Speed_f değeri")]
	public float walkAnimValue = 0.5f;

	private Animator _anim;

	private Vector3 _startPos;

	private Vector3 _target;

	private State _state;

	private float _timer;

	private static readonly int _hashSpeed = Animator.StringToHash("Speed_f");

	private float _currentSpeed;

	private void Awake()
	{
		_anim = GetComponent<Animator>();
		_startPos = base.transform.position;
	}

	private void Start()
	{
		EnterIdle();
	}

	private void Update()
	{
		switch (_state)
		{
		case State.Idle:
			_timer -= Time.deltaTime;
			_currentSpeed = 0f;
			if (_timer <= 0f)
			{
				PickNewTarget();
			}
			break;
		case State.Walk:
			MoveToTarget();
			break;
		}
		float value = Mathf.MoveTowards(_anim.GetFloat(_hashSpeed), _currentSpeed, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
		_anim.SetFloat(_hashSpeed, value);
	}

	private void EnterIdle()
	{
		_state = State.Idle;
		_timer = Random.Range(minIdle, maxIdle);
		_currentSpeed = 0f;
	}

	private void PickNewTarget()
	{
		float x = Random.Range(0f - wanderRange, wanderRange);
		float z = Random.Range((0f - wanderRange) * 0.4f, wanderRange * 0.4f);
		_target = _startPos + new Vector3(x, 0f, z);
		_state = State.Walk;
	}

	private void MoveToTarget()
	{
		Vector3 vector = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
		Vector3 vector2 = new Vector3(_target.x, 0f, _target.z) - vector;
		if (vector2.magnitude < 0.15f)
		{
			EnterIdle();
			return;
		}
		Vector3 normalized = vector2.normalized;
		Quaternion b = Quaternion.LookRotation(normalized);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * rotSpeed);
		Vector3 vector3 = normalized * walkSpeed * Time.deltaTime;
		base.transform.position += new Vector3(vector3.x, 0f, vector3.z);
		_currentSpeed = walkAnimValue;
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 center = (Application.isPlaying ? _startPos : base.transform.position);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(center, new Vector3(wanderRange * 2f, 0.2f, wanderRange * 0.8f));
	}
}
