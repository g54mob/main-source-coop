using Features.ItemSpawnerModule;
using Global.SerializableDictionary;
using TMPro;
using UnityEngine;

namespace Features.StreamersSupportModule.Scripts
{
	public class StreamerStand : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _streamerNameText;

		[SerializeField]
		private SerializableDictionary<StreamersStandType, ItemSpawnerBase> _itemSpawners;

		private int _standIndex;

		public int StandIndex => _standIndex;

		public void Initialize(int standIndex)
		{
			_standIndex = standIndex;
		}

		public void ApplyStandData(StreamersStandData data)
		{
			_streamerNameText.SetText(data.StreamerName);
			if (_itemSpawners.TryGetValue(data.StreamersStandType, out var value))
			{
				value.Spawn();
			}
		}
	}
}
