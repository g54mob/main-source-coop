using System;
using System.Collections.Generic;
using UnityEngine;

namespace StylizedWater2
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Water 2/Water Grid")]
	public class WaterGrid : MonoBehaviour
	{
		[Tooltip("Material used on the tile meshes")]
		public Material material;

		[Tooltip("When not in play-mode, the water will follow the scene-view camera position.")]
		public bool followSceneCamera;

		[Tooltip("If enabled, the object with the \"MainCamera\" tag will be assigned as the follow target when entering play mode")]
		public bool autoAssignCamera;

		[Tooltip("The grid will follow this Transform's position on the XZ axis. Ideally set to the camera's transform.")]
		public Transform followTarget;

		[Tooltip("Scale of the entire grid in the length and width")]
		public float scale = 500f;

		[Range(0.15f, 10f)]
		[Tooltip("Distance between vertices, rather higher than lower")]
		public float vertexDistance = 2f;

		[Min(1f)]
		public int rowsColumns = 4;

		[HideInInspector]
		public int m_rowsColumns = 4;

		[SerializeField]
		[HideInInspector]
		private Mesh mesh;

		[SerializeField]
		[HideInInspector]
		private List<WaterObject> objects = new List<WaterObject>();

		[NonSerialized]
		private float tileSize;

		[NonSerialized]
		private WaterObject m_waterObject;

		[NonSerialized]
		private Transform actualFollowTarget;

		[NonSerialized]
		private Vector3 targetPosition;

		private void Reset()
		{
			Recreate();
		}

		private void Start()
		{
			if (autoAssignCamera)
			{
				followTarget = (Camera.main ? Camera.main.transform : followTarget);
			}
		}

		private void OnEnable()
		{
			m_rowsColumns = rowsColumns;
			if (mesh == null)
			{
				RecreateMesh();
				ReassignMesh();
			}
		}

		private void Update()
		{
			if (Application.isPlaying)
			{
				actualFollowTarget = followTarget;
			}
			if ((bool)actualFollowTarget)
			{
				targetPosition = actualFollowTarget.transform.position;
				targetPosition = SnapToGrid(targetPosition, vertexDistance);
				targetPosition.y = base.transform.position.y;
				base.transform.position = targetPosition;
			}
		}

		public void Recreate()
		{
			RecreateMesh();
			bool flag = m_rowsColumns != rowsColumns || objects.Count < rowsColumns * rowsColumns;
			if (flag)
			{
				m_rowsColumns = rowsColumns;
			}
			if (flag && objects.Count > 0)
			{
				foreach (WaterObject @object in objects)
				{
					if ((bool)@object)
					{
						UnityEngine.Object.DestroyImmediate(@object.gameObject);
					}
				}
				objects.Clear();
			}
			int num = 0;
			for (int i = 0; i < rowsColumns; i++)
			{
				for (int j = 0; j < rowsColumns; j++)
				{
					if (flag)
					{
						m_waterObject = WaterObject.New(material, mesh);
						objects.Add(m_waterObject);
						m_waterObject.transform.parent = base.transform;
						m_waterObject.name = "WaterTile_x" + i + "z" + j;
					}
					else
					{
						m_waterObject = objects[num];
						m_waterObject.AssignMesh(mesh);
						m_waterObject.AssignMaterial(material);
					}
					m_waterObject.transform.localPosition = GridLocalCenterPosition(i, j);
					m_waterObject.transform.localScale = Vector3.one;
					num++;
				}
			}
		}

		private void RecreateMesh()
		{
			rowsColumns = Mathf.Max(rowsColumns, 1);
			tileSize = Mathf.Max(1f, scale / (float)rowsColumns);
			mesh = WaterMesh.Create(WaterMesh.Shape.Rectangle, tileSize, vertexDistance, tileSize);
		}

		private void ReassignMesh()
		{
			foreach (WaterObject @object in objects)
			{
				@object.AssignMesh(mesh);
			}
		}

		private Vector3 GridLocalCenterPosition(int x, int z)
		{
			return new Vector3((float)x * tileSize - tileSize * (float)rowsColumns * 0.5f + tileSize * 0.5f, 0f, (float)z * tileSize - tileSize * (float)rowsColumns * 0.5f + tileSize * 0.5f);
		}

		private static Vector3 SnapToGrid(Vector3 position, float cellSize)
		{
			return new Vector3(SnapToGrid(position.x, cellSize), SnapToGrid(position.y, cellSize), SnapToGrid(position.z, cellSize));
		}

		private static float SnapToGrid(float position, float cellSize)
		{
			return (float)Mathf.FloorToInt(position / cellSize) * cellSize + cellSize * 0.5f;
		}
	}
}
