using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public enum PostHitBehaviour
	{
		Destroy = 0,
		Disable = 1,
		Pause = 2,
		None = 3
	}

	public HelperFunctions.LayerType layerType = HelperFunctions.LayerType.TerrainProp;

	public PostHitBehaviour postHitBehavior;

	public float velocity = 30f;

	public float upVelocity = 2f;

	public float gravity = 10f;

	private Vector3 vel;

	public float damage;

	public float force;

	public float fall;

	private bool paused;

	public Action<RaycastHit> hitAction;

	public Action<RaycastHit> postHitAction;

	private List<Transform> ignoredRoots = new List<Transform>();

	private void OnEnable()
	{
		vel = base.transform.forward * velocity + Vector3.up * upVelocity;
		ignoredRoots.Clear();
		paused = false;
	}

	private void Update()
	{
		if (!paused)
		{
			vel += Vector3.down * gravity * Time.deltaTime;
			Vector3 vector = base.transform.position + vel * Time.deltaTime;
			RaycastHit hit = HelperFunctions.LineCheck(base.transform.position, vector, layerType);
			if ((bool)hit.transform && !ignoredRoots.Contains(hit.transform.root) && !hit.collider.isTrigger)
			{
				Hit(hit);
				PostHit(hit);
			}
			else
			{
				base.transform.position = vector;
			}
		}
	}

	private void Hit(RaycastHit hit)
	{
		base.transform.position = hit.point;
		hitAction?.Invoke(hit);
		Player componentInParent = hit.transform.GetComponentInParent<Player>();
		if ((bool)componentInParent && componentInParent.IsLocal)
		{
			componentInParent.CallTakeDamageAndAddForceAndFallWithFallof(damage, base.transform.forward * force, fall, hit.point, 1f);
		}
	}

	private void PostHit(RaycastHit hit)
	{
		postHitAction?.Invoke(hit);
		if (postHitBehavior == PostHitBehaviour.Destroy)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (postHitBehavior == PostHitBehaviour.Disable)
		{
			base.enabled = false;
		}
		else if (postHitBehavior == PostHitBehaviour.Pause)
		{
			paused = true;
		}
	}

	internal void Ignore(Transform rootToIgnore, float seconds)
	{
		StartCoroutine(IIgnoreFor(rootToIgnore, seconds));
		IEnumerator IIgnoreFor(Transform item, float seconds2)
		{
			ignoredRoots.Add(item);
			yield return new WaitForSeconds(seconds2);
			if (ignoredRoots.Contains(item))
			{
				ignoredRoots.Remove(item);
			}
		}
	}
}
