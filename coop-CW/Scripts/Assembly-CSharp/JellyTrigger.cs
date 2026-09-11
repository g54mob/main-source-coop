using Photon.Pun;
using UnityEngine;

public class JellyTrigger : MonoBehaviour
{
	private Bot_Jelly jelly;

	public float jellyForce;

	private void Start()
	{
		jelly = base.transform.root.GetComponentInChildren<Bot_Jelly>();
	}

	private void FixedUpdate()
	{
		if ((bool)jelly.jellyPlayer)
		{
			jelly.jellyPlayer.refs.ragdoll.AddForce((base.transform.position - jelly.jellyPlayer.Center()) * (Mathf.Clamp01(jelly.sinceCapture) * jellyForce), ForceMode.Acceleration);
			jelly.jellyPlayer.data.sinceGrounded = Mathf.Clamp(jelly.jellyPlayer.data.sinceGrounded, 0f, 1f);
			jelly.jellyPlayer.data.rotationOvveride = Quaternion.LookRotation(Vector3.down, jelly.bot.syncData.lookDireciton);
			jelly.jellyPlayer.data.rotationOvverideStr = 1f;
		}
	}

	private void Update()
	{
		if ((bool)jelly.jellyPlayer)
		{
			jelly.jellyPlayer.data.jelloTime = 0.5f;
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (!col.isTrigger && !jelly.fleeing && !jelly.jellyPlayer)
		{
			Player componentInParent = col.GetComponentInParent<Player>();
			if ((bool)componentInParent && !componentInParent.ai)
			{
				jelly.view.RPC("RPCA_SetJelloTarget", RpcTarget.All, componentInParent.refs.view.ViewID);
			}
		}
	}
}
