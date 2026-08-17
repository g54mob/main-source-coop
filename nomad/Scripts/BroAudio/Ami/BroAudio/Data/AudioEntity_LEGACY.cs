using System;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[Serializable]
	[Obsolete("AudioEntity_LEGACY is no longer used. Use AudioEntity instead.", true)]
	internal class AudioEntity_LEGACY
	{
		[SerializeField]
		public MulticlipsPlayMode MulticlipsPlayMode;

		[SerializeField]
		public PlaybackGroup _group;

		private PlaybackGroup _upperGroup;

		public BroAudioClip[] Clips;

		public bool UseAddressables;

		[field: SerializeField]
		public string Name { get; private set; }

		[field: SerializeField]
		public int ID { get; private set; }

		[field: SerializeField]
		public float MasterVolume { get; private set; }

		[field: SerializeField]
		public bool Loop { get; private set; }

		[field: SerializeField]
		public bool SeamlessLoop { get; private set; }

		[field: SerializeField]
		public float TransitionTime { get; private set; }

		[field: SerializeField]
		public SpatialSetting SpatialSetting { get; private set; }

		[field: SerializeField]
		public int Priority { get; private set; }

		[field: SerializeField]
		public float Pitch { get; private set; }

		[field: SerializeField]
		public float PitchRandomRange { get; private set; }

		[field: SerializeField]
		public float VolumeRandomRange { get; private set; }

		[field: SerializeField]
		public RandomFlag RandomFlags { get; private set; }
	}
}
