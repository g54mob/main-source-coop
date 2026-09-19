using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderZone : MonoBehaviour
{
	[Header("4 Köşe (local, objeye göre)")]
	public Vector3 corner0 = new Vector3(-30f, 0f, -30f);

	public Vector3 corner1 = new Vector3(30f, 0f, -30f);

	public Vector3 corner2 = new Vector3(30f, 0f, 30f);

	public Vector3 corner3 = new Vector3(-30f, 0f, 30f);

	[Header("Gizmo")]
	public Color zoneColor = new Color(0.2f, 0.8f, 1f, 1f);

	public bool filled = true;

	public Vector3 WorldCorner(int i)
	{
		Vector3 position = i switch
		{
			0 => corner0, 
			1 => corner1, 
			2 => corner2, 
			_ => corner3, 
		};
		return base.transform.TransformPoint(position);
	}

	public Vector3 GetRandomPoint()
	{
		Vector3 a = WorldCorner(0);
		Vector3 b = WorldCorner(1);
		Vector3 vector = WorldCorner(2);
		Vector3 c = WorldCorner(3);
		Vector3 vector2 = ((!(UnityEngine.Random.value < 0.5f)) ? RandomInTriangle(a, vector, c) : RandomInTriangle(a, b, vector));
		if (TryGetTerrainHeight(vector2, out var groundY))
		{
			vector2.y = groundY;
		}
		return vector2;
	}

	public static bool TryGetTerrainHeight(Vector3 point, out float groundY)
	{
		RaycastHit[] array = Physics.RaycastAll(point + Vector3.up * 300f, Vector3.down, 1000f, -1, QueryTriggerInteraction.Ignore);
		Array.Sort(array, (RaycastHit h1, RaycastHit h2) => h1.distance.CompareTo(h2.distance));
		bool flag = false;
		bool flag2 = false;
		float num = 0f;
		RaycastHit[] array2 = array;
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			RaycastHit raycastHit = array2[num2];
			if (raycastHit.collider is TerrainCollider)
			{
				groundY = raycastHit.point.y;
				return true;
			}
			if (!flag)
			{
				flag = true;
				flag2 = NavMesh.SamplePosition(point, out var hit, 10f, -1);
				num = hit.position.y;
			}
			if (flag2 && Mathf.Abs(raycastHit.point.y - num) <= 0.5f)
			{
				groundY = raycastHit.point.y;
				return true;
			}
		}
		groundY = point.y;
		return false;
	}

	public static Vector3 SnapToTerrain(Vector3 point)
	{
		if (TryGetTerrainHeight(point, out var groundY))
		{
			point.y = groundY;
		}
		return point;
	}

	private static Vector3 RandomInTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		float num = Mathf.Sqrt(UnityEngine.Random.value);
		float value = UnityEngine.Random.value;
		return a * (1f - num) + b * (num * (1f - value)) + c * (num * value);
	}

	private void OnDrawGizmos()
	{
		Vector3 vector = WorldCorner(0);
		Vector3 vector2 = WorldCorner(1);
		Vector3 vector3 = WorldCorner(2);
		Vector3 vector4 = WorldCorner(3);
		Gizmos.color = zoneColor;
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
		Gizmos.color = new Color(zoneColor.r, zoneColor.g, zoneColor.b, 0.4f);
		Gizmos.DrawLine(vector, vector3);
		Gizmos.color = zoneColor;
		float radius = 0.6f;
		Gizmos.DrawSphere(vector, radius);
		Gizmos.DrawSphere(vector2, radius);
		Gizmos.DrawSphere(vector3, radius);
		Gizmos.DrawSphere(vector4, radius);
	}
}
