using Photon.Pun;
using UnityEngine;
using pworld.Scripts.PPhys;

public class ExplodedGoop : MonoBehaviour
{
	public float lifeTime = 5f;

	private PhotonView view_g;

	private float timeUntilRemove;

	public float slowAmount = 0.5f;

	private Collider col;

	private bool hasLeft;

	public SFX_Instance dissapear;

	public SFX_Instance enter;

	public SFX_Instance exit;

	private float t;

	private void Start()
	{
		view_g = GetComponent<PhotonView>();
		timeUntilRemove = lifeTime;
		if (PhotonGameLobbyHandler.IsSurface)
		{
			GetComponent<MeshRenderer>().material.SetFloat("_MinLight", 0.75f);
		}
	}

	private void Update()
	{
		t -= Time.deltaTime;
		if (t <= 0f)
		{
			t = 0f;
		}
		timeUntilRemove -= Time.deltaTime;
		if (!hasLeft && timeUntilRemove < 2f)
		{
			hasLeft = true;
			PPhysScaleLocal component = GetComponent<PPhysScaleLocal>();
			component.Target = Vector3.zero;
			component.Target = Vector3.zero;
		}
		if (view_g.IsMine && timeUntilRemove < 0f)
		{
			dissapear.Play(base.transform.position);
			PhotonNetwork.Destroy(base.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (!IsNotBlobbable(other))
		{
			other.attachedRigidbody.linearVelocity *= slowAmount;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!IsNotBlobbable(other))
		{
			if (t <= 0f)
			{
				enter.Play(base.transform.position);
			}
			t = 0.5f;
		}
	}

	private static bool IsNotBlobbable(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return true;
		}
		if (other.isTrigger)
		{
			return true;
		}
		return false;
	}

	private void OnTriggerExit(Collider other)
	{
		if (!IsNotBlobbable(other))
		{
			if (t <= 0f)
			{
				exit.Play(base.transform.position);
			}
			t = 0.5f;
		}
	}
}
