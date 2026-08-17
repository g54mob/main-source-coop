using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace StylizedWater2
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Water 2/Water Object")]
	[DisallowMultipleComponent]
	public class WaterObject : MonoBehaviour
	{
		public static readonly List<WaterObject> Instances = new List<WaterObject>();

		public Material material;

		public MeshFilter meshFilter;

		public MeshRenderer meshRenderer;

		private MaterialPropertyBlock _props;

		public MaterialPropertyBlock props
		{
			get
			{
				if (_props == null)
				{
					CreatePropertyBlock(meshRenderer);
				}
				return _props;
			}
			private set
			{
				_props = value;
			}
		}

		private void CreatePropertyBlock(Renderer sourceRenderer)
		{
			_props = new MaterialPropertyBlock();
			sourceRenderer.GetPropertyBlock(_props);
		}

		private void Reset()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			CreatePropertyBlock(meshRenderer);
			meshFilter = GetComponent<MeshFilter>();
		}

		private void OnEnable()
		{
			Instances.Add(this);
		}

		private void OnDisable()
		{
			Instances.Remove(this);
		}

		private void OnValidate()
		{
			if (!meshRenderer)
			{
				meshRenderer = GetComponent<MeshRenderer>();
			}
			if (!meshFilter)
			{
				meshFilter = GetComponent<MeshFilter>();
			}
			FetchWaterMaterial();
		}

		public Material FetchWaterMaterial()
		{
			if ((bool)meshRenderer)
			{
				material = meshRenderer.sharedMaterial;
				return material;
			}
			return null;
		}

		public void ApplyInstancedProperties()
		{
			if (props != null)
			{
				meshRenderer.SetPropertyBlock(props);
			}
		}

		public bool CanTouch(Vector3 position)
		{
			return Buoyancy.CanTouchWater(position, this);
		}

		public void AssignMesh(Mesh mesh)
		{
			if ((bool)meshFilter)
			{
				meshFilter.sharedMesh = mesh;
			}
		}

		public void AssignMaterial(Material newMaterial)
		{
			if ((bool)meshRenderer)
			{
				meshRenderer.sharedMaterial = newMaterial;
			}
			material = newMaterial;
		}

		public static WaterObject New(Material waterMaterial = null, Mesh mesh = null)
		{
			WaterObject component = new GameObject("Water Object", typeof(MeshFilter), typeof(MeshRenderer), typeof(WaterObject))
			{
				layer = LayerMask.NameToLayer("Water")
			}.GetComponent<WaterObject>();
			component.meshRenderer = component.gameObject.GetComponent<MeshRenderer>();
			component.meshFilter = component.gameObject.GetComponent<MeshFilter>();
			component.meshFilter.sharedMesh = mesh;
			component.meshRenderer.sharedMaterial = waterMaterial;
			component.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			component.material = waterMaterial;
			return component;
		}

		public static WaterObject Find(Vector3 position, bool rotationSupport)
		{
			Ray ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
			foreach (WaterObject instance in Instances)
			{
				if (rotationSupport)
				{
					ray.origin = instance.transform.InverseTransformPoint(ray.origin);
					if (instance.meshFilter.sharedMesh.bounds.IntersectRay(ray))
					{
						return instance;
					}
				}
				else if (instance.meshRenderer.bounds.IntersectRay(ray))
				{
					return instance;
				}
			}
			return null;
		}
	}
}
