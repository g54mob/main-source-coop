using System.Collections.Generic;
using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[ExecuteInEditMode]
	[AddComponentMenu("Windshield Rain Asset/WindshieldMeshRenderer")]
	public class WindshieldMeshRenderer : MonoBehaviour
	{
		public static List<WindshieldMeshRenderer> ActiveRenderers = new List<WindshieldMeshRenderer>();

		public Mesh mesh;

		public Material material;

		private bool added;

		private void Init()
		{
			if (!mesh)
			{
				MeshFilter component = GetComponent<MeshFilter>();
				if ((bool)component)
				{
					mesh = component.sharedMesh;
				}
			}
			if (!material)
			{
				MeshRenderer component2 = GetComponent<MeshRenderer>();
				if ((bool)component2)
				{
					material = component2.sharedMaterial;
				}
			}
			if (!added)
			{
				ActiveRenderers.Add(this);
				added = true;
			}
		}

		private void OnEnable()
		{
			Init();
		}

		private void OnDisable()
		{
			ActiveRenderers.Remove(this);
			added = false;
		}

		private void OnDestroy()
		{
			ActiveRenderers.Remove(this);
			added = false;
		}
	}
}
