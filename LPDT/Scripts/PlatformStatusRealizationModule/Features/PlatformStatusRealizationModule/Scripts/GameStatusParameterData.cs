using System;
using Features.DeviceModule.Scripts.DeviceData;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	[Serializable]
	public class GameStatusParameterData
	{
		[field: SerializeField]
		public SerializableDictionary<Features.DeviceModule.Scripts.DeviceData.DeviceType, PlatformGameStatusParameterData> PlatformsData { get; private set; }
	}
}
