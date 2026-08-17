using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
	private bool ballHit;

	[SerializeField]
	private Animator child_00;

	[SerializeField]
	private Animator child_01;

	[SerializeField]
	private float force;

	private bool arrived;

	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (child_00.gameObject.GetComponent<ChildBall>().arrived && child_01.gameObject.GetComponent<ChildBall>().arrived && !arrived)
		{
			arrived = true;
			base.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
			ballHit = false;
			float magnitude = (child_00.transform.position - base.transform.position).magnitude;
			float magnitude2 = (child_01.transform.position - base.transform.position).magnitude;
			if (magnitude < magnitude2)
			{
				child_00.Play("PLAY");
				child_01.Play("PLAY");
				base.transform.parent.gameObject.GetComponent<Animator>().Play("KidsWithBall", 0, 0f);
			}
			else
			{
				child_00.Play("PLAY", 0, 0.5f);
				child_01.Play("PLAY", 0, 0.5f);
				base.transform.parent.gameObject.GetComponent<Animator>().Play("KidsWithBall", 0, 0.5f);
			}
			child_00.GetComponent<SpriteRenderer>().flipX = false;
			child_01.GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			if (!ballHit)
			{
				ballHit = true;
				arrived = false;
				base.transform.parent.gameObject.GetComponent<Animator>().enabled = false;
				rb.bodyType = RigidbodyType2D.Dynamic;
				child_00.SetTrigger("Surprised");
				child_01.SetTrigger("Surprised");
				child_00.gameObject.GetComponent<ChildBall>().arrived = false;
				child_01.gameObject.GetComponent<ChildBall>().arrived = false;
			}
			Vector2 vector = base.transform.position - collision.transform.position;
			vector.y += 5f;
			rb.AddForce(vector.normalized * force);
			rb.AddTorque(5f * vector.normalized.x);
		}
	}
}
