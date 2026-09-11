using UnityEngine;

public class WallWarpArm : MonoBehaviour
{
	public bool isLeftHand;

	public bool reachForPlayer;

	public bool closeHands;

	internal float reachAmount;

	internal float distanceFactor = 1f;

	public Rigidbody target;

	public Animator anim;

	private Quaternion startRot;

	private void Start()
	{
		startRot = base.transform.localRotation;
	}

	internal void InitTarget(Player player)
	{
		if ((bool)player)
		{
			target = player.refs.ragdoll.rigList[Random.Range(0, player.refs.ragdoll.rigList.Count)];
		}
		else
		{
			Debug.LogError("Wallo trying to grab null player. this is bad");
		}
		reachForPlayer = false;
	}

	internal void ClearTarget()
	{
		reachAmount = 0f;
		reachForPlayer = false;
		closeHands = false;
		target = null;
	}

	private void Update()
	{
		if ((bool)target)
		{
			anim.SetFloat("Distance", reachAmount * Vector3.Distance(base.transform.position, target.position) * distanceFactor);
			if (closeHands)
			{
				anim.SetBool("Close", value: true);
			}
			else
			{
				anim.SetBool("Close", value: false);
			}
			if (reachForPlayer)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(target.position - base.transform.position), 10f * Time.deltaTime);
				anim.SetBool("Reach", value: true);
			}
			else
			{
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, startRot, 10f * Time.deltaTime);
				anim.SetBool("Reach", value: false);
			}
		}
		else
		{
			anim.SetBool("Reach", value: false);
		}
		if ((bool)target && reachForPlayer)
		{
			reachAmount = Mathf.MoveTowards(reachAmount, 1f, Time.deltaTime * 1.5f);
		}
		else
		{
			reachAmount = Mathf.MoveTowards(reachAmount, 0f, Time.deltaTime);
		}
	}

	internal void Pull(float pullForce, Vector3 targetPos)
	{
		if ((bool)target)
		{
			target.linearVelocity *= 0.9f;
			target.angularVelocity *= 0.9f;
			target.AddForce(pullForce * (targetPos - target.position).normalized, ForceMode.Acceleration);
		}
	}
}
