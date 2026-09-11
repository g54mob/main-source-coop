using Photon.Pun;
using UnityEngine;

public class Web : MonoBehaviour
{
	private PhotonView view;

	private WebParent webParent;

	private LineRenderer line;

	private BoxCollider col;

	internal Rigidbody heldPart;

	private Player heldPlayer;

	internal Vector3 p1;

	internal Vector3 p2;

	internal Vector3 mid;

	private float stuckFor;

	public float force;

	public float drag;

	public float wholeBodyFactor = 0.1f;

	public float distanceFactor = 0.5f;

	private float sinceSpawn;

	public SFX_Instance stuck;

	public SFX_Instance unStuck;

	private Vector3 vel;

	private Vector3 currentPos;

	private float sinceLetGo;

	internal void StickToPlayer(Rigidbody playerPart, Player player)
	{
		heldPart = playerPart;
		heldPlayer = player;
		stuckFor = 0f;
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((bool)heldPart || sinceLetGo < 1f || !other.attachedRigidbody)
		{
			return;
		}
		Player componentInParent = other.GetComponentInParent<Player>();
		if ((bool)componentInParent && !componentInParent.ai)
		{
			Bodypart bodypartFromCollider = componentInParent.refs.ragdoll.GetBodypartFromCollider(other);
			if ((bool)bodypartFromCollider && bodypartFromCollider.bodypartType != BodypartType.Item)
			{
				stuck.Play(bodypartFromCollider.transform.position);
				webParent.RequestStick(componentInParent, this, componentInParent.refs.ragdoll.GetBodyPartID(bodypartFromCollider));
			}
		}
	}

	private void GetRefs()
	{
		col = GetComponentInChildren<BoxCollider>();
		view = GetComponentInParent<PhotonView>();
		line = GetComponentInChildren<LineRenderer>();
		webParent = GetComponentInParent<WebParent>();
	}

	public bool ConfigWeb()
	{
		GetRefs();
		RaycastHit raycastHit = HelperFunctions.LineCheck(base.transform.position, base.transform.position + base.transform.forward * 10f, HelperFunctions.LayerType.TerrainProp);
		RaycastHit raycastHit2 = HelperFunctions.LineCheck(base.transform.position, base.transform.position + base.transform.forward * -10f, HelperFunctions.LayerType.TerrainProp);
		if ((bool)raycastHit.transform && (bool)raycastHit2.transform)
		{
			p1 = raycastHit.point + base.transform.forward * 0.5f;
			p2 = raycastHit2.point - base.transform.forward * 0.5f;
			mid = Vector3.Lerp(p1, p2, 0.5f);
			line.SetPositions(new Vector3[3] { p1, mid, p2 });
			col.transform.position = mid;
			col.transform.rotation = Quaternion.LookRotation(p2 - p1);
			col.size = new Vector3(0.3f, 0.3f, Vector3.Distance(p1, p2));
			return true;
		}
		return false;
	}

	private void Update()
	{
		if ((bool)heldPart)
		{
			stuckFor += Time.deltaTime;
			currentPos = heldPart.transform.position;
			vel = heldPart.linearVelocity;
		}
		else
		{
			FRILerp.PositionSpring(ref currentPos, mid, 10f, 10f, ref vel);
		}
		line.SetPosition(1, currentPos);
		if (sinceSpawn < 2f)
		{
			sinceSpawn += Time.deltaTime;
			line.SetPosition(0, Vector3.Lerp(mid, p1, sinceSpawn * 3f));
			line.SetPosition(2, Vector3.Lerp(mid, p2, sinceSpawn * 3f));
		}
	}

	private void FixedUpdate()
	{
		sinceLetGo += Time.deltaTime;
		if ((bool)heldPart)
		{
			Vector3 a = mid - heldPart.position;
			a = Vector3.Lerp(a, a.normalized, 1f - distanceFactor);
			heldPlayer.refs.ragdoll.ExtraDrag(drag);
			float num = Mathf.Lerp(1f, 0.1f, stuckFor * 0.1f);
			heldPlayer.refs.ragdoll.AddForce(a * (force * wholeBodyFactor * num), ForceMode.Acceleration);
			heldPart.AddForce(a * (force * num), ForceMode.Force);
			if (view.IsMine && Vector3.SqrMagnitude(mid - heldPart.position) > 25f)
			{
				unStuck.Play(heldPart.transform.position);
				webParent.LetPlayerGo(this);
			}
		}
	}

	internal void LetGo()
	{
		sinceLetGo = 0f;
		heldPart = null;
	}

	internal bool TryInit()
	{
		GetRefs();
		int num = 6;
		for (int i = 0; i < num; i++)
		{
			base.transform.rotation = Random.rotation;
			if (ConfigWeb())
			{
				currentPos = mid;
				return true;
			}
		}
		base.gameObject.SetActive(value: false);
		return false;
	}

	internal void SetCustom(Vector3 vec1, Vector3 vec2)
	{
		GetRefs();
		p1 = vec1;
		p2 = vec2;
		mid = Vector3.Lerp(vec1, vec2, 0.5f);
		line.SetPositions(new Vector3[3] { p1, mid, p2 });
		col.size = new Vector3(0.3f, 0.3f, Vector3.Distance(p1, p2));
		currentPos = mid;
	}
}
