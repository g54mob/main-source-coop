using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CW.Common
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[AddComponentMenu("CW/Common/CW Light Intensity")]
	public class CwLightIntensity : MonoBehaviour
	{
		[SerializeField]
		private float multiplier = 1f;

		[SerializeField]
		private float intensityInStandard = 1f;

		[SerializeField]
		private float intensityInURP = 1f;

		[SerializeField]
		private float intensityInHDRP = 120000f;

		[NonSerialized]
		private Light cachedLight;

		[NonSerialized]
		private bool cachedLightSet;

		[NonSerialized]
		private HDAdditionalLightData cachedLightData;

		public float Multiplier
		{
			get
			{
				return multiplier;
			}
			set
			{
				multiplier = value;
			}
		}

		public float IntensityInStandard
		{
			get
			{
				return intensityInStandard;
			}
			set
			{
				intensityInStandard = value;
			}
		}

		public float IntensityInURP
		{
			get
			{
				return intensityInURP;
			}
			set
			{
				intensityInURP = value;
			}
		}

		public float IntensityInHDRP
		{
			get
			{
				return intensityInHDRP;
			}
			set
			{
				intensityInHDRP = value;
			}
		}

		public Light CachedLight
		{
			get
			{
				if (!cachedLightSet)
				{
					cachedLight = GetComponent<Light>();
					cachedLightSet = true;
				}
				return cachedLight;
			}
		}

		protected virtual void Update()
		{
			if (CwHelper.IsBIRP)
			{
				ApplyIntensity(intensityInStandard);
			}
			else if (CwHelper.IsURP)
			{
				ApplyIntensity(intensityInURP);
			}
			else if (CwHelper.IsHDRP)
			{
				ApplyIntensity(intensityInHDRP);
			}
		}

		private void ApplyIntensity(float intensity)
		{
			if (intensity >= 0f)
			{
				if (!cachedLightSet)
				{
					cachedLight = GetComponent<Light>();
					cachedLightSet = true;
				}
				if (cachedLightData == null)
				{
					cachedLightData = GetComponent<HDAdditionalLightData>();
				}
				if (cachedLightData != null)
				{
					cachedLight.lightUnit = LightUnit.Lux;
					cachedLight.intensity = intensity * multiplier;
				}
			}
		}
	}
}
