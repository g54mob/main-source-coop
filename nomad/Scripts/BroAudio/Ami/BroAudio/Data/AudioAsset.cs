using System;
using System.Collections.Generic;
using Ami.BroAudio.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ami.BroAudio.Data
{
	public class AudioAsset : ScriptableObject, IAudioAsset
	{
		public static class NameOf
		{
			public const string Group = "_group";
		}

		[SerializeField]
		[Obsolete("Entities are only here for backwards compatibility.", true)]
		private AudioEntity_LEGACY[] Entities;

		[SerializeField]
		[Obsolete("Here JUST in case the SoundIDs are not fully converted and need some way to be converted back")]
		private List<AudioEntity> ConvertedEntities;

		[SerializeField]
		[FormerlySerializedAs("Group")]
		private PlaybackGroup _group;

		private PlaybackGroup _upperGroup;

		private bool _hasGroupLinked;

		public PlaybackGroup PlaybackGroup
		{
			get
			{
				if (!_hasGroupLinked)
				{
					LinkPlaybackGroup(SoundManager.Instance.Setting.GlobalPlaybackGroup);
					_hasGroupLinked = true;
				}
				if (!_group)
				{
					return _upperGroup;
				}
				return _group;
			}
		}

		[Obsolete("Entities are only here for backwards compatibility.", true)]
		private int EntitiesCount => Entities.Length;

		public void LinkPlaybackGroup(PlaybackGroup upperGroup)
		{
			if (_group != null)
			{
				_group.SetParent(upperGroup);
			}
			else
			{
				_upperGroup = upperGroup;
			}
		}

		[Obsolete("Should only be used during conversion")]
		public bool TryGetEntityFromId(int id, out AudioEntity entity)
		{
			List<AudioEntity> convertedEntities = ConvertedEntities;
			if (convertedEntities != null && convertedEntities.Count > 0)
			{
				foreach (AudioEntity convertedEntity in ConvertedEntities)
				{
					if (!(convertedEntity == null) && convertedEntity.ID == id)
					{
						entity = convertedEntity;
						return true;
					}
				}
			}
			entity = null;
			return false;
		}

		[Obsolete("Clearing down after ID upgrades are all done", true)]
		public void ClearStoredEntities()
		{
			if (Entities != null)
			{
				Entities = new AudioEntity_LEGACY[0];
			}
			if (ConvertedEntities != null)
			{
				ConvertedEntities.Clear();
			}
		}
	}
}
