using System;
using Features.DeviceModule.Scripts.DeviceData;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	[Serializable]
	public class GameStatusData
	{
		[field: SerializeField]
		public SerializableDictionary<Features.DeviceModule.Scripts.DeviceData.DeviceType, PlatformGameStatusData> PlatformsData { get; private set; }
	}
}
