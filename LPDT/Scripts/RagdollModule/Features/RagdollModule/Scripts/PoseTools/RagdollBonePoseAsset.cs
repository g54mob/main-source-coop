using System.Collections.Generic;
using UnityEngine;

namespace Features.RagdollModule.Scripts.PoseTools
{
	[CreateAssetMenu(fileName = "RagdollBonePoseAsset_Default", menuName = "Configurations/RagdollModule/Ragdoll Bone Pose")]
	public sealed class RagdollBonePoseAsset : ScriptableObject
	{
		[SerializeField]
		private string _sourceRootName;

		[SerializeField]
		private List<RagdollBonePoseEntry> _entries = new List<RagdollBonePoseEntry>();

		public string SourceRootName => _sourceRootName;

		public IReadOnlyList<RagdollBonePoseEntry> Entries => _entries;

		public int BoneCount => _entries?.Count ?? 0;

		public void SetPose(string sourceRootName, IEnumerable<RagdollBonePoseEntry> entries)
		{
			_sourceRootName = sourceRootName;
			_entries.Clear();
			foreach (RagdollBonePoseEntry entry in entries)
			{
				_entries.Add(new RagdollBonePoseEntry(entry));
			}
		}
	}
}
