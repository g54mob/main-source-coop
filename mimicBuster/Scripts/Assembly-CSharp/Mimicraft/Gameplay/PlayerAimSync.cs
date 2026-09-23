using System;
using Mimicraft.Cameras;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerAimSync : NetworkBehaviour
	{
		private const float SendThresholdDegrees = 0.35f;

		[Tooltip("Source of the local pitch value. Auto-resolved from the FPS rig if left empty.")]
		[SerializeField]
		private FirstPersonLook firstPersonLook;

		private readonly NetworkVariable<float> networkPitch = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private Transform pitchPivot;

		private float lastSentPitch;

		private PlayerCameraRig cameraRig;

		private void Awake()
		{
			if (firstPersonLook == null)
			{
				firstPersonLook = GetComponentInChildren<FirstPersonLook>(includeInactive: true);
			}
			if (firstPersonLook != null)
			{
				pitchPivot = firstPersonLook.transform;
			}
			else
			{
				Debug.LogWarning("PlayerAimSync on '" + base.name + "' found no FirstPersonLook - other players will not see this one aim up or down.", this);
			}
		}

		private void Update()
		{
			if (pitchPivot == null)
			{
				return;
			}
			if (base.IsOwner)
			{
				if (cameraRig == null)
				{
					cameraRig = GetComponent<PlayerCameraRig>();
				}
				if (!(cameraRig == null) || !(firstPersonLook == null))
				{
					float num = ((cameraRig != null) ? cameraRig.AimPitch : firstPersonLook.Pitch);
					if (Mathf.Abs(num - lastSentPitch) >= 0.35f)
					{
						lastSentPitch = num;
						networkPitch.Value = num;
					}
					if (cameraRig != null && !cameraRig.IsFirstPerson)
					{
						pitchPivot.localRotation = Quaternion.Euler(num, 0f, 0f);
					}
				}
			}
			else
			{
				pitchPivot.localRotation = Quaternion.Euler(networkPitch.Value, 0f, 0f);
			}
		}

		protected override void __initializeVariables()
		{
			if (networkPitch == null)
			{
				throw new Exception("PlayerAimSync.networkPitch cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			networkPitch.Initialize(this);
			__nameNetworkVariable(networkPitch, "networkPitch");
			NetworkVariableFields.Add(networkPitch);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerAimSync";
		}
	}
}
