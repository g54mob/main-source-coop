using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.HashGrid
{
	public class GridSpawner : NetworkBehaviour
	{
		[SerializeField]
		private NetworkObject _staticPrefab;

		[SerializeField]
		private NetworkObject _movingPrefab;

		[SerializeField]
		private int _movingCount = 100;

		[SerializeField]
		private byte _spacing = 2;

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted;

		private float _range => 25f;

		public override void OnStartServer()
		{
			for (int i = (int)(_range * -1f); (float)i < _range; i += _spacing)
			{
				for (int j = (int)(_range * -1f); (float)j < _range; j++)
				{
					NetworkObject nob = UnityEngine.Object.Instantiate(_staticPrefab, new Vector3(i, j, base.transform.position.z), Quaternion.identity);
					Spawn(nob);
				}
			}
			for (int k = 0; k < _movingCount; k++)
			{
				NetworkObject nob2 = UnityEngine.Object.Instantiate(_movingPrefab, base.transform.position, base.transform.rotation);
				Spawn(nob2);
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EGridSpawnerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
