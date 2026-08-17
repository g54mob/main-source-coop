using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class ExhaustFlash : Effect
	{
		public bool flash;

		public List<Light> flashLights = new List<Light>();

		public float flashChance = 0.2f;

		public bool flashOnRevLimiter = true;

		public bool flashOnShift = true;

		public float flashDuration = 0.05f;

		[Tooltip("Textures representing exhaust flash. If multiple are assigned a random texture will be chosen for each flash.")]
		public List<Texture2D> flashTextures = new List<Texture2D>();

		[Tooltip("    Mesh renderer(s) for the exhaust flash meshes. Materials used should have '_TintColor' property.")]
		public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

		public UnityEvent onFlash = new UnityEvent();

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.onRevLimiter.AddListener(FlashWithChance);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.onRevLimiter.RemoveListener(FlashWithChance);
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			flashLights = new List<Light>();
			flashTextures = new List<Texture2D>();
			meshRenderers = new List<MeshRenderer>();
		}

		public void Flash()
		{
			Flash(triggerEvent: true);
		}

		public void Flash(bool triggerEvent)
		{
			vehicleController.StartCoroutine(FlashCoroutine(triggerEvent));
		}

		public void FlashWithChance()
		{
			FlashWithChance(triggerEvent: true, flashChance);
		}

		public void FlashWithChance(bool triggerEvent, float chance)
		{
			if (UnityEngine.Random.Range(0f, 1f) < chance)
			{
				vehicleController.StartCoroutine(FlashCoroutine(triggerEvent));
			}
		}

		private IEnumerator FlashCoroutine(bool triggerEvent)
		{
			int count = flashTextures.Count;
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				meshRenderer.material.SetTexture("_MainTex", flashTextures[UnityEngine.Random.Range(0, count)]);
				float num = UnityEngine.Random.Range(0.2f, 0.6f);
				meshRenderer.transform.localScale = new Vector3(num, num, num);
				meshRenderer.enabled = true;
			}
			foreach (Light flashLight in flashLights)
			{
				flashLight.enabled = true;
			}
			if (triggerEvent)
			{
				onFlash.Invoke();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(flashDuration * 0.5f, flashDuration * 1.5f));
			foreach (MeshRenderer meshRenderer2 in meshRenderers)
			{
				meshRenderer2.enabled = false;
			}
			foreach (Light flashLight2 in flashLights)
			{
				flashLight2.enabled = false;
			}
			yield return null;
		}
	}
}
