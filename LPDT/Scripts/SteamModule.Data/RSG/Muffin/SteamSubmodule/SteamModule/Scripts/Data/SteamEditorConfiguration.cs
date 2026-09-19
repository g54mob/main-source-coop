using System.Collections.Generic;
using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace RSG.Muffin.SteamSubmodule.SteamModule.Scripts.Data
{
	[CreateAssetMenu(fileName = "SteamEditorConfiguration_Default", menuName = "Configurations/SteamModule/SteamEditorConfiguration")]
	public class SteamEditorConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<Features.DeviceModule.Scripts.DeviceData.DeviceType> DevicesWithSteamChecking { get; private set; }
	}
}
