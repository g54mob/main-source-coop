using System;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class LightSource
	{
		public enum LightType
		{
			Light = 0,
			Mesh = 1
		}

		[ColorUsage(true, true)]
		[Tooltip("    Color of the emitted light.")]
		public Color emissionColor;

		[Tooltip("Light (point/spot/directional/etc.) representing the vehicle light. Will only be used if light type is set to\r\nLight.")]
		public Light light;

		[Tooltip("Mesh renderer using standard shader. Emission on the material will be turned on or off depending on light state.")]
		public MeshRenderer meshRenderer;

		[Tooltip("    If your mesh has more than one material set this number to the index of required material.")]
		public int rendererMaterialIndex;

		[Tooltip("    Type of the light.")]
		public LightType type;

		public UnityEvent onLightTurnedOn = new UnityEvent();

		public UnityEvent onLightTurnedOff = new UnityEvent();

		public bool IsOn { get; private set; }

		public virtual void TurnOff()
		{
			if (!IsOn)
			{
				return;
			}
			onLightTurnedOff.Invoke();
			if (type == LightType.Light && light != null)
			{
				light.enabled = false;
			}
			else if (Application.isPlaying)
			{
				if (meshRenderer == null || meshRenderer.material == null)
				{
					return;
				}
				Material obj = meshRenderer.materials[rendererMaterialIndex];
				obj.SetFloat("_UseEmissiveIntensity", 0f);
				obj.SetColor("_EmissiveColor", emissionColor * 0f);
				meshRenderer.materials[rendererMaterialIndex].DisableKeyword("_EMISSION");
			}
			IsOn = false;
		}

		public virtual void TurnOn()
		{
			if (IsOn)
			{
				return;
			}
			onLightTurnedOn.Invoke();
			if (type == LightType.Light && light != null)
			{
				light.enabled = true;
			}
			else if (Application.isPlaying)
			{
				if (meshRenderer == null || meshRenderer.material == null)
				{
					return;
				}
				Material obj = meshRenderer.materials[rendererMaterialIndex];
				float num = 100000f;
				obj.SetFloat("_UseEmissiveIntensity", 1f);
				obj.SetColor("_EmissiveColor", emissionColor * num);
			}
			IsOn = true;
		}
	}
}
