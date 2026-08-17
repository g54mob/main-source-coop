using NomadDrive.Features.ObjectPlacement;
using UnityEngine;

namespace NomadDrive.Features.Attachables
{
	public interface IObjectSlot
	{
		ObjectSlotType SlotType { get; }

		bool IsOccupied { get; }

		void Detach();

		void Activate();

		void Deactivate();

		void SetHighlight(ObjectHighlightType type);

		void UnignoreHovering();

		void IgnoreHovering();

		void SetupHighlightModel(Transform sourceModel, Vector3 localPosition, Quaternion localRotation, Material overrideMaterial = null);

		void ClearHighlightModel();

		void ApplyAttachPreview(AttachableObject attachable);

		void ClearAttachPreview(AttachableObject attachable);

		void AttachFromPlacement(uint objectNetId);
	}
}
