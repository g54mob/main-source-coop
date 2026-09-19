using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[DisallowMultipleComponent]
	public class LevelRoomEntity : MonoBehaviour
	{
		[SerializeField]
		private RoomType _roomType;

		[Tooltip("Room origin; if empty, uses this GameObject's transform.")]
		[SerializeField]
		private Transform _roomTransform;

		[SerializeField]
		private List<IndexedRoomLight> _lights = new List<IndexedRoomLight>();

		private Dictionary<int, Light> _lightsByIndex;

		public RoomType RoomType => _roomType;

		public Transform RoomTransform
		{
			get
			{
				if (!(_roomTransform != null))
				{
					return base.transform;
				}
				return _roomTransform;
			}
		}

		public IReadOnlyList<IndexedRoomLight> Lights => _lights;

		public bool TryGetLight(int lightIndex, out Light light)
		{
			if (_lightsByIndex == null)
			{
				BuildLightsByIndex();
			}
			return _lightsByIndex.TryGetValue(lightIndex, out light);
		}

		private void BuildLightsByIndex()
		{
			_lightsByIndex = new Dictionary<int, Light>(_lights.Count);
			foreach (IndexedRoomLight light in _lights)
			{
				if (!(light.Light == null) && light.LightIndex >= 0)
				{
					_lightsByIndex[light.LightIndex] = light.Light;
				}
			}
		}
	}
}
