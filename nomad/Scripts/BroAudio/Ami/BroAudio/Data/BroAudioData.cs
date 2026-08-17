using System;
using System.Collections.Generic;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[Obsolete("No need for this to exist anymore")]
	public class BroAudioData : ScriptableObject
	{
		[Obsolete("No need for this to exist anymore")]
		[SerializeField]
		[ReadOnly]
		public string _version;

		[Obsolete("No need for this to exist anymore", true)]
		[SerializeField]
		private List<AudioAsset> _assets = new List<AudioAsset>();

		[Obsolete("No need for this to exist anymore")]
		public IReadOnlyList<IAudioAsset> Assets => _assets;
	}
}
