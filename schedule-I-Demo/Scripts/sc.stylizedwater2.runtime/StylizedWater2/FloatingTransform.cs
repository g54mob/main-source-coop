using System.Collections.Generic;
using UnityEngine;

namespace StylizedWater2
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Water 2/Floating Transform")]
	public class FloatingTransform : MonoBehaviour
	{
		public enum WaterLevelSource
		{
			FixedValue = 0,
			WaterObject = 1
		}

		[Tooltip("This reference is required to grab the wave distance and height values")]
		public WaterObject waterObject;

		[Tooltip("Automatically find the Water Object below of above the Transform's position. This is slower than assigning a specific Water Object directly.")]
		public bool autoFind;

		[Tooltip("Only enable if the material's wave parameters are being changed in realtime, this has some performance overhead.\n\nIn edit-mode, the wave parameters are always fetched, so changes are directly visible")]
		public bool dynamicMaterial;

		[Tooltip("Configure what should be used to set the base water level. Relative wave height is added to this value")]
		public WaterLevelSource waterLevelSource = WaterLevelSource.WaterObject;

		public float waterLevel;

		[Tooltip("You can assign a child mesh object here. When assigned, the sample points will rotate/scale with the transform, instead of transform the component is attached to.")]
		public Transform childTransform;

		public float heightOffset;

		[Min(0f)]
		[Tooltip("Controls how strongly the transform should rotate to align with the wave curvature")]
		public float rollAmount = 0.1f;

		public List<Vector3> samples = new List<Vector3>();

		private Vector3 normal;

		private float height;

		private float m_waterLevel;

		public static bool Disable;

		private void OnValidate()
		{
		}

		public void FixedUpdate()
		{
			if (!this || !base.enabled || Disable)
			{
				return;
			}
			if (autoFind)
			{
				waterObject = WaterObject.Find(base.transform.position, rotationSupport: false);
			}
			if (!waterObject || !waterObject.material)
			{
				return;
			}
			m_waterLevel = (((bool)waterObject && waterLevelSource == WaterLevelSource.WaterObject) ? waterObject.transform.position.y : waterLevel);
			normal = Vector3.up;
			height = 0f;
			if (samples.Count == 0)
			{
				height = Buoyancy.SampleWaves(base.transform.position, waterObject.material, m_waterLevel, rollAmount, dynamicMaterial, out normal);
			}
			else
			{
				Vector3 zero = Vector3.zero;
				for (int i = 0; i < samples.Count; i++)
				{
					height += Buoyancy.SampleWaves(ConvertToWorldSpace(samples[i]), waterObject.material, m_waterLevel, rollAmount, dynamicMaterial, out normal);
					zero += normal;
				}
				height /= samples.Count;
				normal = (zero / samples.Count).normalized;
			}
			height += heightOffset;
			ApplyTransform();
		}

		private void ApplyTransform()
		{
			if (rollAmount > 0f)
			{
				base.transform.up = normal;
			}
			base.transform.position = new Vector3(base.transform.position.x, height, base.transform.position.z);
		}

		public Vector3 ConvertToWorldSpace(Vector3 position)
		{
			if ((bool)childTransform)
			{
				return childTransform.TransformPoint(position);
			}
			return base.transform.TransformPoint(position);
		}

		public Vector3 ConvertToLocalSpace(Vector3 position)
		{
			if ((bool)childTransform)
			{
				return childTransform.InverseTransformPoint(position);
			}
			return base.transform.InverseTransformPoint(position);
		}
	}
}
