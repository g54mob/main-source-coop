using System;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[Serializable]
	public class IndexedRoomLight
	{
		[Tooltip("Index referenced by room lighting overrides.")]
		[SerializeField]
		private int _lightIndex;

		[SerializeField]
		private Light _light;

		public int LightIndex => _lightIndex;

		public Light Light => _light;
	}
}
