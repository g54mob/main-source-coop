using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
	public enum PatrolGroup
	{
		Dog = 0,
		Cat = 1,
		Bird = 2,
		Fish = 3,
		Bear = 4,
		Wolf = 5,
		Ant = 6
	}

	internal bool temporaryPoint;

	public bool alwaysDrawGizmos;

	private static readonly Vector3 up = new Vector3(0f, 2f, 0f);

	public PatrolGroup group;

	public float spawnWeight = 1f;

	public List<PatrolPoint> connectedPoints = new List<PatrolPoint>();

	private void Start()
	{
		if (!GetComponentInParent<Level>())
		{
			temporaryPoint = true;
			Level.currentLevel.AddPoint(this);
		}
	}

	private void OnDestroy()
	{
		if (temporaryPoint)
		{
			Level.currentLevel.RemovePoint(this);
		}
	}

	public void GetClosestPoints_All()
	{
		PatrolPoint[] componentsInChildren = base.transform.root.GetComponentsInChildren<PatrolPoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].GetClosestPoints();
		}
	}

	private void GetClosestPoints()
	{
		connectedPoints.Clear();
		float num = float.PositiveInfinity;
		PatrolPoint[] componentsInChildren = base.transform.root.GetComponentsInChildren<PatrolPoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] == this))
			{
				float num2 = Vector3.Distance(base.transform.position, componentsInChildren[i].transform.position);
				if (num2 < num && !HelperFunctions.LineCheck(base.transform.position + up, componentsInChildren[i].transform.position + up, HelperFunctions.LayerType.Terrain).transform)
				{
					num = num2;
				}
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!(componentsInChildren[j] == this) && !(Vector3.Distance(base.transform.position, componentsInChildren[j].transform.position) > num * 2f) && !HelperFunctions.LineCheck(base.transform.position + up, componentsInChildren[j].transform.position + up, HelperFunctions.LayerType.Terrain).transform)
			{
				if (!connectedPoints.Contains(componentsInChildren[j]))
				{
					connectedPoints.Add(componentsInChildren[j]);
				}
				if (!componentsInChildren[j].connectedPoints.Contains(this))
				{
					componentsInChildren[j].connectedPoints.Add(this);
				}
			}
		}
	}

	private Color GetColorByGroup(PatrolGroup group)
	{
		Color white = Color.white;
		return group switch
		{
			PatrolGroup.Dog => Color.red, 
			PatrolGroup.Cat => Color.yellow, 
			PatrolGroup.Bird => Color.cyan, 
			PatrolGroup.Fish => Color.blue, 
			PatrolGroup.Bear => new Color(0.64f, 0.3f, 0.24f), 
			PatrolGroup.Wolf => Color.gray, 
			PatrolGroup.Ant => Color.magenta, 
			_ => throw new ArgumentOutOfRangeException("group", group, null), 
		};
	}

	public void OnDrawGizmos()
	{
	}

	internal PatrolPoint GetNeighbor(List<PatrolGroup> groups, PatrolPoint currentPoint)
	{
		List<PatrolPoint> list = new List<PatrolPoint>();
		for (int i = 0; i < connectedPoints.Count; i++)
		{
			if (!(connectedPoints[i] == currentPoint) && groups.Contains(connectedPoints[i].group))
			{
				list.Add(connectedPoints[i]);
			}
		}
		if (connectedPoints.Count == 0)
		{
			return currentPoint;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	internal PatrolPoint GetNeighbor(List<PatrolGroup> groups, PatrolPoint currentPoint, Vector3 direction)
	{
		List<PatrolPoint> list = new List<PatrolPoint>();
		for (int i = 0; i < connectedPoints.Count; i++)
		{
			if (!(connectedPoints[i] == currentPoint) && groups.Contains(connectedPoints[i].group))
			{
				list.Add(connectedPoints[i]);
			}
		}
		if (connectedPoints.Count == 0)
		{
			return currentPoint;
		}
		float num = 360f;
		PatrolPoint result = null;
		for (int j = 0; j < list.Count; j++)
		{
			float num2 = Vector3.Angle(list[j].transform.position - base.transform.position, direction);
			if (num2 < num)
			{
				num = num2;
				result = list[j];
			}
		}
		return result;
	}
}
