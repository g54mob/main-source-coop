using EvilCore.Networking.Parenting;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using UnityEngine.Events;

namespace NomadDrive.Features.Player
{
	public interface IEquipmentManager
	{
		float ItemEquippingSmoothSpeed { get; set; }

		NetworkedTransform EquipmentNetworkedTransform { get; set; }

		HeldItem EquippedEntity { get; set; }

		UnityEvent OnItemEquipped { get; set; }

		UnityEvent OnItemUnequipped { get; set; }

		UnityEvent OnItemDropped { get; set; }

		bool IsItemEquipped { get; }

		UnityEvent OnGripPauseRequested { get; set; }

		UnityEvent OnGripResumeRequested { get; set; }

		void Equip(HeldItem heldItem);

		void Unequip(HeldItem heldItem = null, bool keepSlotDetection = false);

		void Drop(HeldItem heldItem = null);

		void Consume();

		bool IsEquippedObjectAttachable();

		bool TryGetEquippedILiquidContainer(out ILiquidContainer container);

		void DisableObjectSlotDetectionMode();

		bool DropInstant(HeldItem heldItem = null);
	}
}
