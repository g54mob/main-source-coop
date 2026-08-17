using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.ColliderRollbacks
{
	public class PlayerMotor : NetworkBehaviour
	{
		[SerializeField]
		private float _moveRate = 3f;

		private CharacterController _characterController;

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted;

		public override void OnStartClient()
		{
			if (base.IsOwner)
			{
				_characterController = GetComponent<CharacterController>();
			}
		}

		private void Update()
		{
			if (base.IsOwner)
			{
				Move();
			}
		}

		private void Move()
		{
			if (!(_characterController == null))
			{
				Vector3 vector = new Vector3(0f, -10f, 0f);
				Vector3 vector2 = base.transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")));
				_characterController.Move((vector + vector2) * _moveRate * Time.deltaTime);
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EPlayerMotorFishNet_002EDemos_002Edll_Excuted = true;
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
