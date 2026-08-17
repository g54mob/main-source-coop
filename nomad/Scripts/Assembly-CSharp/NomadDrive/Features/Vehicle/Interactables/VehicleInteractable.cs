using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public abstract class VehicleInteractable : VehicleInteractableBase
	{
		[Header("Vehicle")]
		[SerializeField]
		private VehicleInteractableId _vehicleInteractableId;

		[Tooltip("Disambiguates several interactables that share the same VehicleInteractableId (e.g. multiple cabin lights). Keep 0 for a single instance; assign 0,1,2,... (max 7) when a vehicle has more than one of this control.")]
		[SerializeField]
		private byte _instanceIndex;

		[Header("Audio")]
		[Tooltip("One-shot played (networked) whenever any interaction on this control completes. Leave empty for silent controls.")]
		[SerializeField]
		private SoundID interactSound;

		private readonly HashSet<NomadDrive.Features.Interaction.Interaction> _soundWiredInteractions = new HashSet<NomadDrive.Features.Interaction.Interaction>();

		public new VehicleInteractableId InteractableId => _vehicleInteractableId;

		public byte NetworkKey => VehicleInteractableKey.Compose(_vehicleInteractableId, _instanceIndex);

		protected override void Start()
		{
			if (base.NetworkSync != null)
			{
				base.NetworkSync.RegisterInteractable(_vehicleInteractableId, _instanceIndex, this, HasNetworkState);
			}
			base.OnInteractionActivityPerformed.AddListener(WireInteractSounds);
			WireInteractSounds();
		}

		private void WireInteractSounds()
		{
			foreach (NomadDrive.Features.Interaction.Interaction activeInteraction in base.ActiveInteractions)
			{
				if (!(activeInteraction == null) && _soundWiredInteractions.Add(activeInteraction))
				{
					activeInteraction.OnInteractionProcessStateChanged += OnInteractionStateForSound;
				}
			}
		}

		private void OnInteractionStateForSound(InteractionState state)
		{
			if (state == InteractionState.Completed && interactSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(interactSound, base.transform.position);
			}
		}

		protected new void RequestStateChange(byte newState)
		{
			if (!(base.NetworkSync == null))
			{
				byte networkKey = NetworkKey;
				if (base.NetworkSync.isServer)
				{
					base.NetworkSync.ServerSetInteractableState(networkKey, newState);
				}
				else if (NetworkClient.active)
				{
					base.NetworkSync.CmdSetInteractableState(networkKey, newState);
				}
			}
		}

		public void PerformPrimaryInteractionFromInput()
		{
			GetBasicInteraction()?.CompleteInteraction();
		}

		protected void PlayInteractSound()
		{
			if (interactSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(interactSound, base.transform.position);
			}
		}

		protected bool RequireUsableBatteryOrWarn()
		{
			VehicleBatteryModule vehicleBatteryModule = ((base.VehicleManager != null) ? base.VehicleManager.GetModule<VehicleBatteryModule>() : null);
			if (vehicleBatteryModule == null || !vehicleBatteryModule.IsBatteryInstalled)
			{
				ShowBatteryWarning("@vehicle.battery_not_installed");
				return false;
			}
			if (vehicleBatteryModule.IsBatteryBroken)
			{
				ShowBatteryWarning("@vehicle.battery_depleted");
				return false;
			}
			return true;
		}

		private void ShowBatteryWarning(string localizationKey)
		{
			Object.FindFirstObjectByType<UIFeedbackManager>()?.CreateFloatingMessage(localizationKey, FeedbackType.Error);
		}
	}
}
