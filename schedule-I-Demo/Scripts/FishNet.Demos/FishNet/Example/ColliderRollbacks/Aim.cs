using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.ColliderRollbacks
{
	public class Aim : NetworkBehaviour
	{
		private readonly Vector3 _offset = new Vector3(0f, 1.65f, 0f);

		private bool NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted;

		public PlayerCamera PlayerCamera { get; private set; }

		public override void OnStartClient()
		{
			if (base.IsOwner)
			{
				PlayerCamera = Camera.main.transform.GetComponent<PlayerCamera>();
			}
		}

		private void Update()
		{
			if (base.IsOwner && !(PlayerCamera == null))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				MoveAim();
				MoveCamera();
			}
		}

		private void MoveAim()
		{
			float num = 2f;
			base.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X") * num, 0f));
			float num2 = PlayerCamera.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * num;
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			num2 = Mathf.Clamp(num2, -89f, 89f);
			PlayerCamera.transform.eulerAngles = new Vector3(num2, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}

		private void MoveCamera()
		{
			PlayerCamera.transform.position = base.transform.position + _offset;
			PlayerCamera.transform.rotation = Quaternion.Euler(PlayerCamera.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EExample_002EColliderRollbacks_002EAimFishNet_002EDemos_002Edll_Excuted = true;
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
