using System;
using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;
using UnityEngine.Events;

namespace PaintIn3D
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwPaintableMesh")]
	[AddComponentMenu("CW/Paint in 3D/CW Paintable Mesh")]
	public class CwPaintableMesh : CwMeshModel
	{
		public enum ActivationType
		{
			Awake = 0,
			OnEnable = 1,
			Start = 2,
			OnFirstUse = 3
		}

		public enum MaterialApplicationType
		{
			PropertyBlock = 0,
			ClonerAndTextures = 1
		}

		[SerializeField]
		private ActivationType activation = ActivationType.Start;

		[SerializeField]
		private MaterialApplicationType materialApplication;

		[SerializeField]
		private List<Renderer> otherRenderers;

		[SerializeField]
		private UnityEvent onActivating;

		[SerializeField]
		private UnityEvent onActivated;

		[SerializeField]
		private UnityEvent onDeactivating;

		[SerializeField]
		private UnityEvent onDeactivated;

		[SerializeField]
		private bool activated;

		[NonSerialized]
		private HashSet<CwPaintableMeshTexture> paintableTextures = new HashSet<CwPaintableMeshTexture>();

		[NonSerialized]
		private List<Mesh> tempDilateMeshes;

		[NonSerialized]
		private static List<CwMaterialCloner> tempMaterialCloners = new List<CwMaterialCloner>();

		[NonSerialized]
		private static List<CwPaintableTexture> tempPaintableTextures = new List<CwPaintableTexture>();

		[NonSerialized]
		private static List<CwPaintableMeshTexture> tempPaintableMeshTextures = new List<CwPaintableMeshTexture>();

		public ActivationType Activation
		{
			get
			{
				return activation;
			}
			set
			{
				activation = value;
			}
		}

		public MaterialApplicationType MaterialApplication
		{
			get
			{
				return materialApplication;
			}
			set
			{
				materialApplication = value;
			}
		}

		public List<Renderer> OtherRenderers
		{
			get
			{
				if (otherRenderers == null)
				{
					otherRenderers = new List<Renderer>();
				}
				return otherRenderers;
			}
			set
			{
				otherRenderers = value;
			}
		}

		public UnityEvent OnActivating
		{
			get
			{
				if (onActivating == null)
				{
					onActivating = new UnityEvent();
				}
				return onActivating;
			}
		}

		public UnityEvent OnActivated
		{
			get
			{
				if (onActivated == null)
				{
					onActivated = new UnityEvent();
				}
				return onActivated;
			}
		}

		public UnityEvent OnDeactivating
		{
			get
			{
				if (onDeactivating == null)
				{
					onDeactivating = new UnityEvent();
				}
				return onDeactivating;
			}
		}

		public UnityEvent OnDeactivated
		{
			get
			{
				if (onDeactivated == null)
				{
					onDeactivated = new UnityEvent();
				}
				return onDeactivated;
			}
		}

		public override bool IsActivated => activated;

		public HashSet<CwPaintableMeshTexture> PaintableTextures => paintableTextures;

		public override void RemoveComponents()
		{
			GetComponents(tempPaintableTextures);
			for (int num = paintableTextures.Count - 1; num >= 0; num--)
			{
				CwPaintableTexture cwPaintableTexture = tempPaintableTextures[num];
				cwPaintableTexture.Deactivate();
				CwHelper.Destroy(cwPaintableTexture);
			}
			GetComponents(tempMaterialCloners);
			for (int num2 = tempMaterialCloners.Count - 1; num2 >= 0; num2--)
			{
				CwMaterialCloner cwMaterialCloner = tempMaterialCloners[num2];
				cwMaterialCloner.Deactivate();
				CwHelper.Destroy(cwMaterialCloner);
			}
			CwHelper.Destroy(this);
		}

		[ContextMenu("Activate")]
		public override void Activate()
		{
			DoActivate();
		}

		protected virtual void DoActivate()
		{
			if (onActivating != null)
			{
				onActivating.Invoke();
			}
			if (materialApplication == MaterialApplicationType.ClonerAndTextures)
			{
				GetComponents(tempMaterialCloners);
				for (int num = tempMaterialCloners.Count - 1; num >= 0; num--)
				{
					tempMaterialCloners[num].Activate();
				}
			}
			AddPaintableTextures(base.transform);
			foreach (CwPaintableMeshTexture paintableTexture in paintableTextures)
			{
				paintableTexture.Activate();
			}
			activated = true;
			if (onActivated != null)
			{
				onActivated.Invoke();
			}
		}

		private void AddPaintableTextures(Transform root)
		{
			root.GetComponents(tempPaintableMeshTextures);
			foreach (CwPaintableMeshTexture tempPaintableMeshTexture in tempPaintableMeshTextures)
			{
				paintableTextures.Add(tempPaintableMeshTexture);
			}
			tempPaintableMeshTextures.Clear();
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.GetComponent<CwPaintableMesh>() == null)
				{
					AddPaintableTextures(child);
				}
			}
		}

		[ContextMenu("Deactivate")]
		public void Deactivate()
		{
			if (activated)
			{
				activated = false;
				DoDeactivate();
			}
		}

		public List<Mesh> GetDilateMeshes()
		{
			if (tempDilateMeshes == null)
			{
				tempDilateMeshes = new List<Mesh>();
				TryPopulateDilateMeshes(base.CachedRenderer);
				if (otherRenderers != null)
				{
					foreach (Renderer otherRenderer in otherRenderers)
					{
						TryPopulateDilateMeshes(otherRenderer);
					}
				}
			}
			return tempDilateMeshes;
		}

		private void TryPopulateDilateMeshes(Renderer renderer)
		{
			if (renderer != null)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh != null)
				{
					tempDilateMeshes.Add(component.sharedMesh);
				}
			}
		}

		protected virtual void DoDeactivate()
		{
			if (onDeactivating != null)
			{
				onDeactivating.Invoke();
			}
			foreach (CwPaintableMeshTexture paintableTexture in paintableTextures)
			{
				if (paintableTexture != null)
				{
					paintableTexture.Deactivate();
				}
			}
			paintableTextures.Clear();
			if (onDeactivated != null)
			{
				onDeactivated.Invoke();
			}
		}

		public void ClearAll(Color color)
		{
			ClearAll(null, color);
		}

		public void ClearAll(Texture texture, Color color)
		{
			if (!activated)
			{
				return;
			}
			foreach (CwPaintableMeshTexture paintableTexture in paintableTextures)
			{
				paintableTexture.Clear(texture, color);
			}
		}

		public void Register(CwPaintableMeshTexture paintableTexture)
		{
			paintableTextures.Add(paintableTexture);
		}

		public void Unregister(CwPaintableMeshTexture paintableTexture)
		{
			paintableTextures.Remove(paintableTexture);
		}

		public override List<CwPaintableTexture> FindPaintableTextures(CwGroup group)
		{
			tempPaintableTextures.Clear();
			foreach (CwPaintableMeshTexture paintableTexture in paintableTextures)
			{
				if ((int)paintableTexture.Group == (int)group)
				{
					tempPaintableTextures.Add(paintableTexture);
				}
			}
			return tempPaintableTextures;
		}

		protected virtual void Awake()
		{
			if (activation == ActivationType.Awake && !activated)
			{
				Activate();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (activation == ActivationType.OnEnable && !activated)
			{
				Activate();
			}
			CwPaintableManager.GetOrCreateInstance();
		}

		protected virtual void Start()
		{
			if (activation == ActivationType.Start && !activated)
			{
				Activate();
			}
		}
	}
}
