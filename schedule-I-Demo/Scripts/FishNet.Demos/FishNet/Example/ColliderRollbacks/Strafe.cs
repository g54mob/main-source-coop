using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.ColliderRollbacks
{
	public class Strafe : NetworkBehaviour
	{
		public float MoveRate = 2f;

		public float MoveDistance = 3f;

		private bool _movingRight = true;

		private float _startX;

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted;

		public override void OnStartServer()
		{
			_startX = base.transform.position.x;
		}

		private void Update()
		{
			if (base.IsServer)
			{
				float x = (_movingRight ? (_startX + MoveDistance) : (_startX - MoveDistance));
				Vector3 vector = new Vector3(x, base.transform.position.y, base.transform.position.z);
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector, MoveRate * Time.deltaTime);
				if (base.transform.position == vector)
				{
					_movingRight = !_movingRight;
				}
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EStrafeFishNet_002EDemos_002Edll_Excuted = true;
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
