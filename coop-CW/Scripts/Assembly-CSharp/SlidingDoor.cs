using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
	private static readonly int Open = Animator.StringToHash("Open");

	public Transform leftDoor;

	public Transform rightDoor;

	public Transform leftEdge;

	public Transform rightEdge;

	private BoxCollider trigger;

	private bool isOpen;

	public float sinceTryClose;

	private PhotonView view;

	private Animator anim;

	public bool alwaysOpen;

	private void Start()
	{
		if (!alwaysOpen && GameAPI.GetSeededPositionProbability(base.transform.position, 0.25))
		{
			base.transform.Find("Door").gameObject.SetActive(value: false);
			base.transform.Find("Wall").gameObject.SetActive(value: true);
			GetComponent<Collider>().enabled = false;
			base.enabled = false;
		}
		else
		{
			Object.Destroy(base.transform.Find("Wall").gameObject);
			anim = GetComponentInChildren<Animator>();
			trigger = GetComponent<BoxCollider>();
			view = GetComponent<PhotonView>();
			StartCoroutine(SwitchColliderLayers());
		}
		IEnumerator SwitchColliderLayers()
		{
			for (int i = 0; i < 5; i++)
			{
				yield return null;
			}
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!componentsInChildren[j].isTrigger)
				{
					componentsInChildren[j].gameObject.layer = 10;
				}
			}
		}
	}

	public void ConfigEdges()
	{
		Vector3 vector = base.transform.TransformPoint(Vector3.forward + Vector3.up);
		RaycastHit raycastHit = HelperFunctions.LineCheck(vector, vector + base.transform.right * 10f, HelperFunctions.LayerType.Terrain);
		RaycastHit raycastHit2 = HelperFunctions.LineCheck(vector, vector + base.transform.right * -10f, HelperFunctions.LayerType.Terrain);
		float num = raycastHit.distance - raycastHit2.distance;
		base.transform.position += base.transform.right * num * 0.5f;
		float num2 = (raycastHit.distance + raycastHit2.distance) * 0.5f;
		rightEdge.transform.localPosition = new Vector3(num2, 0f, 0f);
		leftEdge.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
	}

	private void Update()
	{
		sinceTryClose += Time.deltaTime;
		if (isOpen && sinceTryClose > 0.5f)
		{
			TryClose();
			sinceTryClose = 0f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && view.IsMine && (bool)other.GetComponentInParent<Player>() && !isOpen)
		{
			view.RPC("RPCA_Open", RpcTarget.All);
		}
	}

	private void TryClose()
	{
		Collider[] array = Physics.OverlapBox(base.transform.TransformPoint(trigger.center), trigger.size * 0.5f, base.transform.rotation);
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i].GetComponentInParent<Player>())
			{
				return;
			}
		}
		view.RPC("RPCA_Close", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_Open()
	{
		isOpen = true;
		anim.SetBool(Open, isOpen);
	}

	[PunRPC]
	private void RPCA_Close()
	{
		isOpen = false;
		anim.SetBool(Open, isOpen);
	}
}
