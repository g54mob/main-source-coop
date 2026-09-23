using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Customization
{
	public sealed class BoneMeshBounds
	{
		private const float DominanceThreshold = 0.5f;

		private readonly Dictionary<Transform, BoneBox> perBone = new Dictionary<Transform, BoneBox>();

		private readonly HashSet<Transform> notOriented = new HashSet<Transform>();

		public int RendererCount { get; private set; }

		public int MeasuredBoneCount => perBone.Count;

		public int OrientedBoneCount { get; private set; }

		public bool TryGet(Transform bone, out BoneBox box)
		{
			box = default(BoneBox);
			if (bone != null)
			{
				return perBone.TryGetValue(bone, out box);
			}
			return false;
		}

		public static BoneMeshBounds Measure(GameObject root, IEnumerable<Transform> bones, Func<Renderer, bool> ignore = null)
		{
			BoneMeshBounds boneMeshBounds = new BoneMeshBounds();
			if (root == null || bones == null)
			{
				return boneMeshBounds;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			foreach (Transform bone in bones)
			{
				if (bone != null)
				{
					hashSet.Add(bone);
				}
			}
			if (hashSet.Count == 0)
			{
				return boneMeshBounds;
			}
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null) && (ignore == null || !ignore(renderer)))
				{
					boneMeshBounds.RendererCount++;
					if (!(renderer is SkinnedMeshRenderer renderer2) || !boneMeshBounds.AddSkinned(renderer2, hashSet))
					{
						boneMeshBounds.AddRigid(renderer, hashSet, root.transform);
					}
				}
			}
			boneMeshBounds.CountOriented();
			return boneMeshBounds;
		}

		private void AddRigid(Renderer renderer, HashSet<Transform> boneSet, Transform root)
		{
			Transform transform = NearestBone(renderer.transform, boneSet, root);
			if (!(transform == null))
			{
				Transform transform2 = renderer.transform;
				Bounds localBounds = renderer.localBounds;
				if (!perBone.ContainsKey(transform))
				{
					Vector3 extents = localBounds.extents;
					Vector3 size = new Vector3(2f * transform.InverseTransformVector(transform2.TransformVector(new Vector3(extents.x, 0f, 0f))).magnitude, 2f * transform.InverseTransformVector(transform2.TransformVector(new Vector3(0f, extents.y, 0f))).magnitude, 2f * transform.InverseTransformVector(transform2.TransformVector(new Vector3(0f, 0f, extents.z))).magnitude);
					perBone[transform] = new BoneBox(transform.InverseTransformPoint(transform2.TransformPoint(localBounds.center)), size, Quaternion.Inverse(transform.rotation) * transform2.rotation, oriented: true);
				}
				else
				{
					Bounds hull = ToAxisAligned(perBone[transform]);
					EncapsulateCorners(ref hull, transform, transform2, localBounds);
					perBone[transform] = new BoneBox(hull.center, hull.size, Quaternion.identity, oriented: false);
					notOriented.Add(transform);
				}
			}
		}

		private bool AddSkinned(SkinnedMeshRenderer renderer, HashSet<Transform> boneSet)
		{
			Mesh sharedMesh = renderer.sharedMesh;
			Transform[] bones = renderer.bones;
			if (sharedMesh == null || bones == null || bones.Length == 0 || !sharedMesh.isReadable)
			{
				return false;
			}
			Vector3[] vertices = sharedMesh.vertices;
			BoneWeight[] boneWeights = sharedMesh.boneWeights;
			Matrix4x4[] bindposes = sharedMesh.bindposes;
			if (boneWeights.Length != vertices.Length || bindposes.Length < bones.Length)
			{
				return false;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			Dictionary<Transform, Bounds> dictionary = new Dictionary<Transform, Bounds>();
			for (int i = 0; i < vertices.Length; i++)
			{
				int num = Dominant(boneWeights[i]);
				if (num < 0 || num >= bones.Length)
				{
					continue;
				}
				Transform transform = bones[num];
				if (!(transform == null) && boneSet.Contains(transform))
				{
					Vector3 vector = bindposes[num].MultiplyPoint3x4(vertices[i]);
					if (hashSet.Add(transform))
					{
						BoneBox value2;
						Bounds value = (perBone.TryGetValue(transform, out value2) ? ToAxisAligned(value2) : new Bounds(vector, Vector3.zero));
						value.Encapsulate(vector);
						dictionary[transform] = value;
					}
					else
					{
						Bounds value3 = dictionary[transform];
						value3.Encapsulate(vector);
						dictionary[transform] = value3;
					}
				}
			}
			foreach (KeyValuePair<Transform, Bounds> item in dictionary)
			{
				perBone[item.Key] = new BoneBox(item.Value.center, item.Value.size, Quaternion.identity, oriented: false);
				notOriented.Add(item.Key);
			}
			return true;
		}

		private void CountOriented()
		{
			OrientedBoneCount = 0;
			foreach (KeyValuePair<Transform, BoneBox> item in perBone)
			{
				if (item.Value.Oriented && !notOriented.Contains(item.Key))
				{
					OrientedBoneCount++;
				}
			}
		}

		private static Bounds ToAxisAligned(BoneBox box)
		{
			Vector3 vector = box.Size * 0.5f;
			Bounds result = new Bounds(box.Center, Vector3.zero);
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector2 = new Vector3(((i & 1) == 0) ? (0f - vector.x) : vector.x, ((i & 2) == 0) ? (0f - vector.y) : vector.y, ((i & 4) == 0) ? (0f - vector.z) : vector.z);
				result.Encapsulate(box.Center + box.Rotation * vector2);
			}
			return result;
		}

		private static void EncapsulateCorners(ref Bounds hull, Transform bone, Transform from, Bounds local)
		{
			Vector3 center = local.center;
			Vector3 extents = local.extents;
			for (int i = 0; i < 8; i++)
			{
				Vector3 position = new Vector3(center.x + (((i & 1) == 0) ? (0f - extents.x) : extents.x), center.y + (((i & 2) == 0) ? (0f - extents.y) : extents.y), center.z + (((i & 4) == 0) ? (0f - extents.z) : extents.z));
				hull.Encapsulate(bone.InverseTransformPoint(from.TransformPoint(position)));
			}
		}

		private static Transform NearestBone(Transform from, HashSet<Transform> boneSet, Transform root)
		{
			Transform transform = from;
			while (transform != null)
			{
				if (boneSet.Contains(transform))
				{
					return transform;
				}
				if (transform == root)
				{
					break;
				}
				transform = transform.parent;
			}
			return null;
		}

		private static int Dominant(BoneWeight w)
		{
			if (w.weight0 > 0.5f)
			{
				return w.boneIndex0;
			}
			if (w.weight1 > 0.5f)
			{
				return w.boneIndex1;
			}
			if (w.weight2 > 0.5f)
			{
				return w.boneIndex2;
			}
			if (w.weight3 > 0.5f)
			{
				return w.boneIndex3;
			}
			return -1;
		}
	}
}
