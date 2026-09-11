using System.Collections.Generic;
using UnityEngine;

public class MonsterScroller : MonoBehaviour
{
	private int currentModel;

	public List<Transform> collection;

	private void Start()
	{
		foreach (Transform item in base.transform)
		{
			collection.Add(item);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentModel++;
		}
		if (currentModel >= collection.Count)
		{
			currentModel = 0;
		}
		for (int i = 0; i < collection.Count; i++)
		{
			if (i != currentModel)
			{
				collection[i].gameObject.SetActive(value: false);
			}
			if (i == currentModel)
			{
				collection[i].gameObject.SetActive(value: true);
			}
		}
	}
}
