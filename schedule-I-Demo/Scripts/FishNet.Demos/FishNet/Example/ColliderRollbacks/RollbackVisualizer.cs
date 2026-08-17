using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.ColliderRollbacks
{
	public class RollbackVisualizer : NetworkBehaviour
	{
		[SerializeField]
		private GameObject _originalPrefab;

		[SerializeField]
		private GameObject _rollbackPrefab;

		[SerializeField]
		private TextCanvas _textCanvasPrefab;

		private List<float> _accuracyAverage = new List<float>();

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted;

		private void OnDisable()
		{
			_accuracyAverage.Clear();
		}

		[Server]
		public void ShowDifference(NetworkObject clientObject, Vector3 original, Vector3 rolledBack)
		{
			if (base.IsNetworked && !base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (!base.IsHost)
			{
				float difference = Vector3.Distance(original, rolledBack);
				PrintAverage(fromServer: false, difference, base.NetworkManager);
				TargetShowDifference(clientObject.Owner, original, rolledBack);
			}
		}

		[TargetRpc]
		private void TargetShowDifference(NetworkConnection conn, Vector3 original, Vector3 rollback)
		{
			RpcWriter___Target_TargetShowDifference_2390343144(conn, original, rollback);
		}

		private string PrintAverage(bool fromServer, float difference, NetworkManager nm)
		{
			if (nm.IsHost)
			{
				string text = "Accuracy will not show properly when as clientHost." + Environment.NewLine + "Use a separate client and server for testing.";
				Debug.Log(text);
				return text;
			}
			_accuracyAverage.Add(difference);
			if (_accuracyAverage.Count > 20)
			{
				_accuracyAverage.RemoveAt(0);
			}
			string text2 = "Accuracy is within " + difference.ToString("0.0000") + " units.";
			string text3 = string.Format("{0} hit average is {1}.", _accuracyAverage.Count, (_accuracyAverage.Sum() / (float)_accuracyAverage.Count).ToString("0.0000"));
			string text4 = text2 + " " + text3;
			Debug.Log(text4);
			return text4;
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted = true;
				RegisterTargetRpc(0u, RpcReader___Target_TargetShowDifference_2390343144);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002ERollbackVisualizerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void RpcWriter___Target_TargetShowDifference_2390343144(NetworkConnection conn, Vector3 original, Vector3 rollback)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				writer.WriteVector3(original);
				writer.WriteVector3(rollback);
				SendTargetRpc(0u, writer, channel, DataOrderType.Default, conn, excludeServer: false);
				writer.Store();
			}
		}

		private void RpcLogic___TargetShowDifference_2390343144(NetworkConnection conn, Vector3 original, Vector3 rollback)
		{
			UnityEngine.Object.Instantiate(_originalPrefab, original, base.transform.rotation);
			UnityEngine.Object.Instantiate(_rollbackPrefab, rollback, base.transform.rotation);
			float difference = Vector3.Distance(original, rollback);
			string text = PrintAverage(fromServer: true, difference, base.NetworkManager);
			UnityEngine.Object.Instantiate(_textCanvasPrefab).SetText(text);
		}

		private void RpcReader___Target_TargetShowDifference_2390343144(PooledReader PooledReader0, Channel channel)
		{
			Vector3 original = PooledReader0.ReadVector3();
			Vector3 rollback = PooledReader0.ReadVector3();
			if (base.IsClientInitialized)
			{
				RpcLogic___TargetShowDifference_2390343144(base.LocalConnection, original, rollback);
			}
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
