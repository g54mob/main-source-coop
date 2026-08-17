using System;
using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class VehiclePartFirstAttachTrigger : ObjectiveTrigger
	{
		[Tooltip("Component type that the attached part must carry (e.g. NomadDrive.Features.Driving.Tire). Re-attaching the same instance reuses its netId, so the source-id dedup keeps the counter stable.")]
		public ComponentTypeReference TargetPartType = new ComponentTypeReference();

		protected override void OnActivate()
		{
			AttachableObject.OnAnyAttachedGlobal += HandleAttached;
		}

		protected override void OnDeactivate()
		{
			AttachableObject.OnAnyAttachedGlobal -= HandleAttached;
		}

		private void HandleAttached(AttachableObject part)
		{
			if (!(part == null) && TargetPartType.IsAssigned && TargetPartType.IsMatch(part.gameObject))
			{
				FireWithSource(part.netId.ToString());
			}
		}
	}
}
