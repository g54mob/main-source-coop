using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Cooking
{
	public class ElectricalCookingPotSlot : CookingPotSlot, ICookingPotSlotActions
	{
		[SerializeField]
		private MeshRenderer cookingPotSlotMeshRenderer;

		[SerializeField]
		private GameObject heatDistortionObject;

		private static readonly int EmissiveExposureWeight = Shader.PropertyToID("_EmissiveExposureWeight");

		protected override void Awake()
		{
			base.Awake();
			base.OnCookingPotSlotIgnited.AddListener(OnIgniteActions);
			base.OnCookingPotSlotExtinguished.AddListener(OnExtinguishActions);
		}

		private void OnDisable()
		{
			base.OnCookingPotSlotIgnited.RemoveListener(OnIgniteActions);
			base.OnCookingPotSlotExtinguished.RemoveListener(OnExtinguishActions);
		}

		public void OnIgniteActions()
		{
			Tween.Custom(this, cookingPotSlotMeshRenderer.material.GetFloat(EmissiveExposureWeight), 0f, 1f, delegate(ElectricalCookingPotSlot target, float val)
			{
				target.cookingPotSlotMeshRenderer.material.SetFloat(EmissiveExposureWeight, val);
			}, Ease.InCirc);
			heatDistortionObject.SetActive(value: true);
		}

		public void OnExtinguishActions()
		{
			Tween.Custom(this, cookingPotSlotMeshRenderer.material.GetFloat(EmissiveExposureWeight), 1f, 1f, delegate(ElectricalCookingPotSlot target, float val)
			{
				target.cookingPotSlotMeshRenderer.material.SetFloat(EmissiveExposureWeight, val);
			}, Ease.OutCirc);
			heatDistortionObject.SetActive(value: false);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
