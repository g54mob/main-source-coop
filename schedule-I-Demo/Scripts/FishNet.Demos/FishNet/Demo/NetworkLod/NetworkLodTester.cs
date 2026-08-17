using System.Collections.Generic;
using FishNet.Managing.Observing;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.NetworkLod
{
	public class NetworkLodTester : NetworkBehaviour
	{
		[Header("General")]
		[SerializeField]
		private NetworkObject _prefab;

		[SerializeField]
		private ObserverManager _observerManager;

		[Range(1f, 8f)]
		[SerializeField]
		private byte _lodLevel = 8;

		[Header("Spawning")]
		[SerializeField]
		private int _count = 500;

		[SerializeField]
		private float _xyRange = 15f;

		[SerializeField]
		private float _zRange = 100f;

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted;

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EDemo_002ENetworkLod_002ENetworkLodTester_FishNet_002EDemos_002Edll();
			NetworkInitialize__Late();
		}

		public override void OnStartServer()
		{
			float num = _xyRange / (float)_count;
			float num2 = _zRange / (float)_count;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			for (int i = 0; i < _count; i++)
			{
				float z = num5;
				num3 += num;
				num4 += num;
				num5 += num2;
				float x = 0f;
				float y = 0f;
				NetworkObject nob = UnityEngine.Object.Instantiate(position: new Vector3(x, y, z), original: _prefab, rotation: Quaternion.identity);
				Spawn(nob);
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002ENetworkLodTesterFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void Awake_UserLogic_FishNet_002EDemo_002ENetworkLod_002ENetworkLodTester_FishNet_002EDemos_002Edll()
		{
			if (0 == 0)
			{
				Debug.LogError("Network Level of Detail demo requires Fish-Networking Pro to work.");
				UnityEngine.Object.DestroyImmediate(this);
				return;
			}
			List<float> levelOfDetailDistances = _observerManager.GetLevelOfDetailDistances();
			while (levelOfDetailDistances.Count > _lodLevel)
			{
				levelOfDetailDistances.RemoveAt(levelOfDetailDistances.Count - 1);
			}
		}
	}
}
