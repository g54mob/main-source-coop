using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Interaction
{
	public interface IEquippable
	{
		Vector3 PositionOnHand { get; }

		Vector3 RotationOnHand { get; }

		UnityEvent OnEquipped { get; set; }

		UnityEvent OnUnequipped { get; set; }

		UnityEvent OnDropped { get; set; }

		void Equip();

		void Unequip();

		void Drop();
	}
}
