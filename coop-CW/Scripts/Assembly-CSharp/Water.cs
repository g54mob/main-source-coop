using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	public ParticleSystem part;

	public SFX_PlayOneShot sfx;

	public float force;

	public float drag;

	public float playerForceM = 20f;

	public float maxDepth = 2f;

	private float waterLevel;

	public MeshRenderer rend;

	private List<Transform> ignoredRoots = new List<Transform>();

	private void Start()
	{
		waterLevel = GetComponent<Collider>().bounds.max.y;
	}

	private void Update()
	{
		for (int i = 0; i < PlayerHandler.instance.playersAlive.Count; i++)
		{
			Player player = PlayerHandler.instance.playersAlive[i];
			Vector3 vector = rend.material.GetVector("_Pos" + i);
			Vector3 vector2 = player.Center().XZ();
			float z = vector.z;
			if (player.data.inWaterAmount < 0.5f)
			{
				vector2 = vector;
			}
			float value = player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.linearVelocity.magnitude * player.data.inWaterAmount;
			vector2.z = Mathf.InverseLerp(1f, 0f, value);
			vector2.z *= 3f;
			z = ((!(z > vector2.z)) ? Mathf.MoveTowards(z, vector2.z, Time.deltaTime * 0.25f) : Mathf.MoveTowards(z, vector2.z, Time.deltaTime * 1f));
			vector2.z = z;
			rend.material.SetVector("_Pos" + i, vector2);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		Rigidbody attachedRigidbody = col.attachedRigidbody;
		if (!attachedRigidbody)
		{
			return;
		}
		Player componentInParent = col.GetComponentInParent<Player>();
		int num = 1;
		if (!componentInParent)
		{
			num = attachedRigidbody.GetComponentsInChildren<Collider>().Length;
		}
		float num2 = Mathf.Clamp01((waterLevel - col.transform.position.y) / maxDepth);
		num2 *= 1f / (float)num;
		col.attachedRigidbody.linearVelocity *= Mathf.Lerp(1f, drag, num2);
		col.attachedRigidbody.angularVelocity *= Mathf.Lerp(1f, drag, num2);
		float num3 = 1f;
		if ((bool)componentInParent)
		{
			num3 *= playerForceM;
		}
		col.attachedRigidbody.AddForceAtPosition(Vector3.up * 0.5f * num3 * force * num2, col.transform.position, ForceMode.Acceleration);
		if (!componentInParent)
		{
			if (!ignoredRoots.Contains(col.transform.root) && col.attachedRigidbody.linearVelocity.magnitude > 2f)
			{
				StartCoroutine(IgnoreFor(col.transform.root, 5f));
				part.transform.position = new Vector3(col.transform.position.x, waterLevel, col.transform.position.z);
				part.Play();
				sfx.Play();
			}
			return;
		}
		Bodypart component = attachedRigidbody.GetComponent<Bodypart>();
		if ((bool)component && component.bodypartType == BodypartType.Hip && !ignoredRoots.Contains(col.transform.root) && col.attachedRigidbody.linearVelocity.magnitude > 2f)
		{
			StartCoroutine(IgnoreFor(col.transform.root, 5f));
			part.transform.position = new Vector3(col.transform.position.x, waterLevel, col.transform.position.z);
			part.Play();
			sfx.Play();
		}
		componentInParent.data.inWaterAmount = Mathf.MoveTowards(componentInParent.data.inWaterAmount, 1f, num2 * 2f * Time.fixedDeltaTime);
		if (componentInParent.data.sinceGrounded > 0.25f)
		{
			componentInParent.data.sinceGrounded = Mathf.MoveTowards(componentInParent.data.sinceGrounded, 0.25f, num2 * 2f * Time.fixedDeltaTime);
		}
	}

	private IEnumerator IgnoreFor(Transform target, float seconds)
	{
		if ((bool)target)
		{
			ignoredRoots.Add(target);
		}
		yield return new WaitForSeconds(seconds);
		if ((bool)target && ignoredRoots.Contains(target))
		{
			ignoredRoots.Remove(target);
		}
	}
}
