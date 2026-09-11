using UnityEngine;

public class HasSpaceTest : MonoBehaviour
{
	public class Bot
	{
		private Transform tran;

		public Bot(Transform transform)
		{
			tran = transform;
		}

		public void Look(Vector3 dir, float f = 1f)
		{
			tran.forward = dir;
		}

		public Vector3 Center()
		{
			return tran.position;
		}
	}

	public void TurnToDirectionWithSpace()
	{
		Bot bot = new Bot(base.transform);
		float radius = 0.25f;
		Vector3 vector = bot.Center();
		float num = 5f;
		Vector3 vector2 = base.transform.forward;
		if (!HelperFunctions.SphereLineCheck(vector, vector + vector2 * num, HelperFunctions.LayerType.TerrainProp, radius).transform)
		{
			return;
		}
		int num2 = 20;
		int num3 = 360 / num2;
		Vector3 vector3 = Vector3.zero;
		float num4 = float.MaxValue;
		for (int i = 0; i < num2; i++)
		{
			if (i != 0)
			{
				vector2 = Quaternion.AngleAxis(num3 * i, Vector3.up) * vector2;
				RaycastHit raycastHit = HelperFunctions.SphereLineCheck(vector, vector + vector2 * num, HelperFunctions.LayerType.TerrainProp, radius);
				if (raycastHit.transform == null)
				{
					vector3 = vector2;
					break;
				}
				if (num4 < raycastHit.distance)
				{
					vector3 = vector2;
					num4 = raycastHit.distance;
					Debug.DrawLine(vector, vector + vector2 * num, Color.yellow, 2f);
				}
				else
				{
					Debug.DrawLine(vector, vector + vector2 * num, Color.red, 2f);
				}
			}
		}
		Debug.DrawLine(vector, vector + vector3 * num, Color.green, 2f);
		bot.Look(vector3, 20f);
	}

	private void Update()
	{
	}
}
