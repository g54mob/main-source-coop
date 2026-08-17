using UnityEngine;

namespace NomadDrive.Features.Rope.Scripts
{
	public class MastBend : MonoBehaviour
	{
		public Transform top;

		public Transform bottom;

		public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		private void Start()
		{
			MakeSkinMesh();
		}

		public void UnSkin()
		{
			SkinnedMeshRenderer component = GetComponent<SkinnedMeshRenderer>();
			if ((bool)component)
			{
				Material[] sharedMaterials = component.sharedMaterials;
				Object.DestroyImmediate(component);
				base.gameObject.AddComponent<MeshRenderer>().sharedMaterials = sharedMaterials;
			}
		}

		public void MakeSkinMesh()
		{
			Material[] materials = null;
			MeshFilter component = GetComponent<MeshFilter>();
			SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			if ((bool)skinnedMeshRenderer)
			{
				UnSkin();
				skinnedMeshRenderer = null;
			}
			if (skinnedMeshRenderer == null)
			{
				MeshRenderer component2 = GetComponent<MeshRenderer>();
				materials = component2.sharedMaterials;
				skinnedMeshRenderer = base.gameObject.AddComponent<SkinnedMeshRenderer>();
				if ((bool)component2)
				{
					Object.DestroyImmediate(component2);
				}
			}
			Mesh sharedMesh = component.sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			_ = sharedMesh.bounds.min;
			_ = sharedMesh.bounds.max;
			Vector3 vector = base.transform.InverseTransformPoint(top.position);
			Vector3 vector2 = base.transform.InverseTransformPoint(bottom.position);
			BoneWeight[] array = new BoneWeight[sharedMesh.vertexCount];
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector3 = vertices[i];
				if (vector3.y < vector2.y)
				{
					array[i].boneIndex0 = 0;
					array[i].boneIndex1 = 1;
					array[i].weight0 = 1f;
					array[i].weight1 = 0f;
				}
				else if (vector3.y < vector.y)
				{
					array[i].boneIndex0 = 0;
					array[i].boneIndex1 = 1;
					float time = (vector.y - vector3.y) / (vector.y - vector2.y);
					array[i].weight0 = curve.Evaluate(time);
					array[i].weight1 = 1f - array[i].weight0;
				}
				else
				{
					array[i].boneIndex0 = 0;
					array[i].boneIndex1 = 1;
					array[i].weight0 = 0f;
					array[i].weight1 = 1f;
				}
			}
			sharedMesh.boneWeights = array;
			Transform[] array2 = new Transform[2];
			Matrix4x4[] array3 = new Matrix4x4[2];
			array2[1] = top;
			array3[1] = top.worldToLocalMatrix * base.transform.localToWorldMatrix;
			array2[0] = bottom;
			array3[0] = bottom.worldToLocalMatrix * base.transform.localToWorldMatrix;
			sharedMesh.bindposes = array3;
			skinnedMeshRenderer.bones = array2;
			skinnedMeshRenderer.updateWhenOffscreen = true;
			skinnedMeshRenderer.materials = materials;
		}
	}
}
