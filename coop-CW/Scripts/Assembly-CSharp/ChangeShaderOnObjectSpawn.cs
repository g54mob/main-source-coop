using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderOnObjectSpawn : MonoBehaviour
{
	public GameObject[] onStart;

	public List<Shader> fromList = new List<Shader>();

	public Shader from;

	public Shader to;

	private void Start()
	{
		GameAPI instance = GameAPI.instance;
		instance.objectSpawnedAction = (Action<GameObject>)Delegate.Combine(instance.objectSpawnedAction, new Action<GameObject>(ObjectSpawned));
		GameObject[] array = onStart;
		foreach (GameObject spawned in array)
		{
			ObjectSpawned(spawned);
		}
	}

	private void ObjectSpawned(GameObject spawned)
	{
		Renderer[] componentsInChildren = spawned.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material[] materials = componentsInChildren[i].materials;
			for (int j = 0; j < materials.Length; j++)
			{
				if (materials[j].shader == from || fromList.Contains(materials[j].shader))
				{
					materials[j].shader = to;
				}
			}
			componentsInChildren[i].materials = materials;
		}
	}
}
