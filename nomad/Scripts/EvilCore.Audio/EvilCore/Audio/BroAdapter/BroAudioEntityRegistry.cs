using System.Collections.Generic;
using Ami.BroAudio;
using Ami.BroAudio.Data;
using UnityEngine;

namespace EvilCore.Audio.BroAdapter
{
	[CreateAssetMenu(fileName = "BroAudioEntityRegistry", menuName = "EvilCore/Audio/Bro Audio Entity Registry")]
	public class BroAudioEntityRegistry : ScriptableObject
	{
		[SerializeField]
		private List<AudioEntity> _entities = new List<AudioEntity>();

		private Dictionary<string, AudioEntity> _byName;

		public static BroAudioEntityRegistry ActiveInstance { get; private set; }

		public IReadOnlyList<AudioEntity> Entities => _entities;

		public void SetActive()
		{
			ActiveInstance = this;
		}

		public void ClearActive()
		{
			if (ActiveInstance == this)
			{
				ActiveInstance = null;
			}
		}

		public SoundID GetByName(string entityName)
		{
			if (string.IsNullOrEmpty(entityName))
			{
				return default(SoundID);
			}
			BuildLookupIfNeeded();
			if (_byName.TryGetValue(entityName, out var value) && value != null)
			{
				return new SoundID(value);
			}
			return default(SoundID);
		}

		public bool TryGetByName(string entityName, out SoundID id)
		{
			id = GetByName(entityName);
			return id.IsValid();
		}

		private void BuildLookupIfNeeded()
		{
			if (_byName != null)
			{
				return;
			}
			_byName = new Dictionary<string, AudioEntity>(_entities.Count);
			for (int i = 0; i < _entities.Count; i++)
			{
				AudioEntity audioEntity = _entities[i];
				if (!(audioEntity == null) && !_byName.ContainsKey(audioEntity.name))
				{
					_byName[audioEntity.name] = audioEntity;
				}
			}
		}

		private void OnEnable()
		{
			_byName = null;
		}
	}
}
