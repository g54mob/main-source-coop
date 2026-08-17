using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwProceduralSetup")]
	[AddComponentMenu("CW/Paint in 3D/CW Procedural Setup")]
	public class CwProceduralSetup : MonoBehaviour
	{
		[SerializeField]
		private Material material;

		[SerializeField]
		private float size = 1f;

		[SerializeField]
		private Mesh generatedMesh;

		public Material Material
		{
			get
			{
				return material;
			}
			set
			{
				material = value;
			}
		}

		public float Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		protected virtual void Awake()
		{
			UpdateMesh();
			base.gameObject.AddComponent<MeshFilter>().sharedMesh = generatedMesh;
			base.gameObject.AddComponent<MeshCollider>().sharedMesh = generatedMesh;
			base.gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
			base.gameObject.AddComponent<CwPaintableMesh>();
			base.gameObject.AddComponent<CwPaintableMeshTexture>().Slot = new CwSlot(0, "_MainTex");
		}

		protected virtual void OnDestroy()
		{
			Object.Destroy(generatedMesh);
		}

		private void UpdateMesh()
		{
			if (generatedMesh == null)
			{
				generatedMesh = new Mesh();
			}
			else
			{
				generatedMesh.Clear();
			}
			generatedMesh.vertices = new Vector3[4]
			{
				new Vector3(0f - size, 0f - size),
				new Vector3(size, 0f - size),
				new Vector3(0f - size, size),
				new Vector3(size, size)
			};
			generatedMesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			generatedMesh.triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
			generatedMesh.RecalculateBounds();
			generatedMesh.RecalculateNormals();
			generatedMesh.RecalculateTangents();
		}
	}
}
