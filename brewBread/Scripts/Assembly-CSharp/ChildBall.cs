using UnityEngine;

public class ChildBall : MonoBehaviour
{
	[SerializeField]
	private Transform followPoint;

	[SerializeField]
	private Transform basePoint;

	[SerializeField]
	private ChildBall brother;

	[HideInInspector]
	public bool follow;

	public bool returning;

	public bool arrived;

	public float speed;

	private Vector3 target;

	private void Start()
	{
		speed = Random.Range(speed - 1f, speed + 1f);
	}

	private void Update()
	{
		if (follow)
		{
			target = new Vector3(followPoint.position.x, base.transform.position.y);
			base.transform.position = Vector3.MoveTowards(base.transform.position, target, speed * Time.deltaTime);
			if ((followPoint.position - base.transform.position).magnitude < 1f)
			{
				followPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
				followPoint.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				brother.follow = false;
				follow = false;
				followPoint.parent = base.transform;
				followPoint.transform.localPosition = new Vector3(0.4f, 0.4f);
				target = new Vector3(brother.basePoint.position.x, base.transform.position.y);
				brother.target = target;
				brother.follow = false;
				returning = true;
				brother.returning = true;
				target = new Vector3(basePoint.position.x, base.transform.position.y);
			}
			if (target.x > base.transform.position.x)
			{
				GetComponent<SpriteRenderer>().flipX = false;
			}
			else
			{
				GetComponent<SpriteRenderer>().flipX = true;
			}
		}
		else
		{
			if (!returning)
			{
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, target, speed * Time.deltaTime);
			if (base.transform.position == target)
			{
				if (followPoint.parent == base.transform)
				{
					followPoint.parent = base.transform.parent;
				}
				arrived = true;
				returning = false;
				GetComponent<Animator>().SetTrigger("Arrived");
			}
			if (target.x > base.transform.position.x)
			{
				GetComponent<SpriteRenderer>().flipX = false;
			}
			else
			{
				GetComponent<SpriteRenderer>().flipX = true;
			}
		}
	}

	public void StartChasing()
	{
		follow = true;
	}
}
