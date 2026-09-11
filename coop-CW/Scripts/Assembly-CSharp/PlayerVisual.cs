using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	public TransformFollowerPair[] followerConfig;

	public void SetTargets()
	{
		ConfigTargets();
		Transform root = base.transform.root.Find("RigCreator");
		for (int i = 0; i < followerConfig.Length; i++)
		{
			followerConfig[i].main = HelperFunctions.FindChildRecursive(followerConfig[i].targetType.ToString(), base.transform);
			followerConfig[i].target = HelperFunctions.FindChildRecursive(followerConfig[i].targetType.ToString(), root);
		}
	}

	private void ConfigTargets()
	{
		RigCreator componentInChildren = base.transform.root.GetComponentInChildren<RigCreator>();
		List<TransformFollowerPair> list = new List<TransformFollowerPair>();
		for (int i = 0; i < componentInChildren.bodyparts.Count; i++)
		{
			TransformFollowerPair transformFollowerPair = new TransformFollowerPair();
			transformFollowerPair.targetType = componentInChildren.bodyparts[i].partType;
			list.Add(transformFollowerPair);
		}
		followerConfig = list.ToArray();
	}

	private void Awake()
	{
		Renderer[] componentsInChildren = followerConfig[0].target.parent.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		for (int j = 0; j < followerConfig.Length; j++)
		{
			followerConfig[j].main.transform.SetParent(followerConfig[j].target);
		}
	}

	public void ToggleRenderers()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = !componentsInChildren[i].enabled;
		}
	}

	private void Start()
	{
	}
}
