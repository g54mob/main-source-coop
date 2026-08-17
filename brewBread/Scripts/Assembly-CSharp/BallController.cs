using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
	private int _inContact;

	private bool _grabbed;

	private Rigidbody2D _rb;

	private readonly List<Transform> PENGUINS = new List<Transform>();

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private float _throwForce;

	private float _horizontalPosition;

	private CharacterStateController _penguinParent;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		CharacterStateController[] array = (CharacterStateController[])Object.FindObjectsOfType(typeof(CharacterStateController));
		foreach (CharacterStateController obj in array)
		{
			obj.GrabInputEnabled += GrabBall;
			obj.GrabInputDisabled += ThrowBall;
		}
	}

	private void OnDestroy()
	{
		CharacterStateController[] array = (CharacterStateController[])Object.FindObjectsOfType(typeof(CharacterStateController));
		foreach (CharacterStateController obj in array)
		{
			obj.GrabInputEnabled -= GrabBall;
			obj.GrabInputDisabled += ThrowBall;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			PENGUINS.Add(collision.transform);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			PENGUINS.Remove(collision.transform);
		}
	}

	private void Update()
	{
		if (_grabbed)
		{
			if (_horizontalPosition == 0f && _penguinParent.movement != 0f)
			{
				StopAllCoroutines();
				StartCoroutine(BallMoving());
			}
			if (_penguinParent.movement == 0f && _horizontalPosition != 0f)
			{
				StopAllCoroutines();
				StartCoroutine(BallStopping());
			}
			base.transform.localPosition = new Vector3(_horizontalPosition, 1f, 0f);
		}
	}

	private void GrabBall(Transform penguin)
	{
		if (PENGUINS.Contains(penguin))
		{
			_rb.bodyType = RigidbodyType2D.Static;
			_rb.velocity = Vector2.zero;
			base.transform.parent = penguin;
			_penguinParent = penguin.GetComponent<CharacterStateController>();
			_grabbed = true;
			base.transform.localPosition = new Vector3(0f, 1f, 0f);
			_animator.SetBool("StopPlay", value: true);
		}
	}

	private void ThrowBall(Transform penguin)
	{
		if (!(base.transform.parent != penguin))
		{
			_grabbed = false;
			penguin.TryGetComponent<CharacterStateController>(out var component);
			_rb.bodyType = RigidbodyType2D.Dynamic;
			float x = ((component.rb.velocity.normalized.x == 0f) ? component.movement : (component.rb.velocity.normalized.x * component.movement));
			_rb.AddForce(new Vector2(x, component.rb.velocity.normalized.y) * _throwForce);
			base.transform.parent = null;
		}
	}

	private IEnumerator BallMoving()
	{
		float lerpingTime = 0.2f;
		while (lerpingTime != 0f)
		{
			lerpingTime -= Time.deltaTime;
			_horizontalPosition = Mathf.Lerp(-0.4f, 0f, lerpingTime * 5f);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator BallStopping()
	{
		float lerpingTime = 0.1f;
		while (lerpingTime != 0f)
		{
			lerpingTime -= Time.deltaTime;
			_horizontalPosition = Mathf.Lerp(0f, _horizontalPosition, lerpingTime * 10f);
			yield return new WaitForEndOfFrame();
		}
	}
}
