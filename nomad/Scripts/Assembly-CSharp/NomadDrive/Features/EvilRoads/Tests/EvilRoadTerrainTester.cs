using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Tests
{
	public class EvilRoadTerrainTester : MonoBehaviour
	{
		[SerializeField]
		private Terrain terrain;

		[SerializeField]
		private EvilRoadsManager divisionRoadsManager;

		[SerializeField]
		private float roadPointsDistance = 10f;

		[Header("Road Generation Settings")]
		[SerializeField]
		private float pointInterval = 1f;

		[SerializeField]
		private float curveAmplitude = 2f;

		[SerializeField]
		private float curveFrequency = 0.5f;

		[SerializeField]
		private bool generateOnStart = true;

		[Header("Elevation Settings")]
		[SerializeField]
		private bool enableElevationChanges = true;

		[SerializeField]
		private float elevationAmplitude = 3f;

		[SerializeField]
		private float elevationFrequency = 0.3f;

		[SerializeField]
		private float totalElevationChange = 2f;

		[SerializeField]
		private AnimationCurve elevationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[Header("Debug")]
		[SerializeField]
		private bool drawGizmos = true;

		[Header("Precision Settings")]
		[SerializeField]
		private bool showPrecisionInfo = true;

		[SerializeField]
		private Vector3[] generatedPoints;

		private void Start()
		{
			if (generateOnStart)
			{
				GenerateAndCreateRoad();
			}
		}

		public void GenerateAndCreateRoad()
		{
			generatedPoints = GenerateRoadPoints();
			if (divisionRoadsManager != null && terrain != null && generatedPoints.Length >= 2)
			{
				_ = generatedPoints[0];
				_ = ref generatedPoints[generatedPoints.Length - 1];
				_ = enableElevationChanges;
				divisionRoadsManager.CreateRoad(generatedPoints, terrain);
			}
		}

		private Vector3[] GenerateRoadPoints()
		{
			int num = Mathf.RoundToInt(roadPointsDistance / pointInterval) + 1;
			Vector3[] array = new Vector3[num];
			Vector3 position = base.transform.position;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / (float)(num - 1);
				float num3 = num2 * roadPointsDistance;
				float x = position.x;
				float z = position.z + num3;
				float num4 = Mathf.Sin(num3 * curveFrequency) * curveAmplitude;
				x += num4;
				float y = CalculateElevationAtDistance(position.y, num3, num2);
				array[i] = new Vector3(x, y, z);
			}
			return array;
		}

		private float CalculateElevationAtDistance(float baseY, float distance, float progress)
		{
			if (!enableElevationChanges)
			{
				return baseY;
			}
			float num = elevationCurve.Evaluate(progress) * totalElevationChange;
			float num2 = baseY + num;
			float num3 = Mathf.Sin(distance * elevationFrequency) * elevationAmplitude;
			float num4 = num2 + num3;
			float num5 = Mathf.Sin(distance * elevationFrequency * 2.3f) * (elevationAmplitude * 0.3f);
			float num6 = num4 + num5;
			float num7 = Mathf.Sin(distance * elevationFrequency * 4.7f) * (elevationAmplitude * 0.1f);
			return num6 + num7;
		}

		private float GetTerrainHeightAtPosition(Vector3 worldPos)
		{
			if (terrain == null)
			{
				return worldPos.y;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.transform.position;
			Vector3 vector = worldPos - position;
			if (vector.x < 0f || vector.x > terrainData.size.x || vector.z < 0f || vector.z > terrainData.size.z)
			{
				return worldPos.y;
			}
			float x = vector.x / terrainData.size.x;
			float y = vector.z / terrainData.size.z;
			float interpolatedHeight = terrainData.GetInterpolatedHeight(x, y);
			return position.y + interpolatedHeight;
		}

		private void OnDrawGizmos()
		{
			if (!drawGizmos)
			{
				return;
			}
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position, 0.5f);
			if (generatedPoints != null && generatedPoints.Length != 0)
			{
				Gizmos.color = Color.yellow;
				for (int i = 0; i < generatedPoints.Length; i++)
				{
					if (enableElevationChanges && generatedPoints.Length > 1)
					{
						float y = generatedPoints[0].y;
						float y2 = generatedPoints[0].y;
						Vector3[] array = generatedPoints;
						for (int j = 0; j < array.Length; j++)
						{
							Vector3 vector = array[j];
							if (vector.y < y)
							{
								y = vector.y;
							}
							if (vector.y > y2)
							{
								y2 = vector.y;
							}
						}
						float num = y2 - y;
						if (num > 0.1f)
						{
							float t = (generatedPoints[i].y - y) / num;
							Gizmos.color = Color.Lerp(Color.red, Color.green, t);
						}
						else
						{
							Gizmos.color = Color.yellow;
						}
					}
					else
					{
						Gizmos.color = Color.yellow;
					}
					Gizmos.DrawWireSphere(generatedPoints[i], 0.2f);
					if (i < generatedPoints.Length - 1)
					{
						Gizmos.color = Color.cyan;
						Gizmos.DrawLine(generatedPoints[i], generatedPoints[i + 1]);
					}
				}
			}
			else
			{
				Vector3[] array2 = GeneratePreviewPoints();
				Gizmos.color = Color.red;
				for (int k = 0; k < array2.Length; k++)
				{
					Gizmos.DrawWireSphere(array2[k], 0.15f);
					if (k < array2.Length - 1)
					{
						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(array2[k], array2[k + 1]);
						Gizmos.color = Color.red;
					}
				}
			}
			Gizmos.color = Color.white;
			Vector3 to = base.transform.position + new Vector3(0f, 0f, roadPointsDistance);
			Gizmos.DrawLine(base.transform.position, to);
		}

		private Vector3[] GeneratePreviewPoints()
		{
			int num = Mathf.RoundToInt(roadPointsDistance / pointInterval) + 1;
			Vector3[] array = new Vector3[num];
			Vector3 position = base.transform.position;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / (float)(num - 1);
				float num3 = num2 * roadPointsDistance;
				float x = position.x;
				float z = position.z + num3;
				float num4 = Mathf.Sin(num3 * curveFrequency) * curveAmplitude;
				x += num4;
				float y = CalculateElevationAtDistance(position.y, num3, num2);
				array[i] = new Vector3(x, y, z);
			}
			return array;
		}
	}
}
