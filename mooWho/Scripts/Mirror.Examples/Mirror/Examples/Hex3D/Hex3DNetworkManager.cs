using UnityEngine;

namespace Mirror.Examples.Hex3D
{
	[AddComponentMenu("")]
	public class Hex3DNetworkManager : NetworkManager
	{
		[Header("Spawns")]
		public GameObject spawnPrefab;

		[Range(1f, 8000f)]
		public ushort spawnPrefabsCount = 1000;

		[Range(1f, 10f)]
		public byte spawnPrefabSpacing = 3;

		public new static Hex3DNetworkManager singleton => (Hex3DNetworkManager)NetworkManager.singleton;

		public override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				ushort num = (ushort)Mathf.Pow((int)spawnPrefabsCount, 1f / 3f);
				spawnPrefabsCount = (ushort)Mathf.Pow((int)num, 3f);
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
			int num2 = Mathf.RoundToInt(Mathf.Pow((int)spawnPrefabsCount, 1f / 3f));
			float num3 = (float)(-(num2 - 1) * spawnPrefabSpacing) * 0.5f;
			float num4 = (float)(-(num2 - 1) * spawnPrefabSpacing) * 0.5f;
			float num5 = (float)(-(num2 - 1) * spawnPrefabSpacing) * 0.5f;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						if (num < spawnPrefabsCount)
						{
							float x = num3 + (float)(i * spawnPrefabSpacing);
							float y = num4 + (float)(j * spawnPrefabSpacing);
							float z = num5 + (float)(k * spawnPrefabSpacing);
							NetworkServer.Spawn(Object.Instantiate(position: new Vector3(x, y, z), original: spawnPrefab, rotation: Quaternion.identity, parent: parent));
							num++;
						}
					}
				}
			}
		}
	}
}
