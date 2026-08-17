using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.Scened
{
	public class PlayerController : NetworkBehaviour
	{
		[SerializeField]
		private GameObject _camera;

		[SerializeField]
		private float _moveRate = 4f;

		[SerializeField]
		private bool _clientAuth = true;

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted;

		public override void OnStartClient()
		{
			if (base.IsOwner)
			{
				_camera.SetActive(value: true);
			}
		}

		private void Update()
		{
			if (base.IsOwner)
			{
				float axisRaw = Input.GetAxisRaw("Horizontal");
				float axisRaw2 = Input.GetAxisRaw("Vertical");
				if ((_clientAuth || (!_clientAuth && base.IsServer)) && !Physics.Linecast(base.transform.position + new Vector3(0f, 0.3f, 0f), base.transform.position - Vector3.one * 20f))
				{
					base.transform.position += new Vector3(0f, 3f, 0f);
				}
				if (_clientAuth)
				{
					Move(axisRaw, axisRaw2);
				}
				else
				{
					ServerMove(axisRaw, axisRaw2);
				}
			}
		}

		[ServerRpc]
		private void ServerMove(float hor, float ver)
		{
			RpcWriter___Server_ServerMove_1138564871(hor, ver);
		}

		private void Move(float hor, float ver)
		{
			float num = -10f * Time.deltaTime;
			if (Physics.Raycast(new Ray(base.transform.position + new Vector3(0f, 0.05f, 0f), -Vector3.up), 0.1f + (0f - num)))
			{
				num = 0f;
			}
			Vector3 direction = new Vector3(0f, num, ver * _moveRate * Time.deltaTime);
			base.transform.position += base.transform.TransformDirection(direction);
			base.transform.Rotate(new Vector3(0f, hor * 100f * Time.deltaTime, 0f));
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted = true;
				RegisterServerRpc(0u, RpcReader___Server_ServerMove_1138564871);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EScened_002EPlayerControllerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void RpcWriter___Server_ServerMove_1138564871(float hor, float ver)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if ((object)networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				writer.WriteSingle(hor);
				writer.WriteSingle(ver);
				SendServerRpc(0u, writer, channel, DataOrderType.Default);
				writer.Store();
			}
		}

		private void RpcLogic___ServerMove_1138564871(float hor, float ver)
		{
			Move(hor, ver);
		}

		private void RpcReader___Server_ServerMove_1138564871(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float hor = PooledReader0.ReadSingle();
			float ver = PooledReader0.ReadSingle();
			if (base.IsServerInitialized && OwnerMatches(conn))
			{
				RpcLogic___ServerMove_1138564871(hor, ver);
			}
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
