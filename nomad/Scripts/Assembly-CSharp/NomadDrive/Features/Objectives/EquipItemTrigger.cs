using System;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class EquipItemTrigger : ObjectiveTrigger
	{
		[Tooltip("Equipped HeldItem must carry this Component type.")]
		public ComponentTypeReference TargetType = new ComponentTypeReference();

		protected override void OnActivate()
		{
			if (Context?.EquipmentManager != null)
			{
				Context.EquipmentManager.OnItemEquipped.AddListener(HandleEquipped);
			}
		}

		protected override void OnDeactivate()
		{
			if (Context?.EquipmentManager != null)
			{
				Context.EquipmentManager.OnItemEquipped.RemoveListener(HandleEquipped);
			}
		}

		private void HandleEquipped()
		{
			if (Context?.EquipmentManager != null && TargetType.IsAssigned)
			{
				HeldItem equippedEntity = Context.EquipmentManager.EquippedEntity;
				if (!(equippedEntity == null) && TargetType.IsMatch(equippedEntity.gameObject))
				{
					Fire?.Invoke();
				}
			}
		}
	}
}
