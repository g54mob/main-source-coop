using System;
using UnityEngine;

[Serializable]
public class SyncedAnimTransforms
{
	public Transform follower;

	public Transform target;

	public SyncedAnimTransforms(Transform transform, Transform setTarget)
	{
		follower = transform;
		target = setTarget;
	}
}
