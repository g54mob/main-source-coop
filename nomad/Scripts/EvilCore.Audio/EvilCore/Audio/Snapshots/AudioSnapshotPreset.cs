using UnityEngine;
using UnityEngine.Audio;

namespace EvilCore.Audio.Snapshots
{
	[CreateAssetMenu(fileName = "AudioSnapshotPreset", menuName = "EvilCore/Audio/Audio Snapshot Preset")]
	public class AudioSnapshotPreset : ScriptableObject
	{
		[Tooltip("Snapshot asset embedded in the AudioMixer. Author snapshots inside the mixer first, then drag one here.")]
		public AudioMixerSnapshot snapshot;

		[Tooltip("Default seconds to take when transitioning to this snapshot. Per-call override still wins.")]
		[Min(0f)]
		public float defaultTransitionSeconds = 0.5f;

		[Tooltip("Optional human-readable label for tooling.")]
		public string displayName;

		public bool IsValid => snapshot != null;
	}
}
