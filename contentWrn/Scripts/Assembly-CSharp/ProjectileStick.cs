using System;
using UnityEngine;

public class ProjectileStick : MonoBehaviour
{
	public SFX_Instance[] hitSFX;

	private Vector3 relativePos;

	private Vector3 relativeNormal;

	private Transform hitTransform;

	private void Awake()
	{
		Projectile component = GetComponent<Projectile>();
		component.postHitAction = (Action<RaycastHit>)Delegate.Combine(component.postHitAction, new Action<RaycastHit>(Stick));
	}

	private void Stick(RaycastHit hit)
	{
		if (!hitTransform)
		{
			for (int i = 0; i < hitSFX.Length; i++)
			{
				hitSFX[i].Play(base.transform.position);
			}
		}
		hitTransform = hit.transform;
		relativePos = hitTransform.InverseTransformPoint(hit.point);
		relativeNormal = hitTransform.InverseTransformDirection(-hit.normal);
	}

	private void LateUpdate()
	{
		if ((bool)hitTransform)
		{
			base.transform.position = hitTransform.TransformPoint(relativePos);
			base.transform.rotation = Quaternion.LookRotation(hitTransform.TransformDirection(relativeNormal));
		}
	}
}
