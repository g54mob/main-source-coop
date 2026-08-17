using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.Prediction.Rigidbodies
{
	public class PredictedBullet : NetworkBehaviour
	{
		[HideInInspector]
		[SyncVar(OnChange = "_startingForce_OnChange")]
		public Vector3 _startingForce;

		private uint _stopTick;

		public SyncVar<Vector3> syncVar____startingForce;

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted;

		public Vector3 SyncAccessor__startingForce
		{
			get
			{
				return _startingForce;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					_startingForce = value;
				}
				if (Application.isPlaying)
				{
					syncVar____startingForce.SetValue(value, value);
				}
			}
		}

		public void SetStartingForce(Vector3 value)
		{
			this.sync___set_value__startingForce(value, true);
		}

		public override void OnStartServer()
		{
			StartCoroutine(__DelayDestroy(3f));
			SetVelocity(SyncAccessor__startingForce);
			Debug.Log("Setting new force.");
			this.sync___set_value__startingForce(Vector3.one, true);
		}

		public override void OnStartNetwork()
		{
			uint num = base.TimeManager.TimeToTicks(0.6499999761581421);
			if (base.IsServer || base.Owner.IsLocalClient)
			{
				_stopTick = base.TimeManager.LocalTick + num;
			}
			else
			{
				uint num2 = (uint)Mathf.Max(1f, base.TimeManager.Tick - base.TimeManager.LastPacketTick);
				long num3 = base.TimeManager.Tick + num - num2 - 1;
				if (num3 > 0)
				{
					_stopTick = (uint)num3;
				}
				else
				{
					_stopTick = 1u;
				}
			}
			base.TimeManager.OnTick += TimeManager_OnTick;
		}

		public override void OnStopNetwork()
		{
			base.TimeManager.OnTick -= TimeManager_OnTick;
		}

		private void TimeManager_OnTick()
		{
			if (_stopTick != 0 && base.TimeManager.LocalTick >= _stopTick)
			{
				GetComponent<Rigidbody>().isKinematic = true;
			}
		}

		private void _startingForce_OnChange(Vector3 prev, Vector3 next, bool asServer)
		{
			SetVelocity(next);
		}

		public void SetVelocity(Vector3 value)
		{
			Debug.Log($"Setting velocity on {base.gameObject.name} to {value}");
			GetComponent<Rigidbody>().velocity = value;
		}

		private IEnumerator __DelayDestroy(float time)
		{
			yield return new WaitForSeconds(time);
			Despawn();
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted = true;
				syncVar____startingForce = new SyncVar<Vector3>(this, 0u, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, _startingForce);
				syncVar____startingForce.OnChange += _startingForce_OnChange;
				RegisterSyncVarRead(ReadSyncVar___FishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBullet);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBulletFishNet_002EDemos_002Edll_Excuted = true;
				syncVar____startingForce.SetRegistered();
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		public virtual bool ReadSyncVar___FishNet_002EExample_002EPrediction_002ERigidbodies_002EPredictedBullet(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 0)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value__startingForce(syncVar____startingForce.GetValue(calledByUser: true), true);
					return true;
				}
				Vector3 value = PooledReader0.ReadVector3();
				this.sync___set_value__startingForce(value, Boolean2);
				return true;
			}
			return false;
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
