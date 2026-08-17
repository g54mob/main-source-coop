using System.Collections;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Time a player can be in top of the block before start falling, in Seconds")]
	private float holdTime;

	[SerializeField]
	[Tooltip("Time before the block start recovering, in Seconds")]
	private float recoverTime;

	[SerializeField]
	private LayerMask _playerLayer;

	private bool falling;

	private Animator animator;

	private Vector2 _colliderCornerA;

	private Vector2 _colliderCornerB;

	private void Start()
	{
		animator = GetComponent<Animator>();
		BoxCollider2D component = GetComponent<BoxCollider2D>();
		_colliderCornerA = component.bounds.max;
		_colliderCornerB = component.bounds.min;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9 && !falling)
		{
			animator.SetTrigger("Pressed");
			StartCoroutine(FallBlock());
			falling = true;
		}
	}

	private IEnumerator FallBlock()
	{
		float t = 0f;
		while (true)
		{
			t += Time.deltaTime;
			if (t >= holdTime)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		animator.SetBool("Fall", value: true);
	}

	public void RebornBlock()
	{
		StartCoroutine(RecoverBlock());
	}

	private IEnumerator RecoverBlock()
	{
		float t = 0f;
		while (!(t >= recoverTime) || (bool)Physics2D.OverlapArea(_colliderCornerA, _colliderCornerB, _playerLayer))
		{
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		animator.SetBool("Fall", value: false);
		falling = false;
	}
}
