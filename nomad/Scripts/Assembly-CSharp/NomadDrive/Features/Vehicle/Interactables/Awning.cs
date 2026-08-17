using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class Awning : VehicleInteractable
	{
		[SerializeField]
		private Transform handle;

		[SerializeField]
		private SkinnedMeshRenderer awningMeshRenderer;

		protected override bool UseStateMachine => false;

		public void Open()
		{
			Tween.LocalRotation(handle, Quaternion.Euler(0f, 0f, 1080f), 3f);
			Tween.Custom(awningMeshRenderer, awningMeshRenderer.GetBlendShapeWeight(0), 100f, 3f, delegate(SkinnedMeshRenderer target, float val)
			{
				target.SetBlendShapeWeight(0, val);
			});
		}

		public void Close()
		{
		}
	}
}
