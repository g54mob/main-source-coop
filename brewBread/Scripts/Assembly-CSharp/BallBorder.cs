using UnityEngine;

public class BallBorder : MonoBehaviour
{
	[SerializeField]
	private ChildBall child00;

	[SerializeField]
	private ChildBall child01;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 13)
		{
			if (child00.isActiveAndEnabled)
			{
				child00.enabled = false;
				child01.enabled = false;
				child00.GetComponent<Animator>().Play("IDLE");
				child01.GetComponent<Animator>().Play("IDLE");
			}
			else
			{
				child00.enabled = true;
				child01.enabled = true;
				child00.GetComponent<Animator>().Play("WALK");
				child01.GetComponent<Animator>().Play("WALK");
			}
		}
	}
}
