using System.Collections.Generic;
using UnityEngine;

namespace Features.RagdollModule.Scripts.PoseTools
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Tools/Ragdoll Bone Pose Recorder")]
	public sealed class RagdollBonePoseRecorder : MonoBehaviour
	{
		[SerializeField]
		private Transform _boneRoot;

		[SerializeField]
		private SkinnedMeshRenderer _skinnedMeshRenderer;

		[SerializeField]
		private RagdollBonePoseAsset _lastSavedPose;

		[SerializeField]
		private bool _fallbackToChildTransforms = true;

		public Transform BoneRoot => _boneRoot;

		public SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;

		public RagdollBonePoseAsset LastSavedPose => _lastSavedPose;

		public RagdollBonePoseCaptureResult CaptureCurrentPose()
		{
			if (_lastSavedPose == null)
			{
				return RagdollBonePoseCaptureResult.Failed("Assign a pose asset before capturing.");
			}
			if (!TryGetBoneRoot(out var root))
			{
				return RagdollBonePoseCaptureResult.Failed("Assign a bone root or a SkinnedMeshRenderer with rootBone.");
			}
			List<Transform> list = CollectBones();
			if (list.Count == 0)
			{
				return RagdollBonePoseCaptureResult.Failed("No bones found under '" + root.name + "'.");
			}
			List<RagdollBonePoseEntry> list2 = new List<RagdollBonePoseEntry>(list.Count);
			foreach (Transform item in list)
			{
				list2.Add(new RagdollBonePoseEntry(GetRelativePath(root, item), GetHierarchyPath(root, item), item.localPosition, item.localRotation, item.localScale));
			}
			_lastSavedPose.SetPose(root.name, list2);
			return RagdollBonePoseCaptureResult.Captured(list2.Count);
		}

		public RagdollBonePoseApplyResult ApplyLastSavedPose()
		{
			return ApplyPose(_lastSavedPose);
		}

		public RagdollBonePoseApplyResult ApplyPose(RagdollBonePoseAsset pose)
		{
			if (pose == null)
			{
				return RagdollBonePoseApplyResult.Failed("Assign a saved pose before applying.");
			}
			if (!TryGetBoneRoot(out var root))
			{
				return RagdollBonePoseApplyResult.Failed("Assign a bone root or a SkinnedMeshRenderer with rootBone.");
			}
			int num = 0;
			int num2 = 0;
			foreach (RagdollBonePoseEntry entry in pose.Entries)
			{
				Transform transform = ResolveBone(root, entry);
				if (transform == null)
				{
					num2++;
					continue;
				}
				transform.localPosition = entry.LocalPosition;
				transform.localRotation = entry.LocalRotation;
				transform.localScale = entry.LocalScale;
				num++;
			}
			return RagdollBonePoseApplyResult.Applied(num, num2);
		}

		public List<Transform> ResolvePoseTargets(RagdollBonePoseAsset pose)
		{
			List<Transform> list = new List<Transform>();
			if (pose == null || !TryGetBoneRoot(out var root))
			{
				return list;
			}
			foreach (RagdollBonePoseEntry entry in pose.Entries)
			{
				Transform transform = ResolveBone(root, entry);
				if (transform != null && !list.Contains(transform))
				{
					list.Add(transform);
				}
			}
			return list;
		}

		public List<Transform> CollectBones()
		{
			List<Transform> list = new List<Transform>();
			if (!TryGetBoneRoot(out var root))
			{
				return list;
			}
			AddBone(list, root);
			if (_skinnedMeshRenderer != null && _skinnedMeshRenderer.bones != null)
			{
				Transform[] bones = _skinnedMeshRenderer.bones;
				foreach (Transform transform in bones)
				{
					if (!(transform == null) && IsSameOrChildOf(transform, root))
					{
						AddBone(list, transform);
					}
				}
			}
			if (list.Count <= 1 && _fallbackToChildTransforms)
			{
				Transform[] bones = root.GetComponentsInChildren<Transform>(includeInactive: true);
				foreach (Transform bone in bones)
				{
					AddBone(list, bone);
				}
			}
			return list;
		}

		public bool TryAutoFillReferences()
		{
			bool result = false;
			if (_skinnedMeshRenderer == null)
			{
				_skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(includeInactive: true);
				result = _skinnedMeshRenderer != null;
			}
			if (_boneRoot == null && _skinnedMeshRenderer != null && _skinnedMeshRenderer.rootBone != null)
			{
				_boneRoot = _skinnedMeshRenderer.rootBone;
				result = true;
			}
			return result;
		}

		private bool TryGetBoneRoot(out Transform root)
		{
			root = ((_boneRoot != null) ? _boneRoot : ((_skinnedMeshRenderer != null) ? _skinnedMeshRenderer.rootBone : null));
			return root != null;
		}

		private static void AddBone(List<Transform> bones, Transform bone)
		{
			if (bone != null && !bones.Contains(bone))
			{
				bones.Add(bone);
			}
		}

		private static bool IsSameOrChildOf(Transform transform, Transform parent)
		{
			Transform transform2 = transform;
			while (transform2 != null)
			{
				if (transform2 == parent)
				{
					return true;
				}
				transform2 = transform2.parent;
			}
			return false;
		}

		private static string GetRelativePath(Transform root, Transform target)
		{
			if (target == root)
			{
				return string.Empty;
			}
			Stack<string> stack = new Stack<string>();
			Transform transform = target;
			while (transform != null && transform != root)
			{
				stack.Push(transform.name);
				transform = transform.parent;
			}
			return string.Join("/", stack);
		}

		private static string GetHierarchyPath(Transform root, Transform target)
		{
			if (target == root)
			{
				return string.Empty;
			}
			Stack<int> stack = new Stack<int>();
			Transform transform = target;
			while (transform != null && transform != root)
			{
				stack.Push(transform.GetSiblingIndex());
				transform = transform.parent;
			}
			return string.Join("/", stack);
		}

		private static Transform ResolveBone(Transform root, RagdollBonePoseEntry entry)
		{
			if (string.IsNullOrEmpty(entry.RelativePath))
			{
				return root;
			}
			Transform transform = root.Find(entry.RelativePath);
			if (transform != null)
			{
				return transform;
			}
			return ResolveByHierarchyPath(root, entry.HierarchyPath);
		}

		private static Transform ResolveByHierarchyPath(Transform root, string hierarchyPath)
		{
			if (string.IsNullOrEmpty(hierarchyPath))
			{
				return root;
			}
			Transform transform = root;
			string[] array = hierarchyPath.Split('/');
			for (int i = 0; i < array.Length; i++)
			{
				if (!int.TryParse(array[i], out var result))
				{
					return null;
				}
				if (result < 0 || result >= transform.childCount)
				{
					return null;
				}
				transform = transform.GetChild(result);
			}
			return transform;
		}
	}
}
