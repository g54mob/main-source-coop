using UnityEngine;

namespace Mirror.Examples.Hex2D
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(HexSpatialHash2DInterestManagement))]
	public class Hex2DNetworkManager : NetworkManager
	{
		[Header("Spawns")]
		public GameObject spawnPrefab;

		[Range(1f, 3000f)]
		[Tooltip("Number of prefabs to spawn in a flat 2D grid across the scene.")]
		public ushort spawnPrefabsCount = 1000;

		[Range(1f, 10f)]
		[Tooltip("Spacing between grid points in meters.")]
		public byte spawnPrefabSpacing = 3;

		[Header("Diagnostics")]
		[ReadOnly]
		[SerializeField]
		private HexSpatialHash2DInterestManagement hexSpatialHash2DInterestManagement;

		public new static Hex2DNetworkManager singleton => (Hex2DNetworkManager)NetworkManager.singleton;

		public override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				if (hexSpatialHash2DInterestManagement == null)
				{
					hexSpatialHash2DInterestManagement = GetComponent<HexSpatialHash2DInterestManagement>();
				}
			}
		}

		public override void OnStartClient()
		{
			NetworkClient.RegisterPrefab(spawnPrefab);
		}

		public override void OnStartServer()
		{
			Transform parent = new GameObject("Spawns").transform;
			int num = 0;
			int num2 = (int)Mathf.Sqrt((int)spawnPrefabsCount);
			float num3 = (float)((num2 - 1) * spawnPrefabSpacing) * 0.5f;
			float num4 = 0f - num3;
			float num5 = 0f - num3;
			for (int i = 0; i < num2; i++)
			{
				if (num >= spawnPrefabsCount)
				{
					break;
				}
				for (int j = 0; j < num2; j++)
				{
					if (num >= spawnPrefabsCount)
					{
						break;
					}
					Vector3 zero = Vector3.zero;
					if (hexSpatialHash2DInterestManagement.checkMethod == HexSpatialHash2DInterestManagement.CheckMethod.XZ_FOR_3D)
					{
						float x = num4 + (float)(i * spawnPrefabSpacing);
						float z = num5 + (float)(j * spawnPrefabSpacing);
						zero = new Vector3(x, 0.5f, z);
					}
					else
					{
						float x2 = num4 + (float)(i * spawnPrefabSpacing);
						float y = num5 + (float)(j * spawnPrefabSpacing);
						zero = new Vector3(x2, y, -0.5f);
					}
					NetworkServer.Spawn(Object.Instantiate(spawnPrefab, zero, Quaternion.identity, parent));
					num++;
				}
			}
		}
	}
}
