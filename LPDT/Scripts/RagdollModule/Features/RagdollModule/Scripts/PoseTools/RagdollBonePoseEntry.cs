using System;
using UnityEngine;

namespace Features.RagdollModule.Scripts.PoseTools
{
	[Serializable]
	public sealed class RagdollBonePoseEntry
	{
		[SerializeField]
		private string _relativePath;

		[SerializeField]
		private string _hierarchyPath;

		[SerializeField]
		private Vector3 _localPosition;

		[SerializeField]
		private Quaternion _localRotation = Quaternion.identity;

		[SerializeField]
		private Vector3 _localScale = Vector3.one;

		public string RelativePath => _relativePath;

		public string HierarchyPath => _hierarchyPath;

		public Vector3 LocalPosition => _localPosition;

		public Quaternion LocalRotation => _localRotation;

		public Vector3 LocalScale => _localScale;

		public RagdollBonePoseEntry(string relativePath, string hierarchyPath, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
		{
			_relativePath = relativePath;
			_hierarchyPath = hierarchyPath;
			_localPosition = localPosition;
			_localRotation = localRotation;
			_localScale = localScale;
		}

		public RagdollBonePoseEntry(RagdollBonePoseEntry source)
			: this(source.RelativePath, source.HierarchyPath, source.LocalPosition, source.LocalRotation, source.LocalScale)
		{
		}
	}
}
