using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwAtlasMesh")]
	[AddComponentMenu("CW/Paint Core/CW Atlas Mesh")]
	public abstract class CwModel : MonoBehaviour
	{
		[SerializeField]
		protected CwHash hash;

		[SerializeField]
		private Vector3 baseScale;

		[NonSerialized]
		protected Renderer cachedRenderer;

		[NonSerialized]
		protected bool cachedRendererSet;

		[NonSerialized]
		protected Transform cachedTransform;

		[NonSerialized]
		protected GameObject cachedGameObject;

		[NonSerialized]
		protected bool prepared;

		[NonSerialized]
		private static List<Material> tempMaterials = new List<Material>();

		[NonSerialized]
		private static List<CwModel> tempModels = new List<CwModel>();

		private static LinkedList<CwModel> instances = new LinkedList<CwModel>();

		private LinkedListNode<CwModel> instancesNode;

		private static MaterialPropertyBlock properties;

		public CwHash Hash
		{
			get
			{
				return hash;
			}
			set
			{
				hash = value;
				CwSerialization.TryRegister(this, hash);
			}
		}

		public Vector3 BaseScale
		{
			get
			{
				return baseScale;
			}
			set
			{
				baseScale = value;
			}
		}

		public static LinkedList<CwModel> Instances => instances;

		public Renderer CachedRenderer
		{
			get
			{
				if (!cachedRendererSet)
				{
					CacheRenderer();
				}
				return cachedRenderer;
			}
		}

		public Transform CachedTransform => cachedTransform;

		public GameObject CachedGameObject
		{
			get
			{
				if (!cachedRendererSet)
				{
					CacheRenderer();
				}
				return cachedGameObject;
			}
		}

		public bool Prepared
		{
			get
			{
				return prepared;
			}
			set
			{
				prepared = value;
			}
		}

		public abstract bool IsActivated { get; }

		public abstract void Activate();

		public abstract void RemoveComponents();

		public void ApplyTexture(CwSlot slot, Texture texture)
		{
			if (properties == null)
			{
				properties = new MaterialPropertyBlock();
			}
			if (!cachedRendererSet)
			{
				CacheRenderer();
			}
			cachedRenderer.GetPropertyBlock(properties, slot.Index);
			properties.SetTexture(slot.Name, texture);
			cachedRenderer.SetPropertyBlock(properties, slot.Index);
		}

		public void ApplyTexture(Renderer r, CwSlot slot, Texture texture)
		{
			if (r != null)
			{
				if (properties == null)
				{
					properties = new MaterialPropertyBlock();
				}
				r.GetPropertyBlock(properties, slot.Index);
				properties.SetTexture(slot.Name, texture);
				r.SetPropertyBlock(properties, slot.Index);
			}
		}

		public static List<CwModel> FindOverlap(Vector3 position, float radius, int layerMask)
		{
			tempModels.Clear();
			foreach (CwModel instance in instances)
			{
				if (!CwHelper.IndexInMask(instance.gameObject.layer, layerMask))
				{
					continue;
				}
				Bounds bounds = instance.CachedRenderer.bounds;
				float num = radius + bounds.extents.magnitude;
				num *= num;
				if (Vector3.SqrMagnitude(position - bounds.center) < num)
				{
					tempModels.Add(instance);
					if (!instance.IsActivated)
					{
						instance.Activate();
					}
				}
			}
			return tempModels;
		}

		public abstract List<CwPaintableTexture> FindPaintableTextures(CwGroup group);

		public abstract void GetPrepared(ref Mesh mesh, ref Matrix4x4 matrix, CwCoord coord);

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
			cachedGameObject = base.gameObject;
			cachedTransform = base.transform;
			cachedRenderer = GetComponent<Renderer>();
			CwSerialization.TryRegister(this, hash);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
		}

		protected virtual void OnDestroy()
		{
			CwSerialization.TryRegister(this, default(CwHash));
		}

		protected virtual void CacheRenderer()
		{
			cachedRenderer = GetComponent<Renderer>();
			cachedRendererSet = true;
		}

		public void ScaleSize(ref int width, ref int height)
		{
			if (baseScale != Vector3.zero)
			{
				float num = base.transform.localScale.magnitude / baseScale.magnitude;
				width = Mathf.CeilToInt((float)width * num);
				height = Mathf.CeilToInt((float)height * num);
			}
		}

		public Texture GetExistingTexture(CwSlot slot)
		{
			CachedRenderer.GetSharedMaterials(tempMaterials);
			if (slot.Index >= 0 && slot.Index < tempMaterials.Count)
			{
				Material material = tempMaterials[slot.Index];
				if (material != null)
				{
					return material.GetTexture(slot.Name);
				}
			}
			return null;
		}
	}
}
