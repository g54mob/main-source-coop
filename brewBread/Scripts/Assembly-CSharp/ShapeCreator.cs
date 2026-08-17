using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShapeCreator : MonoBehaviour
{
	[HideInInspector]
	public List<Vector2> points = new List<Vector2>();

	[HideInInspector]
	public List<Transform> childs;

	public MeshFilter mesh;

	public int lastPoints;

	public float shadowAngle = 60f;

	public float shadowDst = 2f;

	public void UpdatePoints()
	{
		childs.Clear();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			childs.Add(base.transform.GetChild(i));
		}
		points.Clear();
		for (int j = 0; j < childs.Count; j++)
		{
			points.Add(childs[j].position);
		}
	}

	public void GenerateShadows()
	{
	}
}
